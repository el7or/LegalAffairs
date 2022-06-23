using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class LetterTemplateService : ILetterTemplateService
    {
        private readonly ILetterTemplateRepository _letterTemplateRepository;
        private readonly ILogger<LetterTemplateService> _logger;

        public LetterTemplateService(ILetterTemplateRepository letterTemplateRepository, ILogger<LetterTemplateService> logger)
        {
            _letterTemplateRepository = letterTemplateRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<LetterTemplateDto>>> GetAllAsync(TemplateQueryObject queryObject)
        {
            try
            {
                var entities = await _letterTemplateRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<LetterTemplateDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<LetterTemplateDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<LetterTemplateDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _letterTemplateRepository.GetAsync(id);

                return new ReturnResult<LetterTemplateDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<LetterTemplateDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<LetterTemplateDto>> AddAsync(LetterTemplateDto model)
        {
            try
            {
                await _letterTemplateRepository.AddAsync(model);

                return new ReturnResult<LetterTemplateDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<LetterTemplateDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<LetterTemplateDto>> EditAsync(LetterTemplateDto model)
        {
            try
            {
                await _letterTemplateRepository.EditAsync(model);

                return new ReturnResult<LetterTemplateDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<LetterTemplateDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _letterTemplateRepository.RemoveAsync(id);

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
    }
}
