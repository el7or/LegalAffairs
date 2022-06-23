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
    public class MinistryDepartmentService : IMinistryDepartmentService
    {
        private readonly IMinistryDepartmentRepository _ministryDepartmentRepository;
        private readonly ILogger<MinistryDepartmentService> _logger;

        public MinistryDepartmentService(IMinistryDepartmentRepository ministryDepartmentRepository, ILogger<MinistryDepartmentService> logger)
        {
            _ministryDepartmentRepository = ministryDepartmentRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<MinistryDepartmentListItemDto>>> GetAllAsync(MinistryDepartmentQueryObject queryObject)
        {
            try
            {
                var entities = await _ministryDepartmentRepository.GetAllAsync(queryObject);

                if (entities == null)
                {
                    return new ReturnResult<QueryResultDto<MinistryDepartmentListItemDto>>(false, HttpStatuses.Status404NotFound, new List<string> { "البيانات المطلوبة غير متوفرة" });
                }

                return new ReturnResult<QueryResultDto<MinistryDepartmentListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<MinistryDepartmentListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<MinistryDepartmentListItemDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _ministryDepartmentRepository.GetAsync(id);

                return new ReturnResult<MinistryDepartmentListItemDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<MinistryDepartmentListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<MinistryDepartmentDto>> AddAsync(MinistryDepartmentDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new MinistryDepartmentValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isMinistryDepartmentExists = await _ministryDepartmentRepository.IsNameExistsAsync(model);
                if (isMinistryDepartmentExists)
                {
                    errors.Add("الادارة موجودة مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<MinistryDepartmentDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _ministryDepartmentRepository.AddAsync(model);

                return new ReturnResult<MinistryDepartmentDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<MinistryDepartmentDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<MinistryDepartmentDto>> EditAsync(MinistryDepartmentDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new MinistryDepartmentValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isMinistryDepartmentExists = await _ministryDepartmentRepository.IsNameExistsAsync(model);
                if (isMinistryDepartmentExists)
                {
                    errors.Add("الادارة موجودة مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<MinistryDepartmentDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _ministryDepartmentRepository.EditAsync(model);

                return new ReturnResult<MinistryDepartmentDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<MinistryDepartmentDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id, Guid userId)
        {
            try
            {
                await _ministryDepartmentRepository.RemoveAsync(id, userId);

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

        public async Task<ReturnResult<int>> GetDepartmentIdAsync(string name)
        {
            try
            {
                var departmentId = await _ministryDepartmentRepository.GetDepartmentIdAsync(name);

                return new ReturnResult<int>(true, HttpStatuses.Status200OK, departmentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, name);

                return new ReturnResult<int>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> IsNameExistsAsync(MinistryDepartmentDto ministryDepartmentDto)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _ministryDepartmentRepository.IsNameExistsAsync(ministryDepartmentDto);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ministryDepartmentDto.Id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
