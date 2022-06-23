using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class InvestigationQuestionService : IInvestigationQuestionService
    {
        private readonly IInvestigationQuestionRepository _InvestigationQuestionRepository;
        private readonly ILogger<InvestigationQuestionService> _logger;

        public InvestigationQuestionService(IInvestigationQuestionRepository InvestigationQuestionRepository, ILogger<InvestigationQuestionService> logger)
        {
            _InvestigationQuestionRepository = InvestigationQuestionRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<InvestigationQuestionListItemDto>>> GetAllAsync(InvestigationQuestionQueryObject queryObject)
        {
            try
            {
                var entities = await _InvestigationQuestionRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<InvestigationQuestionListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<InvestigationQuestionListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<InvestigationQuestionListItemDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _InvestigationQuestionRepository.GetAsync(id);

                return new ReturnResult<InvestigationQuestionListItemDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<InvestigationQuestionListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<InvestigationQuestionDto>> AddAsync(InvestigationQuestionDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new InvestigationQuestionValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isQuestionExists = await _InvestigationQuestionRepository.IsNameExistsAsync(model.Question);
                if (isQuestionExists)
                {
                    errors.Add("السؤال موجود مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<InvestigationQuestionDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _InvestigationQuestionRepository.AddAsync(model);

                return new ReturnResult<InvestigationQuestionDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<InvestigationQuestionDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<InvestigationQuestionDto>> EditAsync(InvestigationQuestionDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new InvestigationQuestionValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                if (errors.Any())
                {
                    return new ReturnResult<InvestigationQuestionDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _InvestigationQuestionRepository.EditAsync(model);

                return new ReturnResult<InvestigationQuestionDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<InvestigationQuestionDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _InvestigationQuestionRepository.RemoveAsync(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status201Created, true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "لا يمكن حذف العنصر لارتباطه بعناصر اخرى." }
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

        public async Task<ReturnResult<InvestigationQuestionDto>> ChangeQuestionStatusAsync(InvestigationQuestionChangeStatusDto model)
        {
            try
            {
                var entitiy = await _InvestigationQuestionRepository.ChangeQuestionStatusAsync(model);

                return new ReturnResult<InvestigationQuestionDto>(true, HttpStatuses.Status201Created, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<InvestigationQuestionDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> IsNameExistAsync(string question)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _InvestigationQuestionRepository.IsNameExistsAsync(question);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, question);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

    }
}
