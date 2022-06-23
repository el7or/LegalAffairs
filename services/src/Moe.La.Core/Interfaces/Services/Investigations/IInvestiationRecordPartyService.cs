using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IInvestiationRecordPartyService
    {
        Task<ReturnResult<bool>> CheckPartyExistAsync(string identityNumber, int? investigationRecordId);
    }
}
