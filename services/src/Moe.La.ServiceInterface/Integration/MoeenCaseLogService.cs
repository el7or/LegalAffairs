using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class MoeenCaseLogService : IMoeenCaseLogService
    {
        private readonly IMoeenCaseLogRepository _repository;
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseResearchersRepository _caseResearchersRepository;
        private readonly ILogger<MoeenCaseLogService> _logger;

        public MoeenCaseLogService(IMoeenCaseLogRepository repository, ICaseRepository caseRepository, ICaseResearchersRepository caseResearchersRepository, ILogger<MoeenCaseLogService> logger)
        {
            _repository = repository;
            _caseRepository = caseRepository;
            _caseResearchersRepository = caseResearchersRepository;
            _logger = logger;
        }
        public async Task<ReturnResult<MoeenCaseDto>> AddAsync(MoeenCaseDto model)
        {
            try
            {
                await _repository.AddAsync(model);

                // create CaseDto object, set the values, add to cases table

                var caseDto = new CaseDto()
                {
                    CaseSource = CaseSources.Moeen,
                    MoeenRef = model.MoeenRef,
                    CaseNumberInSource = model.MoeenId,
                    Subject = model.Subject,
                    LegalStatus = MinistryLegalStatuses.Defendant,
                    CourtId = 1,//model.Court,
                    CircleNumber = model.CircleNumber,
                    LitigationType = (LitigationTypes)(new Random().Next(1, 3)),
                    StartDate = model.StartDate,
                    CaseDescription = model.CaseDescription,
                    RelatedCaseId = model.RelatedCaseId
                };

                foreach (var party in model.Parties)
                {
                    caseDto.Parties.Add(new CasePartyDto()
                    {
                        CaseId = party.CaseId,
                        PartyId = party.PartyId
                    });
                }
                foreach (var hearing in model.Hearings)
                {
                    caseDto.Hearings.Add(new HearingDto()
                    {
                        HearingNumber = hearing.HearingNumber.Value,
                        CircleNumber = hearing.CircleNumber,
                        HearingDate = hearing.HearingDate,
                        HearingTime = hearing.HearingTime,
                        CourtId = 1, //hearing.CircleNumber,
                        HearingDesc = hearing.HearingDesc,
                        Status = HearingStatuses.Scheduled,
                        Type = HearingTypes.Pleading
                    });
                }
                // if new case has related case if related case not exists in db and it has invalid Litigation type then set related case to null.
                if (caseDto.RelatedCaseId != null)
                {
                    var relatedCase = await _caseRepository.GetAsync((int)caseDto.RelatedCaseId, includeAllData: false);
                    if (relatedCase == null
                        || relatedCase.LitigationType.Id == (int)caseDto.LitigationType
                        || caseDto.LitigationType == LitigationTypes.Appeal && relatedCase.LitigationType.Id != (int)LitigationTypes.FirstInstance
                        || caseDto.LitigationType == LitigationTypes.Supreme && relatedCase.LitigationType.Id != (int)LitigationTypes.Appeal)
                    {
                        caseDto.RelatedCaseId = null;
                    }
                }

                // if related case is assigned to researcher then assign new case to same researcher.
                if (caseDto.RelatedCaseId != null)
                {
                    var relatedCaseResearcher = await _caseResearchersRepository.GetByCaseAsync((int)caseDto.RelatedCaseId);
                    if (relatedCaseResearcher != null)
                    {
                        caseDto.Status = CaseStatuses.ReceivedByResearcher;
                        await _caseRepository.AddIntegratedCaseAsync(caseDto);
                        await _caseResearchersRepository.AddIntegratedResearcher(new CaseResearchersDto { ResearcherId = relatedCaseResearcher.ResearcherId, CaseId = caseDto.Id });
                    }
                    else
                    {
                        caseDto.Status = CaseStatuses.NewCase;
                        await _caseRepository.AddIntegratedCaseAsync(caseDto);
                    }
                }
                else
                {
                    caseDto.Status = CaseStatuses.NewCase;
                    caseDto.LitigationType = LitigationTypes.FirstInstance;
                    await _caseRepository.AddIntegratedCaseAsync(caseDto);

                }
                return new ReturnResult<MoeenCaseDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<MoeenCaseDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
