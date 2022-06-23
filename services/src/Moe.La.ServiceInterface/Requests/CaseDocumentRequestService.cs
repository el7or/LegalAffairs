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
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class CaseDocumentRequestService : ICaseSupportingDocumentRequestService
    {
        private readonly ICaseSupportingDocumentRequestRepository _CaseSupportingDocumentRequestRepository;
        private readonly IRequestService _requestService;
        private readonly ICaseSupportingDocumentRequestHistoryService _CaseSupportingDocumentRequestHistoryService;

        private readonly ILogger<CaseDocumentRequestService> _logger;

        public CaseDocumentRequestService(
            ICaseSupportingDocumentRequestRepository CaseSupportingDocumentRequestRepository,
            IRequestService requestService,
            ICaseSupportingDocumentRequestHistoryService CaseSupportingDocumentRequestHistoryService,
            ILogger<CaseDocumentRequestService> logger)
        {
            _CaseSupportingDocumentRequestRepository = CaseSupportingDocumentRequestRepository;
            _CaseSupportingDocumentRequestHistoryService = CaseSupportingDocumentRequestHistoryService;
            _logger = logger;
            _requestService = requestService;
        }

        public async Task<ReturnResult<CaseSupportingDocumentRequestListItemDto>> GetAsync(int id)
        {
            try
            {
                var entity = await _CaseSupportingDocumentRequestRepository.GetAsync(id);

                return new ReturnResult<CaseSupportingDocumentRequestListItemDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<CaseSupportingDocumentRequestListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<AttachedLetterRequestDto>> GetAttachedLetterRequestAsync(int id)
        {
            try
            {
                var entity = await _CaseSupportingDocumentRequestRepository.GetAttachedLetterRequestAsync(id);

                return new ReturnResult<AttachedLetterRequestDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<AttachedLetterRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
        public async Task<CaseSupportingDocumentRequestForPrintDto> GetForPrintAsync(int id)
        {
            try
            {
                var entity = await _CaseSupportingDocumentRequestRepository.GetForPrintAsync(id);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return null;
            }
        }

        public async Task<ReturnResult<CaseSupportingDocumentRequestDto>> AddAsync(CaseSupportingDocumentRequestDto documentRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new DocumentRequestValidator(), documentRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<CaseSupportingDocumentRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                //documentRequestDto.Request.ReceiverRoleId = ApplicationRolesConstants.GeneralSupervisor.Code;
                //documentRequestDto.Request.SendingType = SendingTypes.Role;

                // add supporting document request
                var docRequest = await _CaseSupportingDocumentRequestRepository.AddAsync(documentRequestDto);

                // add request transaction
                await _requestService.AddTransactionAsync(new RequestTransactionDto { RequestId = docRequest.Id, TransactionType = RequestTransactionTypes.Create, Description = "" });

                return new ReturnResult<CaseSupportingDocumentRequestDto>(true, HttpStatuses.Status201Created, documentRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, documentRequestDto);

                return new ReturnResult<CaseSupportingDocumentRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<AttachedLetterRequestDto>> AddAttachedLetterRequestAsync(AttachedLetterRequestDto attachedLetterRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new AttachedLetterRequestValidator(), attachedLetterRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<AttachedLetterRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                attachedLetterRequestDto.Request.ReceiverRoleId = ApplicationRolesConstants.GeneralSupervisor.Code;
                attachedLetterRequestDto.Request.SendingType = SendingTypes.Role;

                // add attached letter request
                var docRequest = await _CaseSupportingDocumentRequestRepository.AddAttachedLetterRequestAsync(attachedLetterRequestDto);

                // add request transaction
                await _requestService.AddTransactionAsync(new RequestTransactionDto { RequestId = (int)docRequest.ParentId, TransactionType = RequestTransactionTypes.AttachedLetter, Description = "تذكير بطلب مستندات داعمة" });

                return new ReturnResult<AttachedLetterRequestDto>(true, HttpStatuses.Status201Created, attachedLetterRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, attachedLetterRequestDto);

                return new ReturnResult<AttachedLetterRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }


        public async Task<ReturnResult<AttachedLetterRequestDto>> EditAttachedLetterRequestAsync(AttachedLetterRequestDto attachedLetterRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new AttachedLetterRequestValidator(), attachedLetterRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<AttachedLetterRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                attachedLetterRequestDto.Request.ReceiverRoleId = ApplicationRolesConstants.GeneralSupervisor.Code;
                attachedLetterRequestDto.Request.SendingType = SendingTypes.Role;

                //// add current attached letter request to history
                //await _CaseSupportingDocumentRequestHistoryService.AddAsync((int)attachedLetterRequestDto.Id);

                // edit attached letter request
                var docRequest = await _CaseSupportingDocumentRequestRepository.EditAttachedLetterRequestAsync(attachedLetterRequestDto);

                // edit request transaction
                await _requestService.AddTransactionAsync(new RequestTransactionDto { RequestId = (int)docRequest.ParentId, TransactionType = RequestTransactionTypes.AttachedLetter, Description = "إعادة صياغة خطاب إلحاقى لطلب مستندات داعمة" });

                return new ReturnResult<AttachedLetterRequestDto>(true, HttpStatuses.Status200OK, attachedLetterRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, attachedLetterRequestDto);

                return new ReturnResult<AttachedLetterRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<CaseSupportingDocumentRequestDto>> ReplyCaseSupportingDocumentRequest(ReplyCaseSupportingDocumentRequestDto replyDocumentRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ReplyDocumentRequestValidator(), replyDocumentRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<CaseSupportingDocumentRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                // save the reply note and date and document request status
                var replyDocumentRequest = await _CaseSupportingDocumentRequestRepository
                     .ReplyDocumentRequestAsync(replyDocumentRequestDto);


                // add request transaction
                var transactionToAdd = new RequestTransactionDto { RequestId = replyDocumentRequest.Id };
                if (replyDocumentRequest.Request.RequestStatus == RequestStatuses.Returned)
                {
                    transactionToAdd.Description = replyDocumentRequest.ReplyNote;
                    transactionToAdd.TransactionType = RequestTransactionTypes.Returned;
                }
                if (replyDocumentRequest.Request.RequestStatus == RequestStatuses.Rejected)
                {
                    transactionToAdd.Description = replyDocumentRequest.ReplyNote;
                    transactionToAdd.TransactionType = RequestTransactionTypes.RequestRejected;
                }
                if (replyDocumentRequest.Request.RequestStatus == RequestStatuses.Accepted)
                {
                    transactionToAdd.Description = "تم قبول الطلب";
                    transactionToAdd.TransactionType = RequestTransactionTypes.RequestAccepted;
                }
                if (replyDocumentRequest.Request.RequestStatus == RequestStatuses.AcceptedFromConsultant)
                {
                    transactionToAdd.Description = "تم قبول الطلب";
                    transactionToAdd.TransactionType = RequestTransactionTypes.RequestAccepted;
                }
                if (replyDocumentRequest.Request.RequestStatus == RequestStatuses.AcceptedFromLitigationManager)
                {
                    transactionToAdd.Description = "تم قبول الطلب من مدير الترافع";
                    transactionToAdd.TransactionType = RequestTransactionTypes.RequestAccepted;
                }
                if (replyDocumentRequest.Request.RequestStatus == RequestStatuses.Approved)
                {
                    transactionToAdd.Description = "تم اعتماد الطلب";
                    transactionToAdd.TransactionType = RequestTransactionTypes.Approved;
                }
                if (replyDocumentRequest.Request.RequestStatus == RequestStatuses.Exported)
                {
                    transactionToAdd.Description = "تم تصدير الطلب";
                    transactionToAdd.TransactionType = RequestTransactionTypes.Exported;
                }
                await _requestService.AddTransactionAsync(transactionToAdd);

                if (replyDocumentRequest.Request.RequestStatus == RequestStatuses.Returned)
                {
                    await _CaseSupportingDocumentRequestHistoryService.AddAsync(replyDocumentRequestDto.Id);
                }

                return new ReturnResult<CaseSupportingDocumentRequestDto>(true, HttpStatuses.Status200OK, replyDocumentRequest);

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, replyDocumentRequestDto);

                return new ReturnResult<CaseSupportingDocumentRequestDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<CaseSupportingDocumentRequestDto>> EditAsync(CaseSupportingDocumentRequestDto documentRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new DocumentRequestValidator(), documentRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<CaseSupportingDocumentRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                documentRequestDto.Request.ReceiverRoleId = ApplicationRolesConstants.GeneralSupervisor.Code;
                documentRequestDto.Request.SendingType = SendingTypes.Role;


                await _CaseSupportingDocumentRequestRepository.EditAsync(documentRequestDto);


                return new ReturnResult<CaseSupportingDocumentRequestDto>(true, HttpStatuses.Status200OK, documentRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, documentRequestDto);

                return new ReturnResult<CaseSupportingDocumentRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
