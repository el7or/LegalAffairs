using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Dtos.Workflow;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories.Workflow
{
    public class WorkflowInstanceRepository : RepositoryBase, IWorkflowInstanceRepository
    {
        public WorkflowInstanceRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task AddWorkflowInstanceAsync(WorkflowInstanceDto workflowInstance)
        {
            if (workflowInstance is null)
            {
                throw new ArgumentNullException(nameof(workflowInstance));
            }

            workflowInstance.Id = Guid.NewGuid();
            workflowInstance.CreatedOn = DateTime.Now;

            var entityToAdd = mapper.Map<WorkflowInstance>(workflowInstance);
            await db.WorkflowInstances.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
            mapper.Map(entityToAdd, workflowInstance);
        }

        public async Task AddWorkflowInstanceLogAsync(WorkflowInstanceLogDto workflowInstanceLog)
        {
            if (workflowInstanceLog is null)
            {
                throw new ArgumentNullException(nameof(workflowInstanceLog));
            }

            workflowInstanceLog.CreatedOn = DateTime.Now;

            var entityToAdd = mapper.Map<WorkflowInstanceLog>(workflowInstanceLog);
            await db.WorkflowInstanceLogs.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
            mapper.Map(entityToAdd, workflowInstanceLog);
        }

        public async Task DeleteWorkflowInstanceLogAsync(Guid workflowInstanceLogId)
        {
            if (workflowInstanceLogId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowInstanceLogId));
            }

            var entityToDelete = await db.WorkflowInstanceLogs.FindAsync(workflowInstanceLogId);
            db.WorkflowInstanceLogs.Remove(entityToDelete);
            await db.SaveChangesAsync();
        }

        public async Task<WorkflowInstanceDto> GetWorkflowInstanceAsync(Guid workflowInstanceId)
        {
            if (workflowInstanceId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowInstanceId));
            }

            var entity = await db.WorkflowInstances
                .Include(m => m.WorkflowType)
                .Include(m => m.CurrentStatus)
                .Include(m => m.CurrentStep.WorkflowStepsPermissions)
                .Include(m => m.WorkflowInstancesLogs)
                .Where(m => m.Id == workflowInstanceId)
                .FirstOrDefaultAsync();

            return mapper.Map<WorkflowInstanceDto>(entity);
        }

        public async Task<WorkflowInstanceActionDto> GetWorkflowInstanceActionAsync(Guid workflowStepId, Guid workflowActionId)
        {
            if (workflowStepId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowStepId));
            }

            if (workflowActionId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowActionId));
            }

            return await (from a in db.WorkflowStepsActions
                          from b in db.WorkflowActions
                          where a.WorkflowActionId == b.Id && a.WorkflowStepId == workflowStepId && a.WorkflowActionId == workflowActionId
                          select new WorkflowInstanceActionDto
                          {
                              Id = b.Id,
                              ActionArName = b.ActionArName,
                              WorkflowTypeId = b.WorkflowTypeId,
                              Description = a.Description,
                              IsNoteVisible = a.IsNoteVisible,
                              IsNoteRequired = a.IsNoteRequired
                          }).FirstOrDefaultAsync();
        }

        public async Task<IList<WorkflowInstanceActionDto>> GetWorkflowInstanceAvailableActionsAsync(Guid workflowInstanceId)
        {
            if (workflowInstanceId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowInstanceId));
            }

            var workflowInstance = await db.WorkflowInstances.FindAsync(workflowInstanceId);

            return await (from a in db.WorkflowStepsActions
                          from b in db.WorkflowActions
                          where a.WorkflowActionId == b.Id && a.WorkflowStepId == workflowInstance.CurrentStepId
                          select new WorkflowInstanceActionDto
                          {
                              Id = b.Id,
                              ActionArName = b.ActionArName,
                              WorkflowTypeId = b.WorkflowTypeId,
                              Description = a.Description,
                              IsNoteVisible = a.IsNoteVisible,
                              IsNoteRequired = a.IsNoteRequired
                          }).ToListAsync();
        }

        public async Task<IList<WorkflowInstanceLogDto>> GetWorkflowInstanceLogsAsync(Guid workflowInstanceId)
        {
            if (workflowInstanceId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowInstanceId));
            }

            var entities = await db.WorkflowInstanceLogs
                .Where(m => m.WorkflowInstanceId == workflowInstanceId)
                .OrderByDescending(m => m.CreatedOn)
                .ToListAsync();

            return mapper.Map<IList<WorkflowInstanceLogDto>>(entities);
        }

        public async Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(Guid workflowTypeId, Guid? workflowStepId, int? workflowStatusId)
        {
            if (workflowTypeId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowTypeId));
            }

            var entities = await db.WorkflowInstances
                .Include(m => m.WorkflowType)
                .Include(m => m.CurrentStatus)
                .Include(m => m.CurrentStep)
                .Where(m => m.WorkflowTypeId == workflowTypeId && (m.CurrentStepId == workflowStepId || workflowStepId == null) && (m.CurrentStatusId == workflowStatusId || workflowStatusId == null))
                .ToListAsync();

            return mapper.Map<IList<WorkflowInstanceDto>>(entities);
        }

        public Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(List<int> permissions, Guid? lockedBy, Guid? claimedBy)
        {
            //await commandDb.WorkflowInstances
            //    .Include(m => m.WorkflowStepPermission)
            throw new NotImplementedException();
        }

        public Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(Guid createdFor, Guid? workflowTypeId, IList<int> statuses)
        {
            throw new NotImplementedException();
        }

        public Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(Guid lockedBy, IList<int> statuses)
        {
            throw new NotImplementedException();
        }

        public Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(Guid assignedTo)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(Guid[] workflowInstancesIds)
        {
            if (workflowInstancesIds == null)
            {
                throw new ArgumentNullException(nameof(workflowInstancesIds));
            }

            var entities = await db.WorkflowInstances
                .Include(m => m.WorkflowType)
                .Include(m => m.CurrentStep)
                .Include(m => m.CurrentStatus)
                .Where(m => workflowInstancesIds.Contains(m.Id))
                .ToListAsync();

            return mapper.Map<IList<WorkflowInstanceDto>>(entities);
        }

        public async Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(Guid? lockedBy, Guid? claimedBy)
        {
            if (lockedBy == null && claimedBy == null)
            {
                return null;
            }

            var entities = await db.WorkflowInstances
                .Include(m => m.WorkflowType)
                .Include(m => m.CurrentStep)
                .Include(m => m.CurrentStatus)
                .Where(m => (m.LockedBy == lockedBy || lockedBy == null) && (m.ClaimedBy == claimedBy || claimedBy == null))
                .ToListAsync();

            return mapper.Map<IList<WorkflowInstanceDto>>(entities);
        }

        public async Task RollBackWorkflowInstanceAsync(WorkflowInstanceDto workflowInstance)
        {
            if (workflowInstance is null)
            {
                throw new ArgumentNullException(nameof(workflowInstance));
            }

            var entityToDelete = mapper.Map<WorkflowInstance>(workflowInstance);
            db.WorkflowInstances.Attach(entityToDelete);
            db.WorkflowInstances.Remove(entityToDelete);
            await db.SaveChangesAsync();
        }

        public async Task SetInstanceLockAsync(WorkflowInstanceDto workflowInstance)
        {
            if (workflowInstance is null)
            {
                throw new ArgumentNullException(nameof(workflowInstance));
            }

            var entityToLock = mapper.Map<WorkflowInstance>(workflowInstance);
            entityToLock.UpdatedBy = CurrentUser.UserId;
            entityToLock.UpdatedOn = DateTime.Now;
            db.WorkflowInstances.Attach(entityToLock);
            db.Entry(entityToLock).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task UnlockAllWorkflowInstancesAsync(Guid workflowTypeId)
        {
            if (workflowTypeId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowTypeId));
            }

            var workflowInstances = await db.WorkflowInstances
                .Where(m => m.WorkflowTypeId == workflowTypeId && m.LockedBy.HasValue)
                .ToListAsync();

            foreach (var workflowInstance in workflowInstances)
            {
                workflowInstance.LockedOn = null;
                workflowInstance.LockedBy = null;
                db.Entry(workflowInstance).State = EntityState.Modified;
            }

            await db.SaveChangesAsync();
        }

        public async Task UnlockAllWorkflowInstancesAsync(Guid lockedBy, bool all = true)
        {
            if (lockedBy == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(lockedBy));
            }

            var workflowInstances = await db.WorkflowInstances
                .Where(m => m.LockedBy == lockedBy)
                .ToListAsync();

            throw new NotImplementedException();
        }

        public async Task UpdateWorkflowInstanceAsync(WorkflowInstanceDto workflowInstance)
        {
            if (workflowInstance is null)
            {
                throw new ArgumentNullException(nameof(workflowInstance));
            }

            var entityToEdit = mapper.Map<WorkflowInstance>(workflowInstance);
            entityToEdit.UpdatedBy = CurrentUser.UserId;
            entityToEdit.UpdatedOn = DateTime.Now;
            db.WorkflowInstances.Attach(entityToEdit);
            db.Entry(entityToEdit).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
