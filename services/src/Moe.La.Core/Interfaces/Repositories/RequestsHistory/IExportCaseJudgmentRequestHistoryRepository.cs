using Moe.La.Core.Dtos;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IExportCaseJudgmentRequestHistoryRepository
    {
        Task AddAsync(int id);
        Task<ExportCaseJudgmentRequestHistoryListItemDto> GetAsync(int id);
    }
}
