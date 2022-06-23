using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IHearingRepository
    {
        Task<QueryResultDto<HearingListItemDto>> GetAllAsync(HearingQueryObject queryObject);

        Task<HearingDetailsDto> GetAsync(int id);

        Task AddAsync(HearingDto hearingDto);

        Task EditAsync(HearingDto hearingDto);

        Task RemoveAsync(int id);

        Task ReceivingJudgmentAsync(ReceivingJudgmentDto receivingJudgmentDto);

        Task RemoveReceivingJudgmentDataAsync(int hearingId);


        //Task CloseAndCreateAsync(HearingCloseCreateDto hearing);

        Task<int> GetMaxHearingNumberAsync(int caseId);

        Task<int> GetFirstHearingIdAsync(int caseId);

        Task<bool> IsHearingNumberExistsAsync(HearingNumberDto hearing);

        Task<bool> IsHearingDateExistsAsync(HearingDto hearing);
        Task<bool> CheckPleadingHearingsDate(HearingDto hearing);

        Task<bool> IsFirstHearingAsync(HearingDto hearing);

        Task<bool> IsReceivedJudgmentCase(int caseId);

        Task<Hearing> GetCasePronouncingJudgmentHearing(int hearingId, int caseId);

        Task<bool> IsCaseSchedulingHearingExists(HearingDto hearing);

        Task<bool> CheckUserAsync(HearingDto hearing);

        Task<QueryResultDto<HearingListItemDto>> GetUpcomingHearingsAsync(int days);

        Task<QueryResultDto<HearingListItemDto>> GetUnclosedHearingsAsync(int days);

        Task<KeyValuePairsDto<Guid>> AssignHearingToAsync(int hearingId, Guid attendantId);

        Task FinishHearing();

        Task CloseHearing();

        Task<List<ResearcherHearingDto>> GetHearingsOrderedByResearcher();

        Task<List<Hearing>> ApproachHearingByResearcher();

        Task<List<Hearing>> GetHearingOutOfDate();

        Task<QueryResultDto<UserListItemDto>> GetConsultantsAndResearchers(UserQueryObject queryObject);

        Task UpdateHearingsCourtsAsync(int caseId, int newCourtId);
    }
}