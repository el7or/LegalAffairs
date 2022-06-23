using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IFarisIntegrationService
    {
        Task<ReturnResult<InvestigationRecordPartyDetailsDto>> GetAsync(string searchText, int? investigationRecordId);
        ReturnResult<FaresUserDto> GetUserAsync(string id);
    }
}
