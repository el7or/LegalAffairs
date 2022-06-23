using Microsoft.Extensions.Logging;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class ResearchsConsultantService : IResearchsConsultantService
    {
        private readonly IResearcherConsultantRepository _researcherConsultantRepository;
        private readonly IUserService _userService;
        private readonly ILogger<ResearchsConsultantService> _logger;

        public ResearchsConsultantService(IResearcherConsultantRepository researcherConsultantRepository,
            IUserService userService, ILogger<ResearchsConsultantService> logger)
        {
            _researcherConsultantRepository = researcherConsultantRepository;
            _userService = userService;
            _logger = logger;
        }

        public async Task<ReturnResult<ResearcherConsultantDto>> AddAsync(ResearcherConsultantDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new ResearcherConsultantValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                var isResearcher = await _userService.IsInRole(model.ResearcherId, ApplicationRolesConstants.LegalResearcher.Code);

                if (!isResearcher.Data)
                {
                    errors.Add("الباحث المحدد لا ينتمي إلى مجموعة الباحثين.");
                }

                var isResearcherEnabled = await _userService.CheckUserEnabled((Guid)model.ResearcherId);

                if (!isResearcherEnabled.Data)
                {
                    errors.Add("الباحث المحدد غير مفعل");
                }

                if (model.ConsultantId != null)
                {
                    var isConsultant = await _userService.IsInRole(model.ConsultantId.Value, ApplicationRolesConstants.LegalConsultant.Code);

                    if (!isConsultant.Data)
                    {
                        errors.Add("المستشار المحدد لا ينتمي إلى مجموعة المستشارين.");
                    }

                    var isConsultantEnabled = await _userService.CheckUserEnabled((Guid)model.ConsultantId);

                    if (!isConsultantEnabled.Data)
                    {
                        errors.Add("المستشار المحدد غير مفعل.");

                    }
                    var isSameLegalDepatrment = await _userService.CheckSameLegalDepartment(model.ResearcherId, (Guid)model.ConsultantId);

                    if (!isSameLegalDepatrment.Data)
                    {
                        errors.Add("الباحث والمستشار غير مرتبطين بنفس الادارة القانونية.");
                    }

                    // check if researcher has already an active relation with same consultant
                    var activeRelationExistsWithConsultant = await _researcherConsultantRepository.GetAllAsync(new ResearcherQueryObject { ResearcherId = model.ResearcherId, ConsultantId = model.ConsultantId });

                    if (activeRelationExistsWithConsultant.Items.Count() > 0)
                    {
                        errors.Add("يوجد علاقة حالية لنفس الباحث مع نفس المستشار.");
                    }
                }

                if (errors.Any())
                {
                    return new ReturnResult<ResearcherConsultantDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }
                await _researcherConsultantRepository.AddToHistoryAsync(model);

                //add relation in  researcher consultant only if consultantId != null
                if (model.ConsultantId != null)
                    await _researcherConsultantRepository.AddAsync(model);

                return new ReturnResult<ResearcherConsultantDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<ResearcherConsultantDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<ResearcherConsultantDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _researcherConsultantRepository.GetAsync(id);

                return new ReturnResult<ResearcherConsultantDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ReturnResult<ResearcherConsultantDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<QueryResultDto<ResearcherConsultantListItemDto>>> GetAllAsync(ResearcherQueryObject queryObject)
        {
            try
            {
                var entitiy = await _researcherConsultantRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<ResearcherConsultantListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ReturnResult<QueryResultDto<ResearcherConsultantListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<ResearcherConsultantDto>> GetConsultantAsync(Guid researcherId)
        {
            try
            {
                var entitiy = await _researcherConsultantRepository.GetConsultantAsync(researcherId);

                return new ReturnResult<ResearcherConsultantDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ReturnResult<ResearcherConsultantDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<bool>> CheckCurrentResearcherHasConsultantAsync()
        {
            try
            {
                var entitiy = await _researcherConsultantRepository.CheckCurrentResearcherHasConsultantAsync();

                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

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
