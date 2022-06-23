using Moe.La.Core.Dtos;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IHearingLegalMemoRepository
    {
        Task<HearingLegalMemoDetailsDto> GetByMemoAsync(int legalMemoId);

        Task<HearingLegalMemoDto> AddAsync(int RequestId);
        Task<bool> HearingHasLegalMomo(int hearingId);
    }
}