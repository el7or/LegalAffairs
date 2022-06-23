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
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly ILogger<CityService> _logger;

        public CityService(ICityRepository cityRepository, ILogger<CityService> logger)
        {
            _cityRepository = cityRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<CityListItemDto>>> GetAllAsync(CityQueryObject queryObject)
        {
            try
            {
                var entities = await _cityRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<CityListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<QueryResultDto<CityListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<CityListItemDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _cityRepository.GetAsync(id);

                return new ReturnResult<CityListItemDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<CityListItemDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<CityListItemDto>> GetByNameAsync(string name)
        {
            try
            {
                var entitiy = await _cityRepository.GetByNameAsync(name);

                return new ReturnResult<CityListItemDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<CityListItemDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<CityDto>> AddAsync(CityDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new CityValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isCityExists = await _cityRepository.IsNameExistsAsync(model);
                if (isCityExists)
                {
                    errors.Add(" المدينة موجودة في نفس المنطقة");
                }

                if (errors.Any())
                {
                    return new ReturnResult<CityDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors,
                        Data = null
                    };
                }

                await _cityRepository.AddAsync(model);

                return new ReturnResult<CityDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<CityDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<CityDto>> EditAsync(CityDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new CityValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isCityExists = await _cityRepository.IsNameExistsAsync(model);
                if (isCityExists)
                {
                    errors.Add(" المدينة موجودة في نفس المنطقة");
                }

                if (errors.Any())
                {
                    return new ReturnResult<CityDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors,
                        Data = null
                    };
                }

                await _cityRepository.EditAsync(model);

                return new ReturnResult<CityDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<CityDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {

                await _cityRepository.RemoveAsync(id);

                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = true
                };
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

        public async Task<ReturnResult<bool>> IsNameExistsAsync(CityDto cityDto)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _cityRepository.IsNameExistsAsync(cityDto);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, cityDto.Id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

    }
}
