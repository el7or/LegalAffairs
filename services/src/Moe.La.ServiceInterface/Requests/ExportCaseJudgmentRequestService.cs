using Microsoft.Extensions.Logging;
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
    public class ExportCaseJudgmentRequestService : IExportCaseJudgmentRequestService
    {
        private readonly IExportCaseJudgmentRequestRepository _exportCaseJudgmentRequestRepository;
        private readonly IRequestService _requestService;
        private readonly IExportCaseJudgmentRequestHistoryService _exportCaseJudgmentRequestHistoryService;
        private readonly ILogger<ExportCaseJudgmentRequestService> _logger;

        public ExportCaseJudgmentRequestService(
            IExportCaseJudgmentRequestRepository exportCaseJudgmentRequestRepository,
            IRequestService requestService,
            IExportCaseJudgmentRequestHistoryService exportCaseJudgmentRequestHistoryService,
            ILogger<ExportCaseJudgmentRequestService> logger)
        {
            _exportCaseJudgmentRequestRepository = exportCaseJudgmentRequestRepository;
            _exportCaseJudgmentRequestHistoryService = exportCaseJudgmentRequestHistoryService;
            _logger = logger;
            _requestService = requestService;
        }

        public async Task<ReturnResult<ExportCaseJudgmentRequestListItemDto>> GetAsync(int id)
        {
            try
            {
                var entity = await _exportCaseJudgmentRequestRepository.GetAsync(id);

                return new ReturnResult<ExportCaseJudgmentRequestListItemDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<ExportCaseJudgmentRequestListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ExportCaseJudgmentRequestDetailsDto>> AddAsync(ExportCaseJudgmentRequestDto exportCaseJudgmentRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ExportCaseJudgmentRequestValidator(), exportCaseJudgmentRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ExportCaseJudgmentRequestDetailsDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                var exportCaseJudgmentRequest = await _exportCaseJudgmentRequestRepository.AddAsync(exportCaseJudgmentRequestDto);

                // Add Transaction
                var requestTransactionDTO = new RequestTransactionDto()
                {
                    RequestId = exportCaseJudgmentRequest.Id,
                    RequestStatus = RequestStatuses.New,
                    TransactionType = RequestTransactionTypes.Create,
                    Description = ""
                };

                await _requestService.AddTransactionAsync(requestTransactionDTO);

                return new ReturnResult<ExportCaseJudgmentRequestDetailsDto>(true, HttpStatuses.Status201Created, exportCaseJudgmentRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, exportCaseJudgmentRequestDto);

                return new ReturnResult<ExportCaseJudgmentRequestDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ExportCaseJudgmentRequestDto>> EditAsync(ExportCaseJudgmentRequestDto exportCaseJudgmentRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ExportCaseJudgmentRequestValidator(), exportCaseJudgmentRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ExportCaseJudgmentRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }


                await _exportCaseJudgmentRequestRepository.EditAsync(exportCaseJudgmentRequestDto);

                if (exportCaseJudgmentRequestDto.Request.RequestStatus == RequestStatuses.Modified)
                    // add request transaction
                    await _requestService.AddTransactionAsync(new RequestTransactionDto { RequestId = exportCaseJudgmentRequestDto.Id, Description = "تم تعديل صياغة الطلب", TransactionType = RequestTransactionTypes.Modified });


                return new ReturnResult<ExportCaseJudgmentRequestDto>(true, HttpStatuses.Status200OK, exportCaseJudgmentRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, exportCaseJudgmentRequestDto);

                return new ReturnResult<ExportCaseJudgmentRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ExportCaseJudgmentRequestDto>> ReplyExportCaseJudgmentRequest(ReplyExportCaseJudgmentRequestDto replyExportCaseJudgmentRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ReplyExportCaseJudgmentRequestValidator(), replyExportCaseJudgmentRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ExportCaseJudgmentRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                // save the reply note and date and case close request status
                var replyExportCaseJudgmentRequest = await _exportCaseJudgmentRequestRepository
                     .ReplyExportCaseJudgmentRequestAsync(replyExportCaseJudgmentRequestDto);


                // add request transaction
                var transactionToAdd = new RequestTransactionDto { RequestId = replyExportCaseJudgmentRequest.Id, RequestStatus = replyExportCaseJudgmentRequestDto.RequestStatus };
                if (replyExportCaseJudgmentRequest.Request.RequestStatus == RequestStatuses.Returned)
                {
                    transactionToAdd.Description = replyExportCaseJudgmentRequestDto.ReplyNote;
                    transactionToAdd.TransactionType = RequestTransactionTypes.Returned;
                }
                if (replyExportCaseJudgmentRequest.Request.RequestStatus == RequestStatuses.Approved)
                {
                    transactionToAdd.Description = "تم اعتمادالطلب";
                    transactionToAdd.TransactionType = RequestTransactionTypes.RequestAccepted;
                }
                if (replyExportCaseJudgmentRequest.Request.RequestStatus == RequestStatuses.Exported)
                {
                    transactionToAdd.Description = "تم تصدير الحكم";
                    transactionToAdd.TransactionType = RequestTransactionTypes.Exported;
                }

                if (replyExportCaseJudgmentRequest.Request.RequestStatus != RequestStatuses.New)
                    await _requestService.AddTransactionAsync(transactionToAdd);

                if (replyExportCaseJudgmentRequest.Request.RequestStatus == RequestStatuses.Returned)
                {
                    await _exportCaseJudgmentRequestHistoryService.AddAsync(replyExportCaseJudgmentRequest.Id);

                }
                return new ReturnResult<ExportCaseJudgmentRequestDto>(true, HttpStatuses.Status200OK, replyExportCaseJudgmentRequest);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, replyExportCaseJudgmentRequestDto);

                return new ReturnResult<ExportCaseJudgmentRequestDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }
        public async Task<ExportCaseJudgmentRequestForPrintDto> GetForPrintAsync(int id)
        {
            try
            {
                var entity = await _exportCaseJudgmentRequestRepository.GetForPrintAsync(id);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return null;
            }
        }

    }
}
