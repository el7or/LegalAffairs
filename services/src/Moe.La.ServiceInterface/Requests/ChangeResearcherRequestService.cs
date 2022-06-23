using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class ChangeResearcherRequestService : IChangeResearcherRequestService
    {
        private readonly IChangeResearcherRequestRepository _changeResearcherRequestRepository;
        private readonly ICaseResearcherService _caseResearchersService;
        private readonly IRequestService _requestService;
        private readonly ICaseService _caseService;
        private readonly ILogger<ChangeResearcherRequestService> _logger;

        public ChangeResearcherRequestService(
            IChangeResearcherRequestRepository changeResearcherRequestRepository,
            ICaseResearcherService caseResearchersService,
            IRequestService requestService, ICaseService caseService,
            ILogger<ChangeResearcherRequestService> logger)
        {
            _changeResearcherRequestRepository = changeResearcherRequestRepository;
            _caseResearchersService = caseResearchersService;
            _requestService = requestService;
            _caseService = caseService;
            _logger = logger;
        }

        public async Task<ReturnResult<ChangeResearcherRequestListItemDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _changeResearcherRequestRepository.GetAsync(id);

                if (entitiy == null)
                {
                    return new ReturnResult<ChangeResearcherRequestListItemDto>(false, HttpStatuses.Status404NotFound, new List<string> { "البيانات المطلوبة غير متوفرة" });
                }

                return new ReturnResult<ChangeResearcherRequestListItemDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<ChangeResearcherRequestListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ChangeResearcherRequestDetailsDto>> AddAsync(ChangeResearcherRequestDto changeResearcherRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ChangeResearcherRequestValidator(), changeResearcherRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ChangeResearcherRequestDetailsDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                var changeResearcherRequest = await _changeResearcherRequestRepository.AddAsync(changeResearcherRequestDto);

                // Add Transaction
                var requestTransactionDto = new RequestTransactionDto()
                {
                    RequestId = changeResearcherRequest.Id,
                    RequestStatus = RequestStatuses.New,
                    TransactionType = RequestTransactionTypes.Create,
                    Description = ""
                };

                await _requestService.AddTransactionAsync(requestTransactionDto);

                return new ReturnResult<ChangeResearcherRequestDetailsDto>(true, HttpStatuses.Status201Created, changeResearcherRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, changeResearcherRequestDto);

                return new ReturnResult<ChangeResearcherRequestDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ReplyChangeResearcherRequestDto>> AcceptChangeResearcherRequest(ReplyChangeResearcherRequestDto replyChangeResearcherRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ReplyChangeResearcherRequestValidator(), replyChangeResearcherRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ReplyChangeResearcherRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                // if the current ResearcherId and new ResearcherId are the same ==> throw exception
                if (replyChangeResearcherRequestDto.CurrentResearcherId == replyChangeResearcherRequestDto.NewResearcherId)
                {
                    return new ReturnResult<ReplyChangeResearcherRequestDto>(false, HttpStatuses.Status400BadRequest, new List<string> { "الباحث الجديد المختار هو نفسة الباحث الحالي، الرجاء اختيار باحث آخر." });
                }

                // save the reply note and date and change request status
                var changeResearcherRequestDto = await _changeResearcherRequestRepository
                     .ReplyChangeResearcherRequestAsync(replyChangeResearcherRequestDto, RequestStatuses.Accepted);

                // get the current case researcher and then delete it
                var caseResearcher = await _caseResearchersService.GetByCaseAsync(changeResearcherRequestDto.CaseId.Value, changeResearcherRequestDto.CurrentResearcher.Id);

                await _caseResearchersService.RemoveAsync(caseResearcher.Data.Id);

                // add the new case researcher
                var newCaseResearcher = new CaseResearchersDto
                {
                    ResearcherId = replyChangeResearcherRequestDto.NewResearcherId.Value,
                    CaseId = changeResearcherRequestDto.CaseId.Value,
                    Note = changeResearcherRequestDto.ReplyNote
                };

                await _caseResearchersService.AddResearcher(newCaseResearcher);

                // Add Transaction
                string description = "";
                if (changeResearcherRequestDto.LegalMemoId.HasValue)
                {
                    description = "قبول طلب رقم <<" + changeResearcherRequestDto.Id + ">> تغيير الباحث من <<" + changeResearcherRequestDto.Request.CreatedBy.Name + ">> بصفته أمين للجنة المذكرات عن المذكرة رقم <<" + changeResearcherRequestDto.LegalMemoId.Value + ">> .";
                }
                else if (changeResearcherRequestDto.CaseId.HasValue)
                {
                    description = "قبول طلب رقم <<" + changeResearcherRequestDto.Id + ">> تغيير الباحث من <<" + changeResearcherRequestDto.Request.CreatedBy.Name + ">> بصفته مكلف كباحث للقضية رقم <<" + changeResearcherRequestDto.CaseId.Value + ">> .";

                }
                await _caseService.AddTransactionAsync(new CaseTransactionDto
                {
                    CaseId = changeResearcherRequestDto.CaseId.Value,
                    TransactionType = CaseTransactionTypes.AcceptChangeResearcherRequest,
                    Note = description
                });

                //تغيير الباحث للقضايا المرتبطه بالقضية الاساسية
                #region changeResearchersForTheCurrentCase
                //var caseLitigationType=await _caseRepository.GetCaseLititgationType(changeResearcherRequestDto.CaseId.Value);
                //var caseIds = new List<int>();
                //if (caseLitigationType!=null && caseLitigationType == LitigationTypes.FirstInstance)
                //{
                //    var relatedCaseId = await _caseRepository.GetCaseByRelatedId(changeResearcherRequestDto.CaseId.Value);
                //    if (relatedCaseId != 0)
                //    {
                //        caseIds.Add(relatedCaseId);
                //        var caseId = await _caseRepository.GetCaseByRelatedId(relatedCaseId);
                //        if (caseId != 0)
                //            caseIds.Add(caseId);
                //    }
                //}
                //else if(caseLitigationType != null && caseLitigationType == LitigationTypes.Appeal)
                //{
                //    var relatedCaseId = await _caseRepository.GetCaseByRelatedId(changeResearcherRequestDto.CaseId.Value);
                //    caseIds.Add(relatedCaseId);
                //}

                //foreach (var item in caseIds)
                //{
                //    // get the current case researcher and then delete it
                //    var relatedCaseResearcher = await _caseResearchersRepository.GetByCaseAsync(item, changeResearcherRequestDto.CurrentResearcher.Id);

                //    await _caseResearchersRepository.RemoveAsync(relatedCaseResearcher.Id);

                //    // add the new case researcher
                //    var newRelatedCaseResearcher = new CaseResearchersDto
                //    {
                //        ResearcherId = replyChangeResearcherRequestDto.NewResearcherId.Value,
                //        CaseId = item,
                //        Note = changeResearcherRequestDto.ReplyNote
                //    };

                //    await _caseResearchersRepository.AddResearcher(newRelatedCaseResearcher);

                //    // Add Transaction
                //    string RrelatedDescription = "";
                //    if (changeResearcherRequestDto.LegalMemoId.HasValue)
                //    {
                //        RrelatedDescription = "قبول طلب رقم <<" + changeResearcherRequestDto.Id + ">> تغيير الباحث من <<" + changeResearcherRequestDto.Request.CreatedBy.Name + ">> بصفته أمين للجنة المذكرات عن المذكرة رقم <<" + changeResearcherRequestDto.LegalMemoId.Value + ">> .";
                //    }
                //    else if (changeResearcherRequestDto.CaseId.HasValue)
                //    {
                //        RrelatedDescription = "قبول طلب رقم <<" + changeResearcherRequestDto.Id + ">> تغيير الباحث من <<" + changeResearcherRequestDto.Request.CreatedBy.Name + ">> بصفته مكلف كباحث للقضية الاساسية رقم <<" + changeResearcherRequestDto.CaseId.Value + ">> .";

                //    }
                //    await _caseService.AddAsync(new CaseTransactionDto
                //    {
                //        CaseId = item,
                //        TransactionType = CaseTransactionTypes.AcceptChangeResearcherRequest,
                //        Note = description
                //    });
                //}
                #endregion


                var requestTransactionDto = new RequestTransactionDto()
                {
                    RequestId = changeResearcherRequestDto.Id,
                    RequestStatus = RequestStatuses.Accepted,
                    TransactionType = RequestTransactionTypes.RequestAccepted,
                    Description = description
                };

                await _requestService.AddTransactionAsync(requestTransactionDto);

                return new ReturnResult<ReplyChangeResearcherRequestDto>(true, HttpStatuses.Status200OK, replyChangeResearcherRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, replyChangeResearcherRequestDto);

                return new ReturnResult<ReplyChangeResearcherRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ReplyChangeResearcherRequestDto>> RejectChangeResearcherRequest(ReplyChangeResearcherRequestDto replyChangeResearcherRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ReplyChangeResearcherRequestValidator(), replyChangeResearcherRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ReplyChangeResearcherRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                // save the reply note and date and change request status
                var changeResearcherRequestDto = await _changeResearcherRequestRepository
                     .ReplyChangeResearcherRequestAsync(replyChangeResearcherRequestDto, RequestStatuses.Rejected);

                // Add Transaction
                string description = "";
                if (changeResearcherRequestDto.LegalMemoId.HasValue)
                {
                    description = "رفض طلب رقم <<" + changeResearcherRequestDto.Id + ">> تغيير الباحث من <<" + changeResearcherRequestDto.Request.CreatedBy.Name + ">> بصفته أمين للجنة المذكرات عن المذكرة رقم <<" + changeResearcherRequestDto.LegalMemoId.Value + ">> .";
                }
                else if (changeResearcherRequestDto.CaseId.HasValue)
                {
                    description = "رفض طلب رقم <<" + changeResearcherRequestDto.Id + ">> تغيير الباحث من <<" + changeResearcherRequestDto.Request.CreatedBy.Name + ">> بصفته مكلف كباحث للقضية رقم <<" + changeResearcherRequestDto.CaseId.Value + ">> .";
                }

                await _caseService.AddTransactionAsync(new CaseTransactionDto
                {
                    CaseId = changeResearcherRequestDto.CaseId.Value,
                    TransactionType = CaseTransactionTypes.RejectChangeResearcherRequest,
                    Note = description
                });

                var requestTransactionDto = new RequestTransactionDto()
                {
                    RequestId = changeResearcherRequestDto.Id,
                    RequestStatus = RequestStatuses.Rejected,
                    TransactionType = RequestTransactionTypes.RequestRejected,
                    Description = description
                };

                await _requestService.AddTransactionAsync(requestTransactionDto);

                return new ReturnResult<ReplyChangeResearcherRequestDto>(true, HttpStatuses.Status200OK, replyChangeResearcherRequestDto);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, replyChangeResearcherRequestDto);

                return new ReturnResult<ReplyChangeResearcherRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
