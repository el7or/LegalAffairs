using Moe.La.Core.Dtos;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IExportCaseJudgmentRequestRepository
    {
        Task<ExportCaseJudgmentRequestListItemDto> GetAsync(int id);

        Task<ExportCaseJudgmentRequestDetailsDto> AddAsync(ExportCaseJudgmentRequestDto exportCaseJudgmentRequestDto);

        Task<ExportCaseJudgmentRequestDetailsDto> EditAsync(ExportCaseJudgmentRequestDto exportCaseJudgmentRequestDto);

        Task<ExportCaseJudgmentRequestDto> ReplyExportCaseJudgmentRequestAsync(ReplyExportCaseJudgmentRequestDto replyExportCaseJudgmentRequestDto);

        Task<ExportCaseJudgmentRequestForPrintDto> GetForPrintAsync(int id);
    }
}
