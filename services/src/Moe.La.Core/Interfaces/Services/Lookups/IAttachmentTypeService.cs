using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IAttachmentTypeService
    {
        Task<ReturnResult<QueryResultDto<AttachmentTypeListItemDto>>> GetAllAsync(AttachmentQueryObject queryObject);

        Task<ReturnResult<AttachmentTypeDetailsDto>> GetAsync(int id);

        Task<ReturnResult<AttachmentTypeDto>> AddAsync(AttachmentTypeDto model);

        Task<ReturnResult<AttachmentTypeDto>> EditAsync(AttachmentTypeDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(AttachmentTypeDto model);
    }
}
