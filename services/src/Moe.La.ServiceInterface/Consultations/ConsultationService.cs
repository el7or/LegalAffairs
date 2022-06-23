using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Dtos.Consultations;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Case;
using Moe.La.ServiceInterface.Validators.Consultation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class ConsultationService : IConsultationService
    {
        private readonly ILogger<ConsultationService> _logger;
        private readonly IConsultationRepository _consultationRepository;
        private readonly IConsultationTransactionService _consultationTransactionService;
        public ConsultationService(IConsultationRepository consultationRepository, IConsultationTransactionService consultationTransactionService, ILogger<ConsultationService> logger)
        {
            _consultationRepository = consultationRepository;
            _consultationTransactionService = consultationTransactionService;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<ConsultationListItemDto>>> GetAllAsync(ConsultationQueryObject queryObject)
        {
            try
            {
                var entities = await _consultationRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<ConsultationListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<ConsultationListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<ConsultationDetailsDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _consultationRepository.GetAsync(id);

                return new ReturnResult<ConsultationDetailsDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<ConsultationDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<ConsultationDto>> AddAsync(ConsultationDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ConsultationValidator(), model);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ConsultationDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = validationResult.Errors,
                        Data = null
                    };
                }

                await _consultationRepository.AddAsync(model);

                // add transaction
                var consultationTransactionDto = new ConsultationTransactionDto
                {
                    ConsultationId = (int)model.Id,
                    ConsultationStatus = model.Status,
                    TransactionType = ConsultationTransactionTypes.Created,
                    Note = "-"
                };

                await _consultationTransactionService.AddAsync(consultationTransactionDto);
                ///

                return new ReturnResult<ConsultationDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<ConsultationDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<ConsultationDto>> EditAsync(ConsultationDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ConsultationValidator(), model);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ConsultationDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = validationResult.Errors,
                        Data = null
                    };
                }

                await _consultationRepository.EditAsync(model);

                if (model.Status == ConsultationStatus.Modified)
                {
                    // add transaction
                    var consultationTransactionDto = new ConsultationTransactionDto
                    {
                        ConsultationId = (int)model.Id,
                        ConsultationStatus = model.Status,
                        TransactionType = ConsultationTransactionTypes.Modified,
                        Note = "تم تعديل صياغة النموذج"
                    };

                    await _consultationTransactionService.AddAsync(consultationTransactionDto);
                }

                return new ReturnResult<ConsultationDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<ConsultationDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<ConsultationReviewDto>> ConsultationReview(ConsultationReviewDto consultationReviewDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ConsultationReviewValidator(), consultationReviewDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ConsultationReviewDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                var _replyConsultationReview = await _consultationRepository.ConsultationReviewAsync(consultationReviewDto);

                // Add transaction  
                var transactionToAdd = new ConsultationTransactionDto { ConsultationId = consultationReviewDto.Id };
                if (consultationReviewDto.ConsultationStatus == ConsultationStatus.Returned)
                {
                    transactionToAdd.Note = consultationReviewDto.DepartmentVision;
                    transactionToAdd.TransactionType = ConsultationTransactionTypes.Returned;
                }
                if (consultationReviewDto.ConsultationStatus == ConsultationStatus.Accepted)
                {
                    transactionToAdd.Note = "-";
                    transactionToAdd.TransactionType = ConsultationTransactionTypes.Accepted;
                }
                if (consultationReviewDto.ConsultationStatus == ConsultationStatus.Approved)
                {
                    transactionToAdd.Note = "-";
                    transactionToAdd.TransactionType = ConsultationTransactionTypes.Approved;
                }
                await _consultationTransactionService.AddAsync(transactionToAdd);

                return new ReturnResult<ConsultationReviewDto>(true, HttpStatuses.Status200OK, consultationReviewDto);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, consultationReviewDto);

                return new ReturnResult<ConsultationReviewDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<bool>> DeleteVisualAsync(int id)
        {
            try
            {

                await _consultationRepository.DeleteVisualAsync(id);

                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

    }
}
