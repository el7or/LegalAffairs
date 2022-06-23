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
    public class MainCategoryService : IMainCategoryService
    {
        private readonly IMainCategoryRepository _mainCategoryRepository;
        private readonly ILogger<MainCategoryService> _logger;

        public MainCategoryService(IMainCategoryRepository mainCategoryRepository, ILogger<MainCategoryService> logger)
        {
            _mainCategoryRepository = mainCategoryRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<MainCategoryListItemDto>>> GetAllAsync(MainCategoryQueryObject queryObject)
        {
            try
            {
                var entities = await _mainCategoryRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<MainCategoryListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<QueryResultDto<MainCategoryListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<MainCategoryListItemDto>> GetAsync(int id)
        {
            try
            {
                var entity = await _mainCategoryRepository.GetAsync(id);

                return new ReturnResult<MainCategoryListItemDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<MainCategoryListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<MainCategoryDto>> AddAsync(MainCategoryDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new MainCategoryValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                //bool isMainCategoryExists = await _mainCategoryRepository.IsNameExistsAsync(model);
                //if (isMainCategoryExists)
                //{
                //    errors.Add("التصنيف الرئيسى موجودة مسبقاً.");
                //}

                if (errors.Any())
                {
                    return new ReturnResult<MainCategoryDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _mainCategoryRepository.AddAsync(model);

                return new ReturnResult<MainCategoryDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<MainCategoryDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<MainCategoryDto>> EditAsync(MainCategoryDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new MainCategoryValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                //bool isMainCategoryExists = await _mainCategoryRepository.IsNameExistsAsync(model);
                //if (isMainCategoryExists)
                //{
                //    errors.Add("التصنيف الرئيسى موجودة مسبقاً.");
                //}

                if (errors.Any())
                {
                    return new ReturnResult<MainCategoryDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _mainCategoryRepository.EditAsync(model);

                return new ReturnResult<MainCategoryDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<MainCategoryDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _mainCategoryRepository.RemoveAsync(id);

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

        public async Task<ReturnResult<bool>> IsNameExistsAsync(MainCategoryDto MainCategoryDto)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _mainCategoryRepository.IsNameExistsAsync(MainCategoryDto);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, MainCategoryDto.Id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
