using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IResearchsConsultantService
    {
        Task<ReturnResult<ResearcherConsultantDto>> GetAsync(int id);

        Task<ReturnResult<QueryResultDto<ResearcherConsultantListItemDto>>> GetAllAsync(ResearcherQueryObject queryObject);

        Task<ReturnResult<ResearcherConsultantDto>> AddAsync(ResearcherConsultantDto model);
        Task<ReturnResult<ResearcherConsultantDto>> GetConsultantAsync(Guid researherId);
        Task<ReturnResult<bool>> CheckCurrentResearcherHasConsultantAsync();
    }
}
