using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ICaseRepository
    {
        Task<QueryResultDto<CaseListItemDto>> GetAllAsync(CaseQueryObject queryObject);

        List<CaseDto> GetAllAsync();

        Task<CaseDetailsDto> GetAsync(int id, bool includeAllData = true);

        Task AddAsync(BasicCaseDto caseDto);

        Task AddNextAsync(NextCaseDto caseDto);

        Task AddIntegratedCaseAsync(CaseDto caseDto);

        Task EditAsync(BasicCaseDto caseDto);

        Task RemoveAsync(int id);

        Task ChangeStatusAsync(CaseChangeStatusDto caseChangeStatusDto);

        Task AddCaseHistoryAsync(Case caseEntity);

        Task EditAttachmentsAsync(CaseAttachmentsListDto caseDto);

        Task<CaseDetailsDto> ChangeDepartment(CaseSendToBranchManagerDto caseSendToBranchManagerDto);

        Task<CaseDetailsDto> GetParentCase(int id);

        Task AddRuleAsync(CaseRuleDto caseRuleDto);

        Task ReceiveJudgmentInstrumentAsync(ReceiveJdmentInstrumentDto receiveJdmentInstrumentDto);

        Task RemoveJudgmentInstrumentAsync(int caseId);

        Task<ReceiveJdmentInstrumentDetailsDto> GetReceiveJudgmentInstrumentAsync(int Id);

        Task EditReceiveJudgmentInstrumentAsync(ReceiveJdmentInstrumentDto receiveJdmentInstrumentDto);
        Task AddCategory(int caseId, int categoryId);

        /// <summary>
        /// Determine and specify a case judgmnet resutl, whether it's final judgment or not.
        /// This method is used by a system job.
        /// </summary>
        /// <returns></returns>
        Task<List<Case>> DetermineJudgment();
        Task UpdateDetermineJudment(Case _case);


        Task<JudgementResults> GetJudgmentResult(int caseId);

        Task CreateAsync(InitialCaseDto caseDto);

        Task<bool> IsCaseSourceNumberExistsAsync(CaseSources caseSource, string caseSourceNumber, DateTime startDate);

        Task<bool> IsCaseStartDateValidAsync(int relatedCaseId, DateTime startDate);

        Task<ObjectionCaseDto> CreateObjectionCase(ObjectionCaseDto objectionCaseDto);

        Task<List<Case>> GetNotFinalJudjmentCases(int days);

        Task<int> GetCaseByRelatedId(int relatedCaseId);

        Task<LitigationTypes?> GetCaseLititgationType(int caseId);

        #region Case Party

        Task AddCasePartyAsync(CasePartyDto partyDto);

        Task UpdateCasePartyAsync(CasePartyDto partyDto);

        Task DeleteCasePartyAsync(int id);

        Task<List<CasePartyDto>> GetCasePartiesAsync(int caseId);

        #endregion

        #region Case Grounds

        Task<ICollection<CaseGroundsDto>> GetAllGroundsAsync(int caseId);

        Task AddGroundAsync(CaseGroundsDto caseGroundsDto);

        Task EditGroundAsync(CaseGroundsDto caseGroundsDto);

        Task RemoveGroundAsync(int id);

        Task UpdateAllGroundsAsync(CaseGroundsListDto caseGrounds);

        #endregion

        #region Case Moamalat

        Task<List<CaseMoamalatDto>> GetCaseMoamalatAsync(int caseId);

        Task AddCaseMoamalatAsync(CaseMoamalatDto caseMoamalatDto);

        Task RemoveCaseMoamalatAsync(int caseId, int moamalaId);

        #endregion
    }
}
