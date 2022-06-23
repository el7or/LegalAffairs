using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IAttachmentService
    {
        Task<ReturnResult<QueryResultDto<AttachmentListItemDto>>> GetAllAsync(AttachmentQueryObject queryObject);

        Task<ReturnResult<AttachmentListItemDto>> AddAsync(AttachmentDto attachment);

        Task<ReturnResult<bool>> UpdateListAsync(List<AttachmentDto> attachment);

        Task<ReturnResult<bool>> RemoveAsync(Guid id);

        /// <summary>
        /// Clean attachments by deleting the drafts.
        /// This method is called by a background job.
        /// </summary>
        /// <returns></returns>
        Task<Task> Cleanup();
    }
}
