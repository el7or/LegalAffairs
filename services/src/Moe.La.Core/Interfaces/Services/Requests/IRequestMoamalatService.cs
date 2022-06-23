using Moe.La.Core.Dtos;
using Moe.La.Core.Dtos.Requests;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IRequestMoamalatService
    {
        Task<ReturnResult<RequestMoamalatDto>> AddAsync(RequestMoamalatDto model);

        Task<ReturnResult<ExportRequestDto>> ExportRequest(ExportRequestDto exportRequestDto);

    }
}