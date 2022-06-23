using Moe.La.Core.Dtos;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ICaseTransactionRepository
    {
        Task AddAsync(CaseTransactionDto caseTransactionDto);
    }
}
