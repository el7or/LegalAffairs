using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IAttachmentRepository
    {
        Task<QueryResultDto<AttachmentListItemDto>> GetAllAsync(AttachmentQueryObject queryObject);

        Task<AttachmentListItemDto> AddAsync(AttachmentDto attachment);

        Task UpdateListAsync(List<AttachmentDto> attachments);

        Task RemoveAsync(Guid id);

        /// <summary>
        /// Delete all IsDraft attachments.
        /// </summary>
        /// <returns></returns>
        Task Cleanup();
    }
}
