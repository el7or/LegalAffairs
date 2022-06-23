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
    public class ConsultationSupportingDocumentService : IConsultationSupportingDocumentRequestService
    {
        private readonly IConsultationSupportingDocumentRequestRepository _ConsultationSupportingDocumentRepository;
        private readonly IRequestService _requestService;
        //private readonly IConsultationSupportingDocumentHistoryRepository _ConsultationSupportingDocumentHistoryRepository;

        private readonly ILogger<ConsultationSupportingDocumentService> _logger;

        public ConsultationSupportingDocumentService(
            IConsultationSupportingDocumentRequestRepository ConsultationSupportingDocumentRepository,
            IRequestService requestService,
            //IConsultationSupportingDocumentHistoryRepository ConsultationSupportingDocumentHistoryRepository,
            ILogger<ConsultationSupportingDocumentService> logger)
        {
            _ConsultationSupportingDocumentRepository = ConsultationSupportingDocumentRepository;
            //_ConsultationSupportingDocumentHistoryRepository = ConsultationSupportingDocumentHistoryRepository;
            _logger = logger;
            _requestService = requestService;
        }

        public async Task<ReturnResult<ConsultationSupportingDocumentListItemDto>> GetAsync(int id)
        {
            try
            {
                var entity = await _ConsultationSupportingDocumentRepository.GetAsync(id);

                return new ReturnResult<ConsultationSupportingDocumentListItemDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<ConsultationSupportingDocumentListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
        //public async Task<ReturnResult<ConsultationSupportingDocumentHistoryListItemDto>> GetHistoryAsync(int id)
        //{
        //    try
        //    {
        //        //var entity = await _ConsultationSupportingDocumentHistoryRepository.GetAsync(id);

        //        return new ReturnResult<ConsultationSupportingDocumentHistoryListItemDto>(true, HttpStatuses.Status200OK, entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message, id);

        //        return new ReturnResult<ConsultationSupportingDocumentHistoryListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
        //    }
        //}

        public async Task<ReturnResult<ConsultationSupportingDocumentRequestDto>> AddAsync(ConsultationSupportingDocumentRequestDto ConsultationSupportingDocumentDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ConsultationSupportingDocumentValidator(), ConsultationSupportingDocumentDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ConsultationSupportingDocumentRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                // add consultation request
                var ConsultationSupportingDocument = await _ConsultationSupportingDocumentRepository.AddAsync(ConsultationSupportingDocumentDto);

                // add request transaction
                await _requestService.AddTransactionAsync(new RequestTransactionDto { RequestId = ConsultationSupportingDocument.RequestId, TransactionType = RequestTransactionTypes.Create, Description = "" });

                return new ReturnResult<ConsultationSupportingDocumentRequestDto>(true, HttpStatuses.Status201Created, ConsultationSupportingDocumentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ConsultationSupportingDocumentDto);

                return new ReturnResult<ConsultationSupportingDocumentRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ConsultationSupportingDocumentRequestDto>> ReplyConsultationSupportingDocument(ReplyConsultationSupportingDocumentDto replyConsultationSupportingDocumentDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ReplyConsultationSupportingDocumentValidator(), replyConsultationSupportingDocumentDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ConsultationSupportingDocumentRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                // save the reply note and date and consultation request status
                var replyConsultationSupportingDocument = await _ConsultationSupportingDocumentRepository
                     .ReplyConsultationSupportingDocumentAsync(replyConsultationSupportingDocumentDto);


                // add request transaction
                var transactionToAdd = new RequestTransactionDto { RequestId = replyConsultationSupportingDocument.RequestId };
                if (replyConsultationSupportingDocument.Request.RequestStatus == RequestStatuses.Returned)
                {
                    transactionToAdd.Description = "تم اعادة الطلب للصياغة";
                    transactionToAdd.TransactionType = RequestTransactionTypes.Returned;
                }
                if (replyConsultationSupportingDocument.Request.RequestStatus == RequestStatuses.Rejected)
                {
                    transactionToAdd.Description = "تم رفض الطلب";
                    transactionToAdd.TransactionType = RequestTransactionTypes.RequestRejected;
                }
                if (replyConsultationSupportingDocument.Request.RequestStatus == RequestStatuses.Accepted)
                {
                    transactionToAdd.Description = "تم قبول الطلب";
                    transactionToAdd.TransactionType = RequestTransactionTypes.RequestAccepted;
                }
                if (replyConsultationSupportingDocument.Request.RequestStatus == RequestStatuses.Approved)
                {
                    transactionToAdd.Description = "تم اعتماد الطلب";
                    transactionToAdd.TransactionType = RequestTransactionTypes.Approved;
                }
                await _requestService.AddTransactionAsync(transactionToAdd);

                return new ReturnResult<ConsultationSupportingDocumentRequestDto>(true, HttpStatuses.Status200OK, replyConsultationSupportingDocument);

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, replyConsultationSupportingDocumentDto);

                return new ReturnResult<ConsultationSupportingDocumentRequestDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<ConsultationSupportingDocumentRequestDto>> EditAsync(ConsultationSupportingDocumentRequestDto ConsultationSupportingDocumentDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ConsultationSupportingDocumentValidator(), ConsultationSupportingDocumentDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ConsultationSupportingDocumentRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                ConsultationSupportingDocumentDto.Request.ReceiverRoleId = ApplicationRolesConstants.GeneralSupervisor.Code;
                ConsultationSupportingDocumentDto.Request.SendingType = SendingTypes.Role;

                // add current consultation request to history
                //await _ConsultationSupportingDocumentHistoryRepository.AddAsync(ConsultationSupportingDocumentDto.RequestId);

                await _ConsultationSupportingDocumentRepository.EditAsync(ConsultationSupportingDocumentDto);

                // add request transaction
                await _requestService.AddTransactionAsync(new RequestTransactionDto { RequestId = ConsultationSupportingDocumentDto.RequestId, Description = "تم تعديل صياغة الطلب", TransactionType = RequestTransactionTypes.Modified });

                return new ReturnResult<ConsultationSupportingDocumentRequestDto>(true, HttpStatuses.Status200OK, ConsultationSupportingDocumentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ConsultationSupportingDocumentDto);

                return new ReturnResult<ConsultationSupportingDocumentRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
