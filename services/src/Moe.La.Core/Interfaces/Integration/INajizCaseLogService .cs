using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface INajizCaseLogService
    {
        Task<ReturnResult<NajizCaseDto>> AddAsync(NajizCaseDto najizCase);
    }
}
