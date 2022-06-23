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
    public class FirstSubCategoryService : IFirstSubCategoryService
    {
        private readonly IFirstSubCategoryRepository _firstSubCategoryRepository;
        private readonly ILogger<FirstSubCategoryService> _logger;

        public FirstSubCategoryService(IFirstSubCategoryRepository firstSubCategoryRepository, ILogger<FirstSubCategoryService> logger)
        {
            _firstSubCategoryRepository = firstSubCategoryRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<FirstSubCategoryListItemDto>>> GetAllAsync(FirstSubCategoriesQueryObject queryObject)
        {
            try
            {
                var entities = await _firstSubCategoryRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<FirstSubCategoryListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<QueryResultDto<FirstSubCategoryListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<FirstSubCategoryListItemDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _firstSubCategoryRepository.GetAsync(id);

                return new ReturnResult<FirstSubCategoryListItemDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<FirstSubCategoryListItemDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<FirstSubCategoryListItemDto>> GetByNameAsync(string name)
        {
            try
            {
                var entitiy = await _firstSubCategoryRepository.GetByNameAsync(name);

                return new ReturnResult<FirstSubCategoryListItemDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<FirstSubCategoryListItemDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<FirstSubCategoryDto>> AddAsync(FirstSubCategoryDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new FirstSubCategoryValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                //bool isFirstSubCategoryExists = await _firstSubCategoryRepository.IsNameExistsAsync(model);
                //if (isFirstSubCategoryExists)
                //{
                //    errors.Add("التصنيف الفرعى 1 موجودة");
                //}

                if (errors.Any())
                {
                    return new ReturnResult<FirstSubCategoryDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors,
                        Data = null
                    };
                }

                await _firstSubCategoryRepository.AddAsync(model);

                return new ReturnResult<FirstSubCategoryDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<FirstSubCategoryDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<FirstSubCategoryDto>> EditAsync(FirstSubCategoryDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new FirstSubCategoryValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                //bool isFirstSubCategoryExists = await _firstSubCategoryRepository.IsNameExistsAsync(model);
                //if (isFirstSubCategoryExists)
                //{
                //    errors.Add("التصنيف الفرعى 1 موجودة");
                //}

                if (errors.Any())
                {
                    return new ReturnResult<FirstSubCategoryDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors,
                        Data = null
                    };
                }

                await _firstSubCategoryRepository.EditAsync(model);

                return new ReturnResult<FirstSubCategoryDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<FirstSubCategoryDto>
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

                await _firstSubCategoryRepository.RemoveAsync(id);

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

        public async Task<ReturnResult<bool>> IsNameExistsAsync(FirstSubCategoryDto FirstSubCategoryDto)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _firstSubCategoryRepository.IsNameExistsAsync(FirstSubCategoryDto);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, FirstSubCategoryDto.Id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

    }
}
