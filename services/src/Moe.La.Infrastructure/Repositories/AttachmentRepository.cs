using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using Moe.La.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{

    public class AttachmentRepository : RepositoryBase, IAttachmentRepository
    {
        public AttachmentRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<AttachmentListItemDto>> GetAllAsync(AttachmentQueryObject queryObject)
        {
            var result = new QueryResult<Attachment>();
            IQueryable<Attachment> query = null;

            if (queryObject.GroupName == GroupNames.Case)
            {
                query = db.CaseAttachments.Where(c => c.CaseId == queryObject.GroupId)
                    .Include(a => a.Attachment)
                    //.ThenInclude(t => t.AttachmentType)
                    .Select(a => a.Attachment)
                    .AsQueryable();

                // check if any attachment in hearings under this case
                var caseHearings = db.Hearings.Where(h => h.CaseId == queryObject.GroupId).AsQueryable();
                if (caseHearings != null)
                {
                    foreach (var hearing in caseHearings)
                    {
                        var hearingAttachments = db.HearingAttachments.Where(c => c.HearingId == hearing.Id)
                            .Include(a => a.Attachment)
                            //.ThenInclude(t => t.AttachmentType)
                            .Select(a => a.Attachment)
                            .AsQueryable();

                        if (hearingAttachments != null)
                        {
                            if (query != null) query = query.Union(hearingAttachments);
                            else query = hearingAttachments;
                        }
                    }
                }
            }

            if (queryObject.GroupName == GroupNames.Hearing)
            {
                query = db.HearingAttachments.Where(c => c.HearingId == queryObject.GroupId)
                    .Include(a => a.Attachment)
                    //.ThenInclude(t => t.AttachmentType)
                    .Select(a => a.Attachment)
                    .AsQueryable();
            }

            if (queryObject.GroupName == GroupNames.CaseRule)
            {
                query = db.CaseRuleAttachments.Where(c => c.CaseRuleId == queryObject.GroupId)
                    .Include(a => a.Attachment)
                    //.ThenInclude(t => t.AttachmentType)
                    .Select(a => a.Attachment)
                    .AsQueryable();
            }

            if (queryObject.GroupName == GroupNames.HearingUpdate)
            {
                query = db.HearingUpdateAttachment.Where(c => c.HearingUpdateId == queryObject.GroupId)
                    .Include(a => a.Attachment)
                    //.ThenInclude(t => t.AttachmentType)
                    .Select(a => a.Attachment)
                    .AsQueryable();
            }

            if (queryObject.GroupName == GroupNames.Moamala)
            {
                query = db.MoamalaAttachments.Where(c => c.MoamalaId == queryObject.GroupId)
                    .Include(a => a.Attachment)
                    //.ThenInclude(t => t.AttachmentType)
                    .Select(a => a.Attachment)
                    .AsQueryable();
            }

            var columnsMap = new Dictionary<string, Expression<Func<Attachment, object>>>()
            {
                ["name"] = v => v.Name,
                //["type"] = v => v.AttachmentType.Name,
                ["size"] = v => v.Size
            };

            query = query.ApplySorting(queryObject, columnsMap);


            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();
            return mapper.Map<QueryResult<Attachment>, QueryResultDto<AttachmentListItemDto>>(result);
        }

        public async Task<AttachmentListItemDto> AddAsync(AttachmentDto data)
        {
            var attachment = new Attachment();
            attachment.Name = data.File.FileName;
            attachment.AttachmentTypeId = data.AttachmentTypeId;
            attachment.Size = Convert.ToInt32(data.File.Length);

            attachment.CreatedBy = CurrentUser.UserId;
            attachment.CreatedOn = DateTime.Now;

            await db.Attachments.AddAsync(attachment);
            await db.SaveChangesAsync();

            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), ApplicationConstants.UploadLocation);

            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var fileName = attachment.Id + Path.GetExtension(data.File.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await data.File.CopyToAsync(stream);
            }

            // to return attachment type
            attachment = await db.Attachments.Include(a => a.AttachmentType).Where(a => a.Id == attachment.Id).FirstOrDefaultAsync();
            ///

            return mapper.Map<Attachment, AttachmentListItemDto>(attachment);

        }


        public async Task UpdateListAsync(List<AttachmentDto> attachments)
        {
            foreach (var item in attachments)
            {
                var entityToUpdate = await db.Attachments.FindAsync(item.Id);

                entityToUpdate.AttachmentTypeId = item.AttachmentTypeId;
                entityToUpdate.Name = item.Name;
                entityToUpdate.IsDeleted = item.IsDeleted;

                entityToUpdate.IsDraft = false;
            }

            await db.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var entityToDelete = await db.Attachments.FindAsync(id);
            entityToDelete.IsDeleted = true;

            var fileName = entityToDelete.Id.ToString() + Path.GetExtension(entityToDelete.Name);
            var filePath = Path.Combine(ApplicationConstants.UploadLocation + fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

            var caseAttachmnets = await db.CaseAttachments.Where(c => c.Id == entityToDelete.Id).ToListAsync();
            if (caseAttachmnets.Count > 0) { db.CaseAttachments.RemoveRange(caseAttachmnets); }

            var hearingsAttachmnets = await db.HearingAttachments.Where(c => c.Id == entityToDelete.Id).ToListAsync();
            if (hearingsAttachmnets.Count > 0) { db.HearingAttachments.RemoveRange(hearingsAttachmnets); }

            var hearingsUpdateAttachmnets = await db.HearingUpdateAttachment.Where(c => c.Id == entityToDelete.Id).ToListAsync();
            if (hearingsUpdateAttachmnets.Count > 0) { db.HearingUpdateAttachment.RemoveRange(hearingsUpdateAttachmnets); }

            await db.SaveChangesAsync();
        }

        public async Task Cleanup()
        {
            var attachmentsToDelete = await db.Attachments.Where(m => m.IsDraft && m.CreatedOn.Date <= DateTime.Now.Date.AddDays(-1))
                .ToListAsync();

            if (attachmentsToDelete.Count > 0)
            {
                foreach (var attach in attachmentsToDelete)
                {
                    var fileName = attach.Id.ToString() + Path.GetExtension(attach.Name);
                    var filePath = Path.Combine("..\\Moe.La.WebApi\\Attachments", fileName); // TODO: add path to configuration.
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }

                db.Attachments.RemoveRange(attachmentsToDelete);
                await db.SaveChangesAsync();
            }
        }


    }
}
