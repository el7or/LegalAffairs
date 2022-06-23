using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface ICaseResearcherService
    {
        Task<ReturnResult<CaseResearchersDto>> GetByCaseAsync(int id);

        Task<ReturnResult<CaseResearchersDto>> GetByCaseAsync(int id, Guid? ResearcherId = null);

        Task<ReturnResult<CaseResearchersDto>> AddResearcher(CaseResearchersDto model);

        Task<ReturnResult<CaseResearchersDto>> AddIntegratedResearcher(CaseResearchersDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);
    }
}
