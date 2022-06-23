using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IConsultationRepository
    {
        Task<QueryResultDto<ConsultationListItemDto>> GetAllAsync(ConsultationQueryObject queryObject);

        Task<ConsultationDetailsDto> GetAsync(int id);

        Task AddAsync(ConsultationDto consultationDto);

        Task<ConsultationReviewDto> ConsultationReviewAsync(ConsultationReviewDto consultationReviewDto);

        Task EditAsync(ConsultationDto consultationDto);

        Task DeleteVisualAsync(int visualId);

    }
}
