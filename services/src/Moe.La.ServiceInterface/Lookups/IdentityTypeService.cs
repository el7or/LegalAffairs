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
    public class IdentityTypeService : IIdentityTypeService
    {
        private readonly IIdentityTypeRepository _identityTypeRepository;
        private readonly ILogger<IdentityTypeService> _logger;

        public IdentityTypeService(IIdentityTypeRepository identityTypeRepository, ILogger<IdentityTypeService> logger)
        {
            _identityTypeRepository = identityTypeRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<IdentityTypeListItemDto>>> GetAllAsync(QueryObject queryObject)
        {
            try
            {
                var entities = await _identityTypeRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<IdentityTypeListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<IdentityTypeListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<IdentityTypeListItemDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _identityTypeRepository.GetAsync(id);

                return new ReturnResult<IdentityTypeListItemDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<IdentityTypeListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<IdentityTypeDto>> AddAsync(IdentityTypeDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new IdentityTypeValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isNameExists = await _identityTypeRepository.IsNameExistsAsync(model.Name);
                if (isNameExists)
                {
                    errors.Add("اسم الهوية موجود مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<IdentityTypeDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _identityTypeRepository.AddAsync(model);

                return new ReturnResult<IdentityTypeDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<IdentityTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<IdentityTypeDto>> EditAsync(IdentityTypeDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new IdentityTypeValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }
                var dbIdentityType = await _identityTypeRepository.GetAsync(model.Id);
                bool isNameExists = await _identityTypeRepository.IsNameExistsAsync(model.Name);
                if (isNameExists && model.Name != dbIdentityType.Name)
                {
                    errors.Add("اسم الهوية موجود مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<IdentityTypeDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _identityTypeRepository.EditAsync(model);

                return new ReturnResult<IdentityTypeDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<IdentityTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _identityTypeRepository.RemoveAsync(id);

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

        public async Task<ReturnResult<bool>> IsNameExistsAsync(string name)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _identityTypeRepository.IsNameExistsAsync(name);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, name);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
