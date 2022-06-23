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
    public class WorkItemTypeService : IWorkItemTypeService
    {
        private readonly IWorkItemTypeRepository _workItemTypeRepository;
        private readonly ILogger<WorkItemTypeService> _logger;

        public WorkItemTypeService(IWorkItemTypeRepository workItemTypeRepository, ILogger<WorkItemTypeService> logger)
        {
            _workItemTypeRepository = workItemTypeRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<WorkItemTypeListItemDto>>> GetAllAsync(WorkItemTypeQueryObject queryObject)
        {
            try
            {
                var entities = await _workItemTypeRepository.GetAllAsync(queryObject);

                if (entities == null)
                {
                    return new ReturnResult<QueryResultDto<WorkItemTypeListItemDto>>(false, HttpStatuses.Status404NotFound, new List<string> { "البيانات المطلوبة غير متوفرة" });
                }

                return new ReturnResult<QueryResultDto<WorkItemTypeListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<WorkItemTypeListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<WorkItemTypeDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _workItemTypeRepository.GetAsync(id);

                return new ReturnResult<WorkItemTypeDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<WorkItemTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<int>> GetByNameAsync(string name)
        {
            try
            {
                var entitiy = await _workItemTypeRepository.GetByNameAsync(name);

                return new ReturnResult<int>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, name);

                return new ReturnResult<int>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<WorkItemTypeDto>> EditAsync(WorkItemTypeDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new WorkItemTypeValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isNameExists = await _workItemTypeRepository.IsNameExistsAsync(model);
                if (isNameExists)
                {
                    errors.Add("نوع العمل موجود مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<WorkItemTypeDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _workItemTypeRepository.EditAsync(model);

                return new ReturnResult<WorkItemTypeDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<WorkItemTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        //public async Task<ReturnResult<WorkItemTypeDto>> AddAsync(WorkItemTypeDto model)
        //{
        //    try
        //    {
        //        await _workItemTypeRepository.AddAsync(model);

        //        return new ReturnResult<WorkItemTypeDto>(true, HttpStatuses.Status200OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message, model);

        //        return new ReturnResult<WorkItemTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
        //    }
        //}

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _workItemTypeRepository.RemoveAsync(id);

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

        public async Task<ReturnResult<bool>> IsNameExistsAsync(WorkItemTypeDto workItemTypeDto)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _workItemTypeRepository.IsNameExistsAsync(workItemTypeDto);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workItemTypeDto.Id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
