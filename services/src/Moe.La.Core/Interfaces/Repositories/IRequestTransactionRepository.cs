using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IRequestTransactionRepository
    {
        Task AddAsync(RequestTransactionDto transactionDto);

        Task<QueryResultDto<RequestTransactionListDto>> GetAllAsync(TransactionQueryObject queryObject);
    }
}