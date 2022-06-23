using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IHearingLegalMemoService
    {
        Task<ReturnResult<HearingLegalMemoDto>> AddAsync(int requestId);

        Task<ReturnResult<HearingLegalMemoDetailsDto>> GetByMemoAsync(int legalMemoId);

        Task<ReturnResult<bool>> HearingHasLegalMomo(int hearingId);

    }
}