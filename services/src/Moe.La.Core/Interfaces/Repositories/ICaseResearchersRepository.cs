using Moe.La.Core.Dtos;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ICaseResearchersRepository
    {
        Task<CaseResearchersDto> GetByCaseAsync(int caseId);

        Task<CaseResearchersDto> GetByCaseAsync(int caseId, Guid? ResearcherId = null);

        Task AddResearcher(CaseResearchersDto caseResearchersDto);

        Task AddIntegratedResearcher(CaseResearchersDto caseResearchersDto);

        Task RemoveAsync(int id);
    }
}