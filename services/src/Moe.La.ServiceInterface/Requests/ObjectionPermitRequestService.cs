using Microsoft.Extensions.Logging;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class ObjectionPermitRequestService : IObjectionPermitRequestService
    {
        private readonly IObjectionPermitRequestRepository _ObjectionPermitRequestRepository;
        private readonly IRequestService _requestService;
        private readonly ICaseService _caseService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IHearingRepository _hearingRepository;

        private readonly ILogger<ObjectionPermitRequestService> _logger;

        public ObjectionPermitRequestService(
            IObjectionPermitRequestRepository ObjectionPermitRequestRepository,
            IRequestService requestService,
            ICaseService caseService,
            IUserService userService,
            INotificationService notificationService,
            IHearingRepository hearingRepository,
            ILogger<ObjectionPermitRequestService> logger)
        {
            _ObjectionPermitRequestRepository = ObjectionPermitRequestRepository;
            _logger = logger;
            _requestService = requestService;
            _caseService = caseService;
            _userService = userService;
            _notificationService = notificationService;
            _hearingRepository = hearingRepository;
        }

        public async Task<ReturnResult<ObjectionPermitRequestListItemDto>> GetAsync(int id)
        {
            try
            {
                var entity = await _ObjectionPermitRequestRepository.GetAsync(id);

                return new ReturnResult<ObjectionPermitRequestListItemDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<ObjectionPermitRequestListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ObjectionPermitRequestDetailsDto>> GetByCaseAsync(int caseId)
        {
            try
            {
                var entity = await _ObjectionPermitRequestRepository.GetByCaseAsync(caseId);

                return new ReturnResult<ObjectionPermitRequestDetailsDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseId);

                return new ReturnResult<ObjectionPermitRequestDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ObjectionPermitRequestDetailsDto>> AddAsync(ObjectionPermitRequestDto ObjectionPermitRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ObjectionPermitRequestValidator(), ObjectionPermitRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ObjectionPermitRequestDetailsDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                var ObjectionPermitRequest = await _ObjectionPermitRequestRepository.AddAsync(ObjectionPermitRequestDto);

                // Add request Transaction
                var requestTransactionDto = new RequestTransactionDto()
                {
                    RequestId = ObjectionPermitRequest.Id,
                    RequestStatus = RequestStatuses.New,
                    TransactionType = RequestTransactionTypes.Create,
                    Description = "تم إضافة طلب توجيه بالاعتراض او الاكتفاء بالحكم"
                };

                await _requestService.AddTransactionAsync(requestTransactionDto);

                // Add case Transaction
                var caseTransactionDto = new CaseTransactionDto()
                {
                    CaseId = ObjectionPermitRequestDto.CaseId,
                    TransactionType = CaseTransactionTypes.AddObjectionPermitRequest,
                    Note = ObjectionPermitRequestDto.Note
                };

                await _caseService.AddTransactionAsync(caseTransactionDto);

                //add notification
                var userIds = new List<Guid>();

                //add litigationManager
                var user = await _userService.GetAsync(ObjectionPermitRequestDto.ResearcherId.Value);
                var litigationManagers = _userService.GetUsersByRolesAsync(ApplicationRolesConstants.DepartmentManager.Code, user.Data.BranchId, (int)Departments.Litigation).Result.Data.FirstOrDefault();

                var notificationDto = new NotificationDto()
                {
                    SendEmailMessage = true,
                    Text = string.Format("تم ورود طلب توجيه على الاعتراض للقضية رقم {0} ", ObjectionPermitRequestDto.CaseId),
                    URL = "/requests/view/" + ObjectionPermitRequestDto.Id + "/" + (int)RequestTypes.ObjectionPermitRequest,
                    UserIds = new List<Guid>() { litigationManagers.Id },
                    Type = NotificationTypes.Info,
                    SendSMSMessage = false,
                };

                await _notificationService.AddAsync(notificationDto);

                return new ReturnResult<ObjectionPermitRequestDetailsDto>(true, HttpStatuses.Status201Created, ObjectionPermitRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ObjectionPermitRequestDto);

                return new ReturnResult<ObjectionPermitRequestDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ObjectionPermitRequestDto>> ReplyObjectionPermitRequest(ReplyObjectionPermitRequestDto replyObjectionPermitRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ReplyObjectionPermitRequestValidator(), replyObjectionPermitRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ObjectionPermitRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                // save the reply note and date and request status
                var replyObjectionPermitRequest = await _ObjectionPermitRequestRepository
                     .ReplyObjectionPermitRequestAsync(replyObjectionPermitRequestDto);


                // add request transaction
                var transactionToAdd = new RequestTransactionDto { RequestId = replyObjectionPermitRequest.Id };
                if (replyObjectionPermitRequest.Request.RequestStatus == RequestStatuses.AcceptedFromLitigationManager)
                {
                    transactionToAdd.Description = "تم اعتمادالطلب";
                    transactionToAdd.TransactionType = RequestTransactionTypes.RequestAccepted;
                }
                if (replyObjectionPermitRequest.Request.RequestStatus == RequestStatuses.Rejected)
                {
                    transactionToAdd.Description = "تم رفض الطلب";
                    transactionToAdd.TransactionType = RequestTransactionTypes.RequestRejected;
                }
                await _requestService.AddTransactionAsync(transactionToAdd);

                // add notification to researcher  
                string notificationText = "";
                if (replyObjectionPermitRequestDto.RequestStatus == RequestStatuses.AcceptedFromLitigationManager)
                {
                    if(replyObjectionPermitRequest.SuggestedOpinon == SuggestedOpinon.ObjectionAction)
                        notificationText = string.Format("تمت الموافقة من مدير الترافع على طلب الاعتراض على الحكم الصادر للقضية رقم {0} ", replyObjectionPermitRequestDto.CaseId);
                    else
                        notificationText = string.Format("تمت الموافقة من مدير الترافع على طلب الاكتفاء بالحكم الصادر للقضية رقم {0} ", replyObjectionPermitRequestDto.CaseId);

                }
                else
                {
                    if (replyObjectionPermitRequest.SuggestedOpinon == SuggestedOpinon.ObjectionAction)
                        notificationText = string.Format("تم الرفض من مدير الترافع لطلب الاعتراض على الحكم الصادر للقضية رقم {0} ", replyObjectionPermitRequestDto.CaseId);
                    else 
                        notificationText = string.Format("تم الرفض من مدير الترافع لطلب الاكتفاء بالحكم الصادر للقضية رقم {0} ", replyObjectionPermitRequestDto.CaseId);

                }
                var notificationDto = new NotificationDto()
                {
                    SendEmailMessage = true,
                    Text = notificationText,
                    URL = "requests/view/" + replyObjectionPermitRequest.Id + "/" + (int)RequestTypes.ObjectionPermitRequest,
                    UserIds = new List<Guid> { replyObjectionPermitRequestDto.ResearcherId },
                    Type = NotificationTypes.Info,
                    SendSMSMessage = false,
                };
                await _notificationService.AddAsync(notificationDto);

                // Add transaction in case
                var caseTransactionDto = new CaseTransactionDto()
                {
                    CaseId = replyObjectionPermitRequestDto.CaseId,
                    Note = notificationText,
                    TransactionType = CaseTransactionTypes.ReplyObjectionPermitRequest
                };
                await _caseService.AddTransactionAsync(caseTransactionDto);

                return new ReturnResult<ObjectionPermitRequestDto>(true, HttpStatuses.Status200OK, replyObjectionPermitRequest);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, replyObjectionPermitRequestDto);

                return new ReturnResult<ObjectionPermitRequestDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<Task> EndExpiredObjections()
        {
            try
            {
                _logger.LogInformation($"{nameof(EndExpiredObjections)} job starting...");

                var _objectionRequests = await _ObjectionPermitRequestRepository.GetExpiredObjections();

                foreach (var _request in _objectionRequests)
                {
                    // Change objection request status
                    await _ObjectionPermitRequestRepository.UpdateExpiredObjectionRequest(_request);

                    // AddTransaction
                    var requestTransactionDto = new RequestTransactionDto()
                    {
                        RequestId = _request.Request.Id,
                        TransactionType = RequestTransactionTypes.Abandoned,
                        Description = $"تم انتهاء مهلة الاعتراض"
                    };

                    await _requestService.AddTransactionAsync(requestTransactionDto);

                    // Add Notifications
                    var userIds = new List<Guid>();
                    userIds.Add(_request.Request.CreatedBy);
                    var litigationManagers = (await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.DepartmentManager.Code, _request.Request.CreatedByUser.BranchId, (int)Departments.Litigation)).Data;
                    userIds.AddRange(litigationManagers.Select(u => u.Id));
                    var generalSupervisors = (await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.GeneralSupervisor.Code, _request.Request.CreatedByUser.BranchId, null)).Data;
                    userIds.AddRange(generalSupervisors.Select(u => u.Id));

                    await _notificationService.AddAsync(new NotificationDto
                    {
                        Text = string.Format("{0} انتهت مهلة الاعتراض وتم تجميد الطلب ", _request.Request.Id),
                        UserIds = userIds,
                        SendEmailMessage = false,
                        URL = "requests/view/" + _request.Id + "/" + _request.Request.RequestType
                    });
                }

                _logger.LogInformation($"{nameof(EndExpiredObjections)} job ended successfully.");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromException(ex);
            }
        }

    }
}
