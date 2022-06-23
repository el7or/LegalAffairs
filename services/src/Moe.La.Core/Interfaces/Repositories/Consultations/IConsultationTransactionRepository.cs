using Moe.La.Core.Dtos;
using Moe.La.Core.Dtos.Consultations;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IConsultationTransactionRepository
    {
        Task AddAsync(ConsultationTransactionDto transactionDto);

        Task<QueryResultDto<ConsultationTransactionListDto>> GetAllAsync(ConsultationTransactionQueryObject queryObject);
    }
}