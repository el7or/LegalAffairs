using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class SubWorkItemTypeService : ISubWorkItemTypeService
    {
        private readonly ISubWorkItemTypeRepository _subWorkItemTypeRepository;
        private readonly ILogger<SubWorkItemTypeService> _logger;

        public SubWorkItemTypeService(ISubWorkItemTypeRepository subWorkItemTypeRepository, ILogger<SubWorkItemTypeService> logger)
        {
            _subWorkItemTypeRepository = subWorkItemTypeRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<SubWorkItemTypeListItemDto>>> GetAllAsync(SubWorkItemTypeQueryObject queryObject)
        {
            try
            {
                var entities = await _subWorkItemTypeRepository.GetAllAsync(queryObject);

                if (entities == null)
                {
                    return new ReturnResult<QueryResultDto<SubWorkItemTypeListItemDto>>(false, HttpStatuses.Status404NotFound, new List<string> { "البيانات المطلوبة غير متوفرة" });
                }

                return new ReturnResult<QueryResultDto<SubWorkItemTypeListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<SubWorkItemTypeListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<SubWorkItemTypeDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _subWorkItemTypeRepository.GetAsync(id);

                return new ReturnResult<SubWorkItemTypeDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<SubWorkItemTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<SubWorkItemTypeDto>> AddAsync(SubWorkItemTypeDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new SubWorkItemTypeValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isNameExists = await _subWorkItemTypeRepository.IsNameExistsAsync(model);
                if (isNameExists)
                {
                    errors.Add("نوع العمل الفرعي موجود مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<SubWorkItemTypeDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _subWorkItemTypeRepository.AddAsync(model);

                return new ReturnResult<SubWorkItemTypeDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<SubWorkItemTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<SubWorkItemTypeDto>> EditAsync(SubWorkItemTypeDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new SubWorkItemTypeValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isNameExists = await _subWorkItemTypeRepository.IsNameExistsAsync(model);
                if (isNameExists)
                {
                    errors.Add("نوع العمل الفرعي موجود مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<SubWorkItemTypeDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _subWorkItemTypeRepository.EditAsync(model);

                return new ReturnResult<SubWorkItemTypeDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<SubWorkItemTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _subWorkItemTypeRepository.RemoveAsync(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
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

        public async Task<ReturnResult<bool>> IsNameExistsAsync(SubWorkItemTypeDto subWorkItemTypeDto)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _subWorkItemTypeRepository.IsNameExistsAsync(subWorkItemTypeDto);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, subWorkItemTypeDto.Id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
