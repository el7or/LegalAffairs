using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IMoeenCaseLogService
    {
        Task<ReturnResult<MoeenCaseDto>> AddAsync(MoeenCaseDto moeenCase);
    }
}
