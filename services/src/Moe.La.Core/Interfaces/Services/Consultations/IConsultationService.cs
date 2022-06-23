using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IConsultationService
    {
        Task<ReturnResult<QueryResultDto<ConsultationListItemDto>>> GetAllAsync(ConsultationQueryObject queryObject);

        Task<ReturnResult<ConsultationDetailsDto>> GetAsync(int id);

        Task<ReturnResult<ConsultationDto>> AddAsync(ConsultationDto model);

        Task<ReturnResult<ConsultationReviewDto>> ConsultationReview(ConsultationReviewDto consultationReview);

        Task<ReturnResult<ConsultationDto>> EditAsync(ConsultationDto model);

        Task<ReturnResult<bool>> DeleteVisualAsync(int id);
    }
}
