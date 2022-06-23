using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface ICaseService
    {
        /// <summary>
        /// Get all cases.
        /// </summary>
        /// <returns></returns>
        ReturnResult<List<CaseDto>> GetAllAsync();

        /// <summary>
        /// Get all cases based on query object.
        /// </summary>
        /// <param name="queryObject">Query object used to filter cases.</param>
        /// <returns></returns>
        Task<ReturnResult<QueryResultDto<CaseListItemDto>>> GetAllAsync(CaseQueryObject queryObject);

        /// <summary>
        /// Get a case
        /// </summary>
        /// <param name="id">Case ID.</param>
        /// <returns></returns>
        Task<ReturnResult<CaseDetailsDto>> GetAsync(int id);

        /// <summary>
        /// Add case.
        /// </summary>
        /// <param name="model">Case object to be added.</param>
        /// <returns></returns>
        Task<ReturnResult<BasicCaseDto>> AddAsync(BasicCaseDto model);

        Task<ReturnResult<NextCaseDto>> CreateNextCase(NextCaseDto model);

        /// <summary>
        /// Edit case.
        /// </summary>
        /// <param name="model">Case model to be edited.</param>
        /// <returns></returns>
        Task<ReturnResult<BasicCaseDto>> EditAsync(BasicCaseDto model);

        /// <summary>
        /// Remove case (logical delete).
        /// </summary>
        /// <param name="id">Case ID. to be deleted.</param>
        /// <returns></returns>
        Task<ReturnResult<bool>> RemoveAsync(int id);

        /// <summary>
        /// Change case status.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ReturnResult<bool>> ChangeStatusAsync(CaseChangeStatusDto model);

        Task<ReturnResult<CaseDetailsDto>> SendToBranchManagerAsync(CaseSendToBranchManagerDto model);

        Task<ReturnResult<bool>> AssignResearcherAsync(CaseResearchersDto caseResearchersDto);

        Task<ReturnResult<CaseDetailsDto>> GetParentCaseAsync(int id);

        Task<ReturnResult<CaseRuleDto>> AddRuleAsync(CaseRuleDto model);

        Task<ReturnResult<ReceiveJdmentInstrumentDto>> ReceiveJudmentInstrumentAsync(ReceiveJdmentInstrumentDto model);

        Task<ReturnResult<ReceiveJdmentInstrumentDetailsDto>> GetReceiveJudmentInstrumentAsync(int Id);

        Task<ReturnResult<ReceiveJdmentInstrumentDto>> EditReceiveJudmentInstrumentAsync(ReceiveJdmentInstrumentDto model);


        Task<Task> DetermineJudgment();
        /// <summary>
        /// create case by litigationManager
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ReturnResult<InitialCaseDto>> CreateCaseAsync(InitialCaseDto model);

        Task<ReturnResult<ObjectionCaseDto>> CreateObjectionCaseAsync(ObjectionCaseDto model);

        Task<ReturnResult<bool>> RemoveJudgmentInstrumentAsync(int caseId);

        Task<Task> NotificationNotRecordedObjection();

        Task<ReturnResult<JudgementResults>> GetJudgmentResult(int caseId);

        Task<ReturnResult<CaseTransactionDto>> AddTransactionAsync(CaseTransactionDto model);

        Task<ReturnResult<CasePartyDto>> AddCasePartyAsync(CasePartyDto model);

        Task<ReturnResult<CasePartyDto>> UpdateCasePartyAsync(CasePartyDto model);

        Task<ReturnResult<bool>> UpdateAttachmentsAsync(CaseAttachmentsListDto attachments);

        Task<ReturnResult<List<CasePartyDto>>> GetCasePartiesAsync(int caseId);

        Task<ReturnResult<bool>> DeleteCasePartyAsync(int id);

        #region case-ground
        Task<ReturnResult<ICollection<CaseGroundsDto>>> GetAllGroundsAsync(int caseId);

        Task<ReturnResult<CaseGroundsDto>> AddGroundAsync(CaseGroundsDto caseGroundsDto);

        Task<ReturnResult<CaseGroundsDto>> EditGroundAsync(CaseGroundsDto caseGroundsDto);

        Task<ReturnResult<bool>> RemoveGroundAsync(int id);

        Task<ReturnResult<bool>> UpdateAllGroundsAsync(CaseGroundsListDto caseGrounds);
        #endregion

        #region Case Moamalat

        Task<ReturnResult<ICollection<CaseMoamalatDto>>> GetCaseMoamalatAsync(int caseId);

        Task<ReturnResult<CaseMoamalatDto>> AddCaseMoamalatAsync(CaseMoamalatDto caseMoamalatDto);

        Task<ReturnResult<bool>> RemoveCaseMoamalatAsync(int caseId, int moamalaId);

        #endregion
    }

}
