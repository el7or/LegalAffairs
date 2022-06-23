using Microsoft.Extensions.Logging;
using Moe.La.Common.Extensions;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators;
using Moe.La.ServiceInterface.Validators.Case;
using Moe.La.ServiceInterface.Validators.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class CaseService : ICaseService
    {
        private readonly IAddingObjectionLegalMemoToCaseRequestRepository _addingObjectionLegalMemoToCaseRequestRepository;
        private readonly IObjectionPermitRequestRepository _objectionPermitRequestRepository;
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseTransactionRepository _caseTransactionRepository;
        private readonly IHearingRepository _hearingRepository;
        private readonly ICaseResearcherService _caseResearchersService;
        private readonly INotificationService _notificationService;
        private readonly IResearchsConsultantService _researchsConsultantService;
        private readonly IUserService _userService;
        private readonly IAttachmentService _attachmentService;
        private readonly ILogger<CaseService> _logger;

        public CaseService(
            IAddingObjectionLegalMemoToCaseRequestRepository addingObjectionLegalMemoToCaseRequestRepository,
            IObjectionPermitRequestRepository objectionPermitRequestRepository,
            ICaseRepository caseRepository,
            ICaseTransactionRepository caseTransactionRepository,
            IHearingRepository hearingRepository,
            ICaseResearcherService caseResearchersService,
            INotificationService notificationService,
            IResearchsConsultantService researchsConsultantService,
            IUserService userService,
            IAttachmentService attachmentService,
            ILogger<CaseService> logger)
        {
            _addingObjectionLegalMemoToCaseRequestRepository = addingObjectionLegalMemoToCaseRequestRepository;
            _objectionPermitRequestRepository = objectionPermitRequestRepository;
            _caseRepository = caseRepository;
            _caseTransactionRepository = caseTransactionRepository;
            _hearingRepository = hearingRepository;
            _caseResearchersService = caseResearchersService;
            _notificationService = notificationService;
            _researchsConsultantService = researchsConsultantService;
            _userService = userService;
            _attachmentService = attachmentService;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<CaseListItemDto>>> GetAllAsync(CaseQueryObject queryObject)
        {
            try
            {
                var entities = await _caseRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<CaseListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<CaseListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public ReturnResult<List<CaseDto>> GetAllAsync()
        {
            try
            {
                var entities = _caseRepository.GetAllAsync();

                return new ReturnResult<List<CaseDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ReturnResult<List<CaseDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<CaseDetailsDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _caseRepository.GetAsync(id);



                if (entitiy is null)
                {
                    return new ReturnResult<CaseDetailsDto>(false, HttpStatuses.Status404NotFound, new List<string> { "البيانات المطلوبة غير متوفرة." });
                }

                return new ReturnResult<CaseDetailsDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<CaseDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<BasicCaseDto>> AddAsync(BasicCaseDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new CaseValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isCaseSourceNumberExists = await _caseRepository.IsCaseSourceNumberExistsAsync(model.CaseSource, model.CaseNumberInSource, model.StartDate);
                if (isCaseSourceNumberExists)
                {
                    errors.Add("رقم القضية موجود لنفس المصدر في نفس السنة");
                }

                if (model.RelatedCaseId.HasValue)
                {
                    bool isCaseStartDateValid = await _caseRepository.IsCaseStartDateValidAsync((int)model.RelatedCaseId, model.StartDate);
                    if (!isCaseStartDateValid)
                    {
                        errors.Add("يجب الا يسبق تاريخ بداية القضية تاريخ النطق بالحكم للقضية السابقة");
                    }
                }

                if (errors.Any())
                {
                    return new ReturnResult<BasicCaseDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors,
                        Data = null
                    };
                }


                // check if new case has related case.
                if (model.RelatedCaseId != null && model.Status != CaseStatuses.Draft)
                {
                    // if related case is assigned to researcher then assign new case to same researcher.
                    var relatedCaseResearcher = await _caseResearchersService.GetByCaseAsync((int)model.RelatedCaseId, null);
                    if (relatedCaseResearcher != null)
                    {
                        model.Status = CaseStatuses.ReceivedByResearcher;

                        await _caseRepository.AddAsync(model);

                        await _caseResearchersService.AddResearcher(new CaseResearchersDto { ResearcherId = relatedCaseResearcher.Data.ResearcherId, CaseId = model.Id });
                    }
                }
                else
                {
                    await _caseRepository.AddAsync(model);
                }

                // if case has moamala id
                if (model.MoamalaId != null)
                {
                    await AddCaseMoamalatAsync(new CaseMoamalatDto
                    {
                        CaseId = model.Id,
                        MoamalaId = model.MoamalaId.Value
                    });
                }

                return new ReturnResult<BasicCaseDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<BasicCaseDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<NextCaseDto>> CreateNextCase(NextCaseDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new NextCaseValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                // check case number not repeated for same caseSource
                bool isCaseSourceNumberExists = await _caseRepository.IsCaseSourceNumberExistsAsync(model.CaseSource, model.CaseNumberInSource, model.StartDate);
                if (isCaseSourceNumberExists)
                {
                    errors.Add("رقم القضية موجود لنفس المصدر في نفس السنة");
                }

                // check objection case type 
                if (model.LitigationType == LitigationTypes.Appeal
                    || model.LitigationType == LitigationTypes.Supreme)
                {
                    errors.Add("يجب ان تكون القضية الجديدة استئناف او عليا");
                }

                // check if valid start date
                bool isCaseStartDateValid = await _caseRepository.IsCaseStartDateValidAsync(model.RelatedCaseId, model.StartDate);
                if (!isCaseStartDateValid)
                {
                    errors.Add("يجب الا يسبق تاريخ بداية القضية تاريخ النطق بالحكم للقضية السابقة");
                }

                var relatedCaseJudgmentResult = await _caseRepository.GetJudgmentResult(model.RelatedCaseId);
                // if judgementResult is favor check if researcher created objectionPermitRequest for  objection and litigation manager accepted request.
                if (relatedCaseJudgmentResult == JudgementResults.Favor)
                {
                    var isCaseObjectionPermitRequestAccepted = await _objectionPermitRequestRepository.CheckCaseObjectionPermitRequestAcceptedAsync(model.RelatedCaseId);
                    if (!isCaseObjectionPermitRequestAccepted)
                    {
                        errors.Add("يجب موافقة مدير الترافع على بدء الاعتراض اولا ");
                    }
                }

                // check if researcher created objection request with memo and consultant accepted request
                var isCaseObjectionRequestAccepted = await _addingObjectionLegalMemoToCaseRequestRepository.CheckCaseObjectionRequestAcceptedAsync(model.RelatedCaseId);
                if (!isCaseObjectionRequestAccepted)
                {
                    errors.Add("يجب موافقة المستشار على مذكرة الاعتراض قبل انشاء قضية جديدة ");
                }


                if (errors.Any())
                {
                    return new ReturnResult<NextCaseDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors,
                        Data = null
                    };
                }

                // add next case
                await _caseRepository.AddNextAsync(model);
                ///

                // Add transaction  
                var caseTransactionDto = new CaseTransactionDto()
                {
                    CaseId = model.RelatedCaseId,
                    Note = "تم انشاء قضية  " + EnumExtensions.GetDescription(model.LitigationType) + " برقم " + model.CaseNumberInSource + " سنة " + model.StartDate.Year,
                    TransactionType = CaseTransactionTypes.SendToBranchManager
                };
                await AddTransactionAsync(caseTransactionDto);
                ////


                await NotificationAddNextCase(model, model.RelatedCaseId);

                return new ReturnResult<NextCaseDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<NextCaseDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<BasicCaseDto>> EditAsync(BasicCaseDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new CaseValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                if (model.RelatedCaseId.HasValue)
                {
                    bool isCaseStartDateValid = await _caseRepository.IsCaseStartDateValidAsync((int)model.RelatedCaseId, model.StartDate);
                    if (!isCaseStartDateValid)
                    {
                        errors.Add("يجب الا يسبق تاريخ بداية القضية تاريخ النطق بالحكم للقضية السابقة");
                    }
                }
                if (errors.Any())
                {
                    return new ReturnResult<BasicCaseDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors,
                        Data = null
                    };
                }

                // if court changed==> update court to all hearings belong a case
                var _case = await _caseRepository.GetAsync(model.Id, includeAllData: false);
                if (_case.CourtId != model.CourtId)
                {
                    await _hearingRepository.UpdateHearingsCourtsAsync(model.Id, model.CourtId);
                }

                // update case
                await _caseRepository.EditAsync(model);

                return new ReturnResult<BasicCaseDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<BasicCaseDto>
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

                await _caseRepository.RemoveAsync(id);

                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = true
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

        public async Task<ReturnResult<bool>> ChangeStatusAsync(CaseChangeStatusDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new CaseChangeStatusValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }
                if ((model.Status == CaseStatuses.SentToBranchManager))
                {
                    errors.Add("يجب تحديد الإدارة القانونية");

                }
                if (errors.Any())
                {
                    return new ReturnResult<bool>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors,
                        Data = false
                    };
                }

                await _caseRepository.ChangeStatusAsync(model);

                var caseTransactionDto = new CaseTransactionDto()
                {
                    CaseId = model.Id,
                    Note = model.Note,
                };

                if (model.Status == CaseStatuses.ReceivedByLitigationManager)
                {
                    caseTransactionDto.TransactionType = CaseTransactionTypes.ReceivedByLitigationManager;
                }
                if (model.Status == CaseStatuses.ReceivedByResearcher)
                {
                    caseTransactionDto.TransactionType = CaseTransactionTypes.SendToResearcher;
                }
                if (model.Status == CaseStatuses.SentToRegionsSupervisor)
                {
                    caseTransactionDto.TransactionType = CaseTransactionTypes.SendToRegionsSupervisor;
                }
                if (model.Status == CaseStatuses.ReceivedByRegionsSupervisor)
                {
                    caseTransactionDto.TransactionType = CaseTransactionTypes.ReceivedByRegionsSupervisor;
                }
                if (model.Status == CaseStatuses.ReturnedToLitigationManager)
                {
                    caseTransactionDto.TransactionType = CaseTransactionTypes.ReturnToLitigationManager;
                }
                if (model.Status == CaseStatuses.ReceivedByBranchManager)
                {
                    caseTransactionDto.TransactionType = CaseTransactionTypes.ReceivedByBranchManager;
                }
                if (model.Status == CaseStatuses.ReturnedToRegionsSupervisor)
                {
                    caseTransactionDto.TransactionType = CaseTransactionTypes.ReturnToRegionsSupervisor;
                }
                if (model.Status == CaseStatuses.ObjectionRecorded)
                {
                    caseTransactionDto.TransactionType = CaseTransactionTypes.RecordObjectionRequest;
                }
                await AddTransactionAsync(caseTransactionDto);

                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        #region Case Party
        public async Task<ReturnResult<List<CasePartyDto>>> GetCasePartiesAsync(int caseId)
        {
            try
            {
                var parties = await _caseRepository.GetCasePartiesAsync(caseId);

                return new ReturnResult<List<CasePartyDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = parties
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseId);

                return new ReturnResult<List<CasePartyDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }

        }

        public async Task<ReturnResult<CasePartyDto>> AddCasePartyAsync(CasePartyDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new CasePartyValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                if (errors.Any())
                {

                    return new ReturnResult<CasePartyDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }

                await _caseRepository.AddCasePartyAsync(model);

                return new ReturnResult<CasePartyDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<CasePartyDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<CasePartyDto>> UpdateCasePartyAsync(CasePartyDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new CasePartyValidator(), model);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<CasePartyDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = validationResult.Errors,
                        Data = null
                    };
                }

                await _caseRepository.UpdateCasePartyAsync(model);

                return new ReturnResult<CasePartyDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<CasePartyDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<bool>> DeleteCasePartyAsync(int id)
        {
            try
            {
                await _caseRepository.DeleteCasePartyAsync(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status201Created, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        #endregion

        #region Case Ground
        public async Task<ReturnResult<ICollection<CaseGroundsDto>>> GetAllGroundsAsync(int caseId)
        {
            try
            {
                var result = await _caseRepository.GetAllGroundsAsync(caseId);

                return new ReturnResult<ICollection<CaseGroundsDto>>(true, HttpStatuses.Status201Created, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseId);

                return new ReturnResult<ICollection<CaseGroundsDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<CaseGroundsDto>> AddGroundAsync(CaseGroundsDto caseGroundsDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new CaseGroundValidator(), caseGroundsDto);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<CaseGroundsDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _caseRepository.AddGroundAsync(caseGroundsDto);

                return new ReturnResult<CaseGroundsDto>(true, HttpStatuses.Status201Created, caseGroundsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseGroundsDto);

                return new ReturnResult<CaseGroundsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<CaseGroundsDto>> EditGroundAsync(CaseGroundsDto caseGroundsDto)
        {
            try
            {
                await _caseRepository.EditGroundAsync(caseGroundsDto);

                return new ReturnResult<CaseGroundsDto>(true, HttpStatuses.Status201Created, caseGroundsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseGroundsDto);

                return new ReturnResult<CaseGroundsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveGroundAsync(int id)
        {
            try
            {
                await _caseRepository.RemoveGroundAsync(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status201Created, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> UpdateAllGroundsAsync(CaseGroundsListDto caseGrounds)
        {
            try
            {
                var errors = new List<string>();

                foreach (var ground in caseGrounds.CaseGrounds)
                {
                    var validationResult = ValidationResult.CheckModelValidation(new CaseGroundValidator(), ground);
                    if (!validationResult.IsValid)
                    {
                        errors.AddRange(validationResult.Errors);
                    }
                }

                if (errors.Any())
                {

                    return new ReturnResult<bool>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }


                await _caseRepository.UpdateAllGroundsAsync(caseGrounds);

                return new ReturnResult<bool>(true, HttpStatuses.Status201Created, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseGrounds.CaseId);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        #endregion

        #region Case Moamalat

        public async Task<ReturnResult<ICollection<CaseMoamalatDto>>> GetCaseMoamalatAsync(int caseId)
        {
            try
            {
                var result = await _caseRepository.GetCaseMoamalatAsync(caseId);

                return new ReturnResult<ICollection<CaseMoamalatDto>>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseId);

                return new ReturnResult<ICollection<CaseMoamalatDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<CaseMoamalatDto>> AddCaseMoamalatAsync(CaseMoamalatDto caseMoamalatDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new CaseMoamalatValidator(), caseMoamalatDto);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<CaseMoamalatDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _caseRepository.AddCaseMoamalatAsync(caseMoamalatDto);

                return new ReturnResult<CaseMoamalatDto>(true, HttpStatuses.Status201Created, caseMoamalatDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseMoamalatDto);

                return new ReturnResult<CaseMoamalatDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveCaseMoamalatAsync(int caseId, int moamalaId)
        {
            try
            {
                await _caseRepository.RemoveCaseMoamalatAsync(caseId, moamalaId);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseId);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        #endregion

        public async Task<ReturnResult<CaseDetailsDto>> SendToBranchManagerAsync(CaseSendToBranchManagerDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new CaseSendToBranchManagerValidator(), model);
                if (!validationResult.IsValid)
                {

                    return new ReturnResult<CaseDetailsDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = validationResult.Errors,
                        Data = null
                    };
                }
                // Add transaction  

                var caseTransactionDto = new CaseTransactionDto()
                {
                    CaseId = model.Id,
                    Note = model.Note,
                    TransactionType = CaseTransactionTypes.SendToBranchManager
                };


                await AddTransactionAsync(caseTransactionDto);

                // update case legal department 
                var result = await _caseRepository.ChangeDepartment(model);


                return new ReturnResult<CaseDetailsDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<CaseDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<bool>> UpdateAttachmentsAsync(CaseAttachmentsListDto data)
        {
            try
            {
                await _caseRepository.EditAttachmentsAsync(data);

                await _attachmentService.UpdateListAsync(data.Attachments);

                return new ReturnResult<bool>(true, HttpStatuses.Status201Created, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, data.CaseId);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> AssignResearcherAsync(CaseResearchersDto caseResearchersDto)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new CaseResearchersValidator(), caseResearchersDto);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                //var isResearcher = await _userRepository.IsInRole(caseResearchersDto.ResearcherId, ApplicationRolesConstants.LegalResearcher.Code);

                //if (!isResearcher)
                //{
                //    errors.Add("الباحث المحدد لا ينتمي إلى مجموعة الباحثين.");
                //}
                // check if researcher selected is connected with a consultant
                var researcherConsultant = await _researchsConsultantService.GetAllAsync(new ResearcherQueryObject { ResearcherId = caseResearchersDto.ResearcherId, HasConsultant = true });

                if (researcherConsultant.Data.TotalItems == 0)
                {
                    errors.Add("لايوجد مستشار معين لهذا الباحث ");
                }

                if (errors.Any())
                {

                    return new ReturnResult<bool>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }

                var caseChangeStatusDto = new CaseChangeStatusDto()
                {
                    Id = caseResearchersDto.CaseId,
                    Status = CaseStatuses.ReceivedByResearcher,
                };

                await _caseRepository.ChangeStatusAsync(caseChangeStatusDto);

                var caseTransactionDto = new CaseTransactionDto()
                {
                    CaseId = caseResearchersDto.CaseId,
                    Note = caseResearchersDto.Note,
                    TransactionType = CaseTransactionTypes.SendToResearcher
                };

                await AddTransactionAsync(caseTransactionDto);

                await _caseResearchersService.AddResearcher(caseResearchersDto);

                return new ReturnResult<bool>(true, HttpStatuses.Status201Created, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseResearchersDto);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<CaseDetailsDto>> GetParentCaseAsync(int id)
        {
            try
            {
                var entitiy = await _caseRepository.GetParentCase(id);

                return new ReturnResult<CaseDetailsDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<CaseDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }


        public async Task<ReturnResult<CaseRuleDto>> AddRuleAsync(CaseRuleDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new CaseRuleValidator(), model);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<CaseRuleDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = validationResult.Errors,
                        Data = null
                    };
                }
                await _caseRepository.AddRuleAsync(model);

                return new ReturnResult<CaseRuleDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<CaseRuleDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<ReceiveJdmentInstrumentDto>> ReceiveJudmentInstrumentAsync(ReceiveJdmentInstrumentDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ReceiveJdmentInstumentaValidator(), model);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ReceiveJdmentInstrumentDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = validationResult.Errors,
                        Data = null
                    };
                }
                await _caseRepository.ReceiveJudgmentInstrumentAsync(model);

                if (model.Attachments != null && model.Attachments.Count > 0)
                    await _attachmentService.UpdateListAsync(model.Attachments);

                var caseTransactionDto = new CaseTransactionDto()
                {
                    CaseId = model.Id,
                    TransactionType = CaseTransactionTypes.ReceiveJudmentInstrument,
                    Note = $"تم استلام صك الحكم"
                };

                await AddTransactionAsync(caseTransactionDto);

                return new ReturnResult<ReceiveJdmentInstrumentDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);
                return new ReturnResult<ReceiveJdmentInstrumentDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<ReceiveJdmentInstrumentDetailsDto>> GetReceiveJudmentInstrumentAsync(int Id)
        {
            try
            {
                var caseRuleEntitiy = await _caseRepository.GetReceiveJudgmentInstrumentAsync(Id);
                return new ReturnResult<ReceiveJdmentInstrumentDetailsDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = caseRuleEntitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, Id);

                return new ReturnResult<ReceiveJdmentInstrumentDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<ReceiveJdmentInstrumentDto>> EditReceiveJudmentInstrumentAsync(ReceiveJdmentInstrumentDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ReceiveJdmentInstumentaValidator(), model);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ReceiveJdmentInstrumentDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = validationResult.Errors,
                        Data = null
                    };
                }
                await _caseRepository.EditReceiveJudgmentInstrumentAsync(model);

                await _attachmentService.UpdateListAsync(model.Attachments);

                var caseTransactionDto = new CaseTransactionDto()
                {
                    CaseId = model.Id,
                    TransactionType = CaseTransactionTypes.EditReceiveJudmentInstrument,
                    Note = $"تم تعديل صك الحكم"
                };

                await AddTransactionAsync(caseTransactionDto);
                return new ReturnResult<ReceiveJdmentInstrumentDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);
                return new ReturnResult<ReceiveJdmentInstrumentDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<Task> DetermineJudgment()
        {
            try
            {
                _logger.LogInformation($"{nameof(DetermineJudgment)} job is running...");

                var _cases = await _caseRepository.DetermineJudgment();

                if (_cases != null && _cases.Count > 0)
                {
                    foreach (var _case in _cases)
                    {

                        await _caseRepository.UpdateDetermineJudment(_case);

                        var caseTransactionDto = new CaseTransactionDto()
                        {
                            CaseId = _case.Id,
                            TransactionType = CaseTransactionTypes.FinalJudgment,
                            Note = $"تم تحويل الحكم لحكم نهائي"
                        };

                        await AddTransactionAsync(caseTransactionDto);
                    }
                }

                _logger.LogInformation($"{nameof(DetermineJudgment)} job ended successfully.");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromException(ex);
            }
        }

        public async Task<ReturnResult<InitialCaseDto>> CreateCaseAsync(InitialCaseDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new InitialCaseValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isCaseSourceNumberExists = await _caseRepository.IsCaseSourceNumberExistsAsync(model.CaseSource, model.CaseNumberInSource, model.StartDate);
                if (isCaseSourceNumberExists)
                {
                    errors.Add("رقم القضية موجود لنفس المصدر في نفس السنة");
                }

                if (errors.Any())
                {
                    return new ReturnResult<InitialCaseDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors,
                        Data = null
                    };
                }

                await _caseRepository.CreateAsync(model);

                if (model.CaseCategory != null)
                {
                    if (model.CaseCategory.Id == 0)
                    {
                        //var categoryAdded = await _caseCategoryService.AddAsync(new CaseCategoryDto { Name = category.Name, CaseSource = (CaseSources)model.CaseSource });
                        //await _caseRepository.AddCategory(model.Id, categoryAdded.Data.Id);
                    }
                    else
                    {
                        await _caseRepository.AddCategory(model.Id, model.CaseCategory.Id);
                    }

                    //await _caseRepository.ChangeStatusAsync(new CaseChangeStatusDto { Id = model.Id, Status = CaseStatuses.Categorized });
                }

                return new ReturnResult<InitialCaseDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<InitialCaseDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<ObjectionCaseDto>> CreateObjectionCaseAsync(ObjectionCaseDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new ObjectionCaseValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isCaseSourceNumberExists = await _caseRepository.IsCaseSourceNumberExistsAsync(model.CaseSource, model.CaseSourceNumber, model.StartDate);
                if (isCaseSourceNumberExists)
                {
                    errors.Add("رقم القضية موجود لنفس المصدر في نفس السنة");
                }

                if (errors.Any())
                {
                    return new ReturnResult<ObjectionCaseDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors,
                        Data = null
                    };
                }
                //await _caseRepository.ChangeStatusAsync(new CaseChangeStatusDto { Id = model.RelatedCaseId, Status = CaseStatuses.ObjectionRecorded });
                var objectionCase = await _caseRepository.CreateObjectionCase(model);
                await AssignResearcherAsync(new CaseResearchersDto { CaseId = objectionCase.Id, ResearcherId = objectionCase.ResearcherId });

                var userIds = new List<Guid>();

                // add consultant
                var consultantId = (await _researchsConsultantService.GetConsultantAsync(objectionCase.ResearcherId)).Data.ConsultantId;
                if (consultantId.HasValue)
                    userIds.Add((Guid)consultantId);

                // add litigationManagers &   GeneralSupervisors
                //if (objectionCase.GeneralManagmentId.HasValue)
                //{
                var litigationManagers = (await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.DepartmentManager.Code, objectionCase.GeneralManagmentId, (int)Departments.Litigation)).Data;
                userIds.AddRange(litigationManagers.Select(m => m.Id).ToList());
                var GeneralSupervisors = (await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.GeneralSupervisor.Code, null, null)).Data;
                userIds.AddRange(GeneralSupervisors.Select(g => g.Id).ToList());
                //}

                await _notificationService.AddAsync(new NotificationDto
                {
                    Type = NotificationTypes.Info,
                    UserIds = userIds,
                    Text = String.Format("تم انشاء قضية جديدة على طلب الاعتراض للقضية رقم {0} ", objectionCase.RelatedCaseId),
                    SendSMSMessage = true,
                    URL = "cases/view/" + objectionCase.RelatedCaseId
                });
                return new ReturnResult<ObjectionCaseDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<ObjectionCaseDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<bool>> RemoveJudgmentInstrumentAsync(int caseId)
        {
            try
            {
                await _caseRepository.RemoveJudgmentInstrumentAsync(caseId);

                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseId);

                return new ReturnResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<JudgementResults>> GetJudgmentResult(int caseId)
        {
            try
            {
                var entity = await _caseRepository.GetJudgmentResult(caseId);

                return new ReturnResult<JudgementResults>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseId);

                return new ReturnResult<JudgementResults>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        //ارسال اشعارات عند عدم تسجيل الاعتراض على الحكم غير النهائي 
        public async Task<Task> NotificationNotRecordedObjection()
        {
            try
            {
                _logger.LogInformation($"{nameof(NotificationNotRecordedObjection)} job is starting...");

                var _cases = await _caseRepository.GetNotFinalJudjmentCases(5);

                if (_cases != null && _cases.Count > 0)
                {
                    foreach (var _case in _cases)
                    {
                        var researcherId = (await _caseResearchersService.GetByCaseAsync(_case.Id)).Data.ResearcherId;
                        await _notificationService.AddAsync(new NotificationDto
                        {
                            Type = NotificationTypes.Warning,
                            Text = String.Format("    تم بدء مهلة الاعتراض على القضية رقم {0} فى الدائرة رقم {1} محكمة {2} و تنتهى المهلة بتاريخ {3}  ", _case.CaseNumberInSource, _case.CircleNumber, _case.Court.Name, Common.DateTimeHelper.GetHigriDate(_case.ReceivingJudgmentDate.Value.AddDays(30))),
                            URL = "cases/view/" + _case.Id,
                            UserIds = new List<Guid>() { researcherId },
                            SendEmailMessage = true,
                            SendSMSMessage = true
                        });
                    }
                }

                _cases = await _caseRepository.GetNotFinalJudjmentCases(18);

                if (_cases != null && _cases.Count > 0)
                {
                    foreach (var _case in _cases)
                    {
                        var researcherId = await _caseResearchersService.GetByCaseAsync(_case.Id);
                        var consultantId = await _researchsConsultantService.GetConsultantAsync(researcherId.Data.ResearcherId);
                        await _notificationService.AddAsync(new NotificationDto
                        {
                            Type = NotificationTypes.Warning,
                            Text = String.Format("لم يتم تقديم الاعتراض للقضية رقم {0} فى الدائرة رقم {1} محكمة {2} و تاريخ نهاية مهلة الاعتراض {3} .",
                            _case.CaseNumberInSource, _case.CircleNumber, _case.Court.Name, Common.DateTimeHelper.GetHigriDate(_case.ReceivingJudgmentDate.Value.AddDays(30))),
                            URL = "cases/view/" + _case.Id,
                            UserIds = new List<Guid>() { consultantId.Data.ConsultantId.Value },
                            SendEmailMessage = true,
                            SendSMSMessage = true
                        });
                    }
                }

                _cases = await _caseRepository.GetNotFinalJudjmentCases(20);

                if (_cases != null && _cases.Count > 0)
                {
                    foreach (var _case in _cases)
                    {
                        var litigationManager = await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.DepartmentManager.Code, _case.BranchId, (int)Departments.Litigation);//(new UserQueryObject { Roles = ApplicationRolesConstants.DepartmentManager.Name, PageSize = 999, Page = 1, GeneralManagementId = _case.GeneralManagementId });
                        await _notificationService.AddAsync(new NotificationDto
                        {
                            Type = NotificationTypes.Warning,
                            URL = "cases/view/" + _case.Id,
                            Text = String.Format("لم يتم تقديم الاعتراض للقضية رقم {0} فى الدائرة رقم {1} محكمة {2} و تاريخ نهاية مهلة الاعتراض {3} .",
                            _case.CaseNumberInSource, _case.CircleNumber, _case.Court.Name, Common.DateTimeHelper.GetHigriDate(_case.ReceivingJudgmentDate.Value.AddDays(30))),
                            UserIds = litigationManager.Data.Select(u => u.Id).ToList(),
                            SendEmailMessage = true,
                            SendSMSMessage = true
                        });
                    }
                }

                _cases = await _caseRepository.GetNotFinalJudjmentCases(25);

                if (_cases != null && _cases.Count > 0)
                {
                    foreach (var _case in _cases)
                    {
                        var generalSupervisors = await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.GeneralSupervisor.Code, _case.BranchId, null);//(new UserQueryObject { Roles = ApplicationRolesConstants.GeneralSupervisor.Name, PageSize = 999, Page = 1, GeneralManagementId = _case.GeneralManagementId });
                        await _notificationService.AddAsync(new NotificationDto
                        {
                            Type = NotificationTypes.Warning,
                            URL = "cases/view/" + _case.Id,
                            Text = String.Format("لم يتم تقديم الاعتراض للقضية رقم {0} فى الدائرة رقم {1} محكمة {2} و تاريخ نهاية مهلة الاعتراض {3} .",
                             _case.CaseNumberInSource, _case.CircleNumber, _case.Court.Name, Common.DateTimeHelper.GetHigriDate(_case.ReceivingJudgmentDate.Value.AddDays(30))),
                            UserIds = generalSupervisors.Data.Select(u => u.Id).ToList(),
                            SendEmailMessage = true,
                            SendSMSMessage = true
                        });
                    }
                }

                _logger.LogInformation($"{nameof(NotificationNotRecordedObjection)} job ended successfully.");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromException(ex);
            }
        }


        //ارسال اشعار انشاء قضية استئناف او عليا 
        public async Task<Task> NotificationAddNextCase(NextCaseDto model, int relatedCaseId)
        {
            try
            {
                var caseResearchers = (await _caseResearchersService.GetByCaseAsync(relatedCaseId, null)).Data;

                var userIds = new List<Guid>();

                // add researcher
                userIds.Add(caseResearchers.ResearcherId);

                // add consultant
                var consultantId = (await _researchsConsultantService.GetConsultantAsync(caseResearchers.ResearcherId)).Data.ConsultantId;
                if (consultantId.HasValue)
                    userIds.Add((Guid)consultantId);

                var litigationManagers = (await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.DepartmentManager.Code, model.BranchId, (int)Departments.Litigation)).Data;

                userIds.AddRange(litigationManagers.Select(m => m.Id).ToList());

                await _notificationService.AddAsync(new NotificationDto
                {
                    Type = NotificationTypes.Info,
                    Text = "تم انشاء قضية  " + EnumExtensions.GetDescription(model.LitigationType) + " برقم " + model.CaseNumberInSource + " سنة " + model.StartDate.Year,
                    URL = "cases/view/" + model.Id,
                    UserIds = userIds,
                    SendEmailMessage = true,
                    SendSMSMessage = true
                });


                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromException(ex);
            }
        }

        public async Task<ReturnResult<CaseTransactionDto>> AddTransactionAsync(CaseTransactionDto CaseTransactionDto)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new CaseTransactionValidator(), CaseTransactionDto);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }
                await _caseTransactionRepository.AddAsync(CaseTransactionDto);

                return new ReturnResult<CaseTransactionDto>(true, HttpStatuses.Status200OK, CaseTransactionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, CaseTransactionDto);

                return new ReturnResult<CaseTransactionDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        //public async Task<ReturnResult<CasesStatistics>> GetStatisticsAsync(CaseQueryObjectDto model)
        //{
        //    try
        //    {
        //        var entitiy = await _caseRepository.GetStatistics(model);

        //        return new ReturnResult<CasesStatistics>
        //        {
        //            IsSuccess = true,
        //            StatusCode = StatusCode.Status200OK,
        //            Data = entitiy
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message, model);

        //        return new ReturnResult<CasesStatistics>
        //        {
        //            IsSuccess = false,
        //            StatusCode = StatusCode.Status500InternalServerError,
        //            ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
        //        };
        //    }
        //}
    }

}
