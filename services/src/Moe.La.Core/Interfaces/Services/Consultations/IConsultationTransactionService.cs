using Moe.La.Core.Dtos;
using Moe.La.Core.Dtos.Consultations;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IConsultationTransactionService
    {
        Task<ReturnResult<ConsultationTransactionDto>> AddAsync(ConsultationTransactionDto model);
        Task<ReturnResult<QueryResultDto<ConsultationTransactionListDto>>> GetAllAsync(ConsultationTransactionQueryObject queryObject);
    }
}