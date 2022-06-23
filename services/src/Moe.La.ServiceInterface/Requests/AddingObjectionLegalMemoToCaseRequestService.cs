using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class AddingObjectionLegalMemoToCaseRequestService : IAddingObjectionLegalMemoToCaseRequestService
    {
        private readonly IAddingObjectionLegalMemoToCaseRequestRepository _addingObjectionLegalMemoToCaseRequestRepository;
        private readonly IRequestService _requestService;
        private readonly IResearchsConsultantService _researcherConsultantService;
        private readonly ILogger<AddingLegalMemoToHearingRequestService> _logger;
        private readonly ICaseService _caseService;
        private readonly INotificationService _notificationService;

        public AddingObjectionLegalMemoToCaseRequestService(IAddingObjectionLegalMemoToCaseRequestRepository addingObjectionLegalMemoToCaseRequestRepository,
            IRequestService requestService, IHearingLegalMemoService hearingLegalMemoService,
            ICaseService caseService,
            INotificationService notificationService,
            ILogger<AddingLegalMemoToHearingRequestService> logger, IResearchsConsultantService researcherConsultantService)
        {
            _logger = logger;
            _addingObjectionLegalMemoToCaseRequestRepository = addingObjectionLegalMemoToCaseRequestRepository;
            _requestService = requestService;
            _researcherConsultantService = researcherConsultantService;
            _caseService = caseService;
            _notificationService = notificationService;
        }

        public async Task<ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>> AddAsync(AddingObjectionLegalMemoToCaseRequestDto objectionLegalMemoRequestDto)
        {
            try
            {
                var errors = new List<string>();

                //check if hearing has any legal memos
                var caseLegalMemo = await _addingObjectionLegalMemoToCaseRequestRepository.GetByCaseAsync(objectionLegalMemoRequestDto.CaseId);
                if (caseLegalMemo != null)
                {
                    if (caseLegalMemo.Request.RequestStatus.Id != (int)RequestStatuses.Rejected)
                    {
                        errors.Add("يوجد مذكرة سابقة مرتبطة بهذه القضية");
                    }
                }

                var researcherConsultant = await _researcherConsultantService.CheckCurrentResearcherHasConsultantAsync();

                if (!researcherConsultant.Data)
                {
                    errors.Add("لايوجد مستشار معين لهذا الباحث ");
                }

                if (errors.Count > 0)
                {
                    return new ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                var request = await _addingObjectionLegalMemoToCaseRequestRepository.AddAsync(objectionLegalMemoRequestDto);

                // add request transaction
                await _requestService.AddTransactionAsync(new RequestTransactionDto { RequestId = request.Request.Id, Description = "", TransactionType = RequestTransactionTypes.AddObjectionLegalMemoToCase });

                return new ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>(true, HttpStatuses.Status201Created, request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, objectionLegalMemoRequestDto);

                return new ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>> GetAsync(int Id)
        {
            try
            {
                var entity = await _addingObjectionLegalMemoToCaseRequestRepository.GetAsync(Id);

                return new ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, Id);

                return new ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>> GetByCaseAsync(int caseId)
        {
            try
            {
                var entity = await _addingObjectionLegalMemoToCaseRequestRepository.GetByCaseAsync(caseId);

                return new ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseId);

                return new ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>> ReplyObjectionLegalMemoRequest(ReplyObjectionLegalMemoRequestDto replyObjectionLegalMemoRequestDto)
        {
            try
            {
                // save the reply note and date and request status
                var replyObjectionLegalMemoRequest = await _addingObjectionLegalMemoToCaseRequestRepository
                     .ReplyObjectionLegalMemoRequestAsync(replyObjectionLegalMemoRequestDto);


                // add request transaction
                var transactionToAdd = new RequestTransactionDto { RequestId = replyObjectionLegalMemoRequest.Id };
                if (replyObjectionLegalMemoRequest.Request.RequestStatus.Id == (int)RequestStatuses.Accepted)
                {
                    transactionToAdd.Description = "تم اعتمادالطلب";
                    transactionToAdd.TransactionType = RequestTransactionTypes.RequestAccepted;
                }
                if (replyObjectionLegalMemoRequest.Request.RequestStatus.Id == (int)RequestStatuses.Rejected)
                {
                    transactionToAdd.Description = "تم رفض الطلب";
                    transactionToAdd.TransactionType = RequestTransactionTypes.RequestRejected;
                }

                // add notification to researcher                 
                string notificationText = string.Format(replyObjectionLegalMemoRequestDto.RequestStatus == RequestStatuses.Accepted ? "تمت الموافقة من المستشار على طلب توجيه على الاعتراض للقضية رقم {0} " : "تم الرفض من المستشار  لطلب توجيه على الاعتراض للقضية رقم {0} ", replyObjectionLegalMemoRequest.CaseId);
                var notificationDto = new NotificationDto()
                {
                    SendEmailMessage = true,
                    Text = notificationText,
                    URL = "",
                    UserIds = new List<Guid> { replyObjectionLegalMemoRequest.Request.CreatedBy.Id },
                    Type = NotificationTypes.Info,
                    SendSMSMessage = false,
                };
                await _notificationService.AddAsync(notificationDto);

                // Add transaction in case
                var caseTransactionDto = new CaseTransactionDto()
                {
                    CaseId = replyObjectionLegalMemoRequest.CaseId,
                    Note = notificationText,
                    TransactionType = CaseTransactionTypes.ReplyObjectionLegalMemoRequest
                };
                await _caseService.AddTransactionAsync(caseTransactionDto);

                return new ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>(true, HttpStatuses.Status200OK, replyObjectionLegalMemoRequest);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, replyObjectionLegalMemoRequestDto);

                return new ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

    }
}
