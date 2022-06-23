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
    public class GovernmentOrganizationService : IGovernmentOrganizationService
    {
        private readonly IGovernmentOrganizationRepository _GovernmentOrganizationRepository;
        private readonly ILogger<GovernmentOrganizationService> _logger;

        public GovernmentOrganizationService(IGovernmentOrganizationRepository GovernmentOrganizationRepository, ILogger<GovernmentOrganizationService> logger)
        {
            _GovernmentOrganizationRepository = GovernmentOrganizationRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<GovernmentOrganizationListItemDto>>> GetAllAsync(QueryObject queryObject)
        {
            try
            {
                var entities = await _GovernmentOrganizationRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<GovernmentOrganizationListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<QueryResultDto<GovernmentOrganizationListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<GovernmentOrganizationListItemDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _GovernmentOrganizationRepository.GetAsync(id);

                return new ReturnResult<GovernmentOrganizationListItemDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<GovernmentOrganizationListItemDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<GovernmentOrganizationDto>> AddAsync(GovernmentOrganizationDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new GovernmentOrganizationValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isGovernmentOrganizationExists = await _GovernmentOrganizationRepository.IsNameExistsAsync(model.Name);
                if (isGovernmentOrganizationExists)
                {
                    errors.Add("اسم الجهة موجود مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<GovernmentOrganizationDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }

                await _GovernmentOrganizationRepository.AddAsync(model);

                return new ReturnResult<GovernmentOrganizationDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<GovernmentOrganizationDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<GovernmentOrganizationDto>> EditAsync(GovernmentOrganizationDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new GovernmentOrganizationValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isGovernmentOrganizationExists = await _GovernmentOrganizationRepository.IsNameExistsAsync(model.Name);
                if (isGovernmentOrganizationExists)
                {
                    errors.Add("اسم الجهة موجود مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<GovernmentOrganizationDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }

                await _GovernmentOrganizationRepository.EditAsync(model);

                return new ReturnResult<GovernmentOrganizationDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<GovernmentOrganizationDto>
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

                await _GovernmentOrganizationRepository.RemoveAsync(id);

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

        public async Task<ReturnResult<bool>> IsNameExistsAsync(string name)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _GovernmentOrganizationRepository.IsNameExistsAsync(name);

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
