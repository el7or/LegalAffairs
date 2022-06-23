using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IResearcherConsultantRepository
    {
        Task AddAsync(ResearcherConsultantDto researcherConsultant);
        Task AddToHistoryAsync(ResearcherConsultantDto researcherConsultantDto);

        Task<ResearcherConsultantDto> GetAsync(int id);

        Task<QueryResultDto<ResearcherConsultantListItemDto>> GetAllAsync(ResearcherQueryObject queryObject);

        Task<bool> CheckCurrentResearcherHasConsultantAsync();

        Task<ResearcherConsultantDto> GetConsultantAsync(Guid researcherId);
    }
}
