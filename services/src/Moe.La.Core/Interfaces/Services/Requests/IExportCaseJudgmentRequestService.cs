using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IExportCaseJudgmentRequestService
    {
        Task<ReturnResult<ExportCaseJudgmentRequestListItemDto>> GetAsync(int id);

        Task<ReturnResult<ExportCaseJudgmentRequestDetailsDto>> AddAsync(ExportCaseJudgmentRequestDto exportCaseJudgmentRequestDto);

        Task<ReturnResult<ExportCaseJudgmentRequestDto>> EditAsync(ExportCaseJudgmentRequestDto exportCaseJudgmentRequestDto);

        Task<ReturnResult<ExportCaseJudgmentRequestDto>> ReplyExportCaseJudgmentRequest(ReplyExportCaseJudgmentRequestDto replyExportCaseJudgmentRequestDto);

        Task<ExportCaseJudgmentRequestForPrintDto> GetForPrintAsync(int id);

    }
}