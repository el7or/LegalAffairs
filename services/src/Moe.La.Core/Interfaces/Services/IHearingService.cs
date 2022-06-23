using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IHearingService
    {
        Task<ReturnResult<QueryResultDto<HearingListItemDto>>> GetAllAsync(HearingQueryObject queryObject);

        Task<ReturnResult<HearingDetailsDto>> GetAsync(int id);

        Task<ReturnResult<HearingDto>> AddAsync(HearingDto model);

        Task<ReturnResult<HearingDto>> EditAsync(HearingDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<HearingDto>> ReceivingJudgmentAsync(ReceivingJudgmentDto model);

        //Task<ReturnResult<HearingDto>> CloseAndCreateAsync(HearingCloseCreateDto hearing);

        Task<ReturnResult<int>> GetMaxHearingNumberAsync(int caseId);

        Task<ReturnResult<int>> GetFirstHearingIdAsync(int caseId);

        Task<ReturnResult<bool>> IsHearingNumberExistsAsync(HearingNumberDto hearingDto);

        Task<ReturnResult<QueryResultDto<HearingListItemDto>>> GetUpcomingHearingsAsync(int days);

        Task<ReturnResult<string>> SendUpcomingHearingsNotificationsToConsultantAsync(int days);

        Task<ReturnResult<string>> SendUpcomingHearingsNotificationsToAdministratorAsync(int days);

        Task<ReturnResult<string>> SendClosingHearingsNotificationsToConsultantAsync(int days);

        Task<ReturnResult<string>> SendClosingHearingsNotificationsToAdminstratorAsync(int days);

        Task<ReturnResult<KeyValuePairsDto<Guid>>> AssignHearingToAsync(int heraingId, Guid attendantId);

        Task<Task> FinishHearing();

        Task<Task> CloseHearing();

        Task<Task> NotifyResearcherToCompleteHearingOutOfDate();

        Task<Task> NotifyManagerWithInCompletedHearingOutOfDate();

        Task<Task> NotifyUsersWithApproachHearingDate();

        Task<ReturnResult<QueryResultDto<UserListItemDto>>> GetConsultantsAndResearchers(UserQueryObject queryObject);
    }
}