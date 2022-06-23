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
    public class ProvinceService : IProvinceService
    {
        private readonly IProvinceRepository _provinceRepository;
        private readonly ILogger<ProvinceService> _logger;

        public ProvinceService(IProvinceRepository provinceRepository, ILogger<ProvinceService> logger)
        {
            _provinceRepository = provinceRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<ProvinceListItemDto>>> GetAllAsync(ProvinceQueryObject queryObject)
        {
            try
            {
                var entities = await _provinceRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<ProvinceListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<QueryResultDto<ProvinceListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ProvinceListItemDto>> GetAsync(int id)
        {
            try
            {
                var entity = await _provinceRepository.GetAsync(id);

                return new ReturnResult<ProvinceListItemDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<ProvinceListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ProvinceDto>> AddAsync(ProvinceDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new ProvinceValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isProvinceExists = await _provinceRepository.IsNameExistsAsync(model.Name);
                if (isProvinceExists)
                {
                    errors.Add("المنطقة موجودة مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<ProvinceDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _provinceRepository.AddAsync(model);

                return new ReturnResult<ProvinceDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<ProvinceDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ProvinceDto>> EditAsync(ProvinceDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new ProvinceValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isProvinceExists = await _provinceRepository.IsNameExistsAsync(model.Name);
                if (isProvinceExists)
                {
                    errors.Add("المنطقة موجودة مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<ProvinceDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _provinceRepository.EditAsync(model);

                return new ReturnResult<ProvinceDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<ProvinceDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _provinceRepository.RemoveAsync(id);

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

                bool result = await _provinceRepository.IsNameExistsAsync(name);

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
