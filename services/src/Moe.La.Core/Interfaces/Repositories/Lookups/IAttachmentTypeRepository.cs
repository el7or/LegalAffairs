using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IAttachmentTypeRepository
    {
        Task<QueryResultDto<AttachmentTypeListItemDto>> GetAllAsync(AttachmentQueryObject queryObject);

        Task<AttachmentTypeDetailsDto> GetAsync(int id);

        Task AddAsync(AttachmentTypeDto attachmentTypeDto);

        Task EditAsync(AttachmentTypeDto attachmentTypeDto);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(AttachmentTypeDto attachmentTypeDto);
    }
}
