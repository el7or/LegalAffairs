using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Dtos.Consultations;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Others;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class ConsultationTransactionService : IConsultationTransactionService
    {
        private readonly IConsultationTransactionRepository _transactionRepository;
        private readonly ILogger<ConsultationTransactionService> _logger;

        public ConsultationTransactionService(IConsultationTransactionRepository transactionRepository, ILogger<ConsultationTransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<ConsultationTransactionDto>> AddAsync(ConsultationTransactionDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ConsultationTransactionValidator(), model);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ConsultationTransactionDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = validationResult.Errors,
                        Data = null
                    };
                }

                await _transactionRepository.AddAsync(model);

                return new ReturnResult<ConsultationTransactionDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);
                return new ReturnResult<ConsultationTransactionDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }

        }

        public async Task<ReturnResult<QueryResultDto<ConsultationTransactionListDto>>> GetAllAsync(ConsultationTransactionQueryObject queryObject)
        {
            try
            {
                var entities = await _transactionRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<ConsultationTransactionListDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<ConsultationTransactionListDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }
    }
}
