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
    public class SecondSubCategoryService : ISecondSubCategoryService
    {
        private readonly ISecondSubCategoryRepository _secondSubCategoryRepository;
        private readonly IFirstSubCategoryService _firstSubCategoryService;
        private readonly IMainCategoryService _mainCategoryService;
        private readonly ILegalMemoService _legalMemoService;
        private readonly ICaseService _caseService;



        private readonly ILogger<SecondSubCategoryService> _logger;

        public SecondSubCategoryService(ISecondSubCategoryRepository secondSubCategoryRepository,
                                        IFirstSubCategoryService firstSubCategoryService,
                                        IMainCategoryService mainCategoryService,
                                        ILegalMemoService legalMemoService,
                                        ICaseService caseService,
                                        ILogger<SecondSubCategoryService> logger)
        {
            _secondSubCategoryRepository = secondSubCategoryRepository;
            _firstSubCategoryService = firstSubCategoryService;
            _mainCategoryService = mainCategoryService;
            _legalMemoService = legalMemoService;
            _caseService = caseService;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<SecondSubCategoryListItemDto>>> GetAllAsync(SecondSubCategoryQueryObject queryObject)
        {
            try
            {
                var entities = await _secondSubCategoryRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<SecondSubCategoryListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<QueryResultDto<SecondSubCategoryListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<SecondSubCategoryDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _secondSubCategoryRepository.GetAsync(id);

                return new ReturnResult<SecondSubCategoryDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<SecondSubCategoryDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<SecondSubCategoryDto>> AddAsync(SecondSubCategoryDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new SecondSubCategoryValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isSecondSubCategoryExists = await _secondSubCategoryRepository.IsNameExistsAsync(model);
                if (isSecondSubCategoryExists)
                {
                    errors.Add("التصنيف الفرعى 2 موجود.");
                }
                else
                {
                    var isMainCategoryExists = await _mainCategoryService.IsNameExistsAsync(new MainCategoryDto { Id = model.MainCategory.Id, Name = model.MainCategory.Name });
                    if (isMainCategoryExists.Data && model.MainCategory.Id == null)
                    {
                        errors.Add("التصنيف الرئيسى موجود.");
                    }

                    else
                    {
                        var isFirstSubCategoryExists = await _firstSubCategoryService.IsNameExistsAsync(new FirstSubCategoryDto { Id = model.FirstSubCategory.Id, Name = model.FirstSubCategory.Name, MainCategoryId = model.MainCategory.Id });
                        if (isFirstSubCategoryExists.Data && model.FirstSubCategory.Id == null)
                        {
                            errors.Add("التصنيف الفرعى 1 موجود.");
                        }
                        else
                        {
                            if (!isMainCategoryExists.Data)
                            {
                                var MainCategory = await _mainCategoryService.AddAsync(new MainCategoryDto { Id = model.MainCategory.Id, Name = model.MainCategory.Name, CaseSource = model.CaseSource.Value });
                                model.MainCategory = MainCategory.Data;

                                if (!isFirstSubCategoryExists.Data)
                                {
                                    var FirstSubCategory = await _firstSubCategoryService.AddAsync(new FirstSubCategoryDto { Id = model.FirstSubCategory.Id, Name = model.FirstSubCategory.Name, MainCategoryId = model.MainCategory.Id });
                                    model.FirstSubCategory = FirstSubCategory.Data;
                                }
                            }
                            else
                            {
                                if (!isFirstSubCategoryExists.Data)
                                {
                                    var FirstSubCategory = await _firstSubCategoryService.AddAsync(new FirstSubCategoryDto { Id = model.FirstSubCategory.Id, Name = model.FirstSubCategory.Name, MainCategoryId = model.MainCategory.Id });
                                    model.FirstSubCategory = FirstSubCategory.Data;
                                }
                            }

                        }
                    }
                }

                if (errors.Any())
                {
                    return new ReturnResult<SecondSubCategoryDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }

                await _secondSubCategoryRepository.AddAsync(model);

                return new ReturnResult<SecondSubCategoryDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<SecondSubCategoryDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<SecondSubCategoryDto>> EditAsync(SecondSubCategoryDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new SecondSubCategoryValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }
                var category = await _secondSubCategoryRepository.GetAsync(model.Id.Value);
                if (category != null)
                {
                    if (category.CaseSource == model.CaseSource && category.Name == model.Name && category.FirstSubCategory.Name == model.FirstSubCategory.Name && category.MainCategory.Name == model.MainCategory.Name)
                    {
                        errors.Add("لا يوجد تغيير فى البيانات");
                    }
                    else
                    {
                        bool isSecondSubCategoryExists = await _secondSubCategoryRepository.IsNameExistsAsync(model);
                        if (isSecondSubCategoryExists && category.Name != model.Name)
                        {
                            errors.Add("التصنيف الفرعى 2 موجود.");
                        }
                        else
                        {
                            var isFirstSubCategoryExists = await _firstSubCategoryService.IsNameExistsAsync(new FirstSubCategoryDto { Id = model.FirstSubCategory.Id, Name = model.FirstSubCategory.Name, MainCategoryId = model.MainCategory.Id });
                            if (isFirstSubCategoryExists.Data && category.FirstSubCategory.Name != model.FirstSubCategory.Name)
                            {
                                errors.Add("التصنيف الفرعى 1 موجود.");
                            }

                            else
                            {
                                var isMainCategoryExists = await _mainCategoryService.IsNameExistsAsync(new MainCategoryDto { Id = model.MainCategory.Id, Name = model.MainCategory.Name });
                                if (isMainCategoryExists.Data && category.MainCategory.Name != model.MainCategory.Name)
                                {
                                    errors.Add("التصنيف الرئيسى موجود.");
                                }
                                else
                                {
                                    if (!isMainCategoryExists.Data)
                                    {
                                        var MainCategory = await _mainCategoryService.AddAsync(new MainCategoryDto { Id = model.MainCategory.Id, Name = model.MainCategory.Name, CaseSource = model.CaseSource.Value });
                                        model.MainCategory = MainCategory.Data;
                                        if (category.FirstSubCategory.Name == model.FirstSubCategory.Name)
                                        {
                                            var FirstSubCategory = await _firstSubCategoryService.EditAsync(new FirstSubCategoryDto { Id = model.FirstSubCategory.Id, Name = model.FirstSubCategory.Name, MainCategoryId = model.MainCategory.Id });
                                            model.FirstSubCategory = FirstSubCategory.Data;
                                        }
                                        else
                                        {
                                            var FirstSubCategory = await _firstSubCategoryService.AddAsync(new FirstSubCategoryDto { Id = model.FirstSubCategory.Id, Name = model.FirstSubCategory.Name, MainCategoryId = model.MainCategory.Id });
                                            model.FirstSubCategory = FirstSubCategory.Data;
                                        }

                                    }
                                    if (!isFirstSubCategoryExists.Data)
                                    {
                                        var FirstSubCategory = await _firstSubCategoryService.AddAsync(new FirstSubCategoryDto { Id = model.FirstSubCategory.Id, Name = model.FirstSubCategory.Name, MainCategoryId = model.MainCategory.Id });
                                        model.FirstSubCategory = FirstSubCategory.Data;
                                    }

                                    if (category.CaseSource != model.CaseSource && isMainCategoryExists.Data)
                                    {
                                        var MainCategory = await _mainCategoryService.EditAsync(new MainCategoryDto { Id = category.MainCategory.Id, Name = model.MainCategory.Name, CaseSource = model.CaseSource.Value });
                                        model.MainCategory = MainCategory.Data;
                                        if (category.FirstSubCategory.Name == model.FirstSubCategory.Name)
                                        {
                                            var FirstSubCategory = await _firstSubCategoryService.EditAsync(new FirstSubCategoryDto { Id = model.FirstSubCategory.Id, Name = model.FirstSubCategory.Name, MainCategoryId = model.MainCategory.Id });
                                            model.FirstSubCategory = FirstSubCategory.Data;
                                        }
                                        else
                                        {
                                            var FirstSubCategory = await _firstSubCategoryService.AddAsync(new FirstSubCategoryDto { Id = model.FirstSubCategory.Id, Name = model.FirstSubCategory.Name, MainCategoryId = model.MainCategory.Id });
                                            model.FirstSubCategory = FirstSubCategory.Data;
                                        }

                                    }
                                    if (model.MainCategory.Id != null && category.CaseSource == model.CaseSource)
                                    {
                                        if (category.FirstSubCategory.Id != model.FirstSubCategory.Id && model.FirstSubCategory.MainCategoryId == null)
                                        {
                                            var FirstSubCategory = await _firstSubCategoryService.EditAsync(new FirstSubCategoryDto { Id = model.FirstSubCategory.Id, Name = model.FirstSubCategory.Name, MainCategoryId = model.MainCategory.Id });
                                            model.FirstSubCategory = FirstSubCategory.Data;
                                        }
                                        else
                                        {
                                            if (category.FirstSubCategory.Id == null)
                                            {
                                                var FirstSubCategory = await _firstSubCategoryService.AddAsync(new FirstSubCategoryDto { Id = model.FirstSubCategory.Id, Name = model.FirstSubCategory.Name, MainCategoryId = model.MainCategory.Id });
                                                model.FirstSubCategory = FirstSubCategory.Data;
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }

                }

                if (errors.Any())
                {
                    return new ReturnResult<SecondSubCategoryDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }

                await _secondSubCategoryRepository.EditAsync(model);

                return new ReturnResult<SecondSubCategoryDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<SecondSubCategoryDto>
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
                var errors = new List<string>();

                await _secondSubCategoryRepository.RemoveAsync(id);

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

        public async Task<ReturnResult<bool>> IsNameExistsAsync(SecondSubCategoryDto SecondSubCategoryDto)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _secondSubCategoryRepository.IsNameExistsAsync(SecondSubCategoryDto);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, SecondSubCategoryDto.Id.Value);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> ChangeCatergoryActivityAsync(int secondSubCategoryId, bool isActive)
        {
            try
            {
                await _secondSubCategoryRepository.ChangeCategoryActivity(secondSubCategoryId, isActive);
                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

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
