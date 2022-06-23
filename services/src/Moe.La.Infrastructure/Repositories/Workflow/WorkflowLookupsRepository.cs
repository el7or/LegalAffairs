using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Dtos.Workflow;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class WorkflowLookupsRepository : RepositoryBase, IWorkflowLookupsRepository
    {
        public WorkflowLookupsRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        #region WorkflowAction

        public async Task<WorkflowActionDto> GetWorkflowActionAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var entity = await db.WorkflowActions.FindAsync(id);

            return mapper.Map<WorkflowActionDto>(entity);
        }

        public async Task<IList<WorkflowActionListItemDto>> GetWorkflowActionsAsync()
        {
            var entities = await db.WorkflowActions
                .Include(m => m.WorkflowType)
                .ToListAsync();

            return mapper.Map<IList<WorkflowActionListItemDto>>(entities);
        }

        public async Task<IList<WorkflowActionListItemDto>> GetWorkflowActionsAsync(Guid workflowTypeId)
        {
            if (workflowTypeId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowTypeId));
            }

            var entities = await db.WorkflowActions
                .Include(m => m.WorkflowType)
                .Where(m => m.WorkflowTypeId == workflowTypeId)
                .ToListAsync();

            return mapper.Map<IList<WorkflowActionListItemDto>>(entities);
        }

        #endregion

        #region WorkflowStatus

        public async Task<IList<WorkflowStatusDto>> GetWorkflowStatusesAsync()
        {
            var entities = await db.WorkflowStatuses.ToListAsync();
            return mapper.Map<IList<WorkflowStatusDto>>(entities);
        }

        #endregion

        #region WorkflowStepAction

        public async Task<WorkflowStepActionDto> GetWorkflowStepActionAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var workflowStepAction = await db.WorkflowStepsActions.FindAsync(id);
            return mapper.Map<WorkflowStepActionDto>(workflowStepAction);
        }

        public async Task<IList<WorkflowInstanceActionDto>> GetWorkflowStepActionsByWorkflowStepAsync(Guid workflowStepId)
        {
            if (workflowStepId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowStepId));
            }

            var workflowStepActions = await (from a in db.WorkflowStepsActions
                                             from b in db.WorkflowActions
                                             where a.WorkflowActionId == b.Id && a.WorkflowStepId == workflowStepId
                                             select new WorkflowInstanceActionDto
                                             {
                                                 Id = b.Id,
                                                 ActionArName = b.ActionArName,
                                                 WorkflowTypeId = b.WorkflowTypeId,
                                                 Description = a.Description,
                                                 IsNoteVisible = a.IsNoteVisible,
                                                 IsNoteRequired = a.IsNoteRequired
                                             }).ToListAsync();

            return workflowStepActions;
        }

        public async Task<IList<WorkflowStepActionListItemDto>> GetWorkflowStepActionsByWorkflowTypeAsync(Guid workflowTypeId)
        {
            if (workflowTypeId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowTypeId));
            }

            var workflowStepActions = await (from a in db.WorkflowStepsActions
                                             from b in db.WorkflowSteps
                                             from c in db.WorkflowTypes
                                             from d in db.WorkflowStatuses
                                             where a.WorkflowStepId == b.Id && b.WorkflowTypeId == c.Id && a.NextStatusId == d.Id && c.Id == workflowTypeId
                                             select new WorkflowStepActionListItemDto
                                             {
                                                 Id = a.Id,
                                                 WorkflowStepId = a.WorkflowStepId,
                                                 WorkflowStepName = a.WorkflowStep.StepArName,
                                                 WorkflowActionId = a.WorkflowActionId,
                                                 WorkflowActionName = a.WorkflowAction.ActionArName,
                                                 Description = a.Description,
                                                 NextStatusId = a.NextStatusId,
                                                 NextStatusName = a.NextStatus.StatusArName,
                                                 NextStepId = a.NextStepId,
                                                 NextStepName = a.NextStep.StepArName,
                                                 IsNoteRequired = a.IsNoteRequired,
                                                 IsNoteVisible = a.IsNoteVisible,
                                                 CreatedOn = a.CreatedOn
                                             }).ToListAsync();

            return workflowStepActions;
        }

        #endregion

        #region WorkflowStep

        public async Task<WorkflowStepDto> GetWorkflowStepAsync(Guid id)
        {
            var entity = await db.WorkflowSteps.FindAsync(id);

            return mapper.Map<WorkflowStepDto>(entity);
        }

        public async Task<IList<WorkflowStepListItemDto>> GetWorkflowStepsAsync()
        {
            var entities = await db.WorkflowSteps
               .Include(m => m.WorkflowStepCategory)
               .Include(m => m.WorkflowType)
               .ToListAsync();

            return mapper.Map<IList<WorkflowStepListItemDto>>(entities);
        }

        public async Task<IList<WorkflowStepListItemDto>> GetWorkflowStepsAsync(Guid workflowTypeId)
        {
            if (workflowTypeId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowTypeId));
            }

            var entities = await db.WorkflowSteps
                .Include(m => m.WorkflowStepCategory)
                .Include(m => m.WorkflowType)
                .Where(m => m.WorkflowTypeId == workflowTypeId)
                .ToListAsync();

            return mapper.Map<IList<WorkflowStepListItemDto>>(entities);
        }

        #endregion

        #region WorkflowStepsCategories

        public async Task<IList<WorkflowStepCategoryDto>> GetWorkflowStepsCategoriesAsync()
        {
            var entities = await db.WorkflowStepsCategories.ToListAsync();
            return mapper.Map<IList<WorkflowStepCategoryDto>>(entities);
        }

        #endregion

        #region WorkflowTypes

        public async Task<IList<WorkflowTypeDto>> GetWorkflowTypesAsync()
        {
            var entities = await db.WorkflowTypes.ToListAsync();
            return mapper.Map<IList<WorkflowTypeDto>>(entities);
        }

        public async Task<WorkflowTypeViewDto> GetWorkflowTypeAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            //var workflowType = await (from a in commandDb.WorkflowTypes
            //                          join b in commandDb.WorkflowActions on a.Id equals b.WorkflowTypeId into bb
            //                          from ab in bb.DefaultIfEmpty()
            //                          join c in commandDb.WorkflowSteps.Include(m => m.WorkflowStepCategory) on a.Id equals c.WorkflowTypeId into cc
            //                          from ac in cc.DefaultIfEmpty()
            //                          where a.Id == id
            //                          select new WorkflowTypeViewDto
            //                          {
            //                              WorkflowType = mapper.Map<WorkflowTypeListItemDto>(a),
            //                              WorkflowActions = mapper.Map<IList<WorkflowActionListItemDto>>(a.WorkflowActions),
            //                              WorkflowSteps = mapper.Map<IList<WorkflowStepListItemDto>>(a.WorkflowSteps)
            //                          }).FirstOrDefaultAsync();

            //var workflowType = await (from a in commandDb.Set<WorkflowType>()
            //                          from b in commandDb.Set<WorkflowStep>().Include(b => b.WorkflowStepCategory).Where(b => a.Id == b.WorkflowTypeId).DefaultIfEmpty()
            //                          where a.Id == id
            //                          select new WorkflowTypeViewDto
            //                          {
            //                              WorkflowType = mapper.Map<WorkflowTypeListItemDto>(a),
            //                              WorkflowSteps = mapper.Map<IList<WorkflowStepListItemDto>>(b)
            //                          }).FirstOrDefaultAsync();

            var workflowType = await db.WorkflowTypes.FindAsync(id);

            var workflowActions = await db.WorkflowActions
                .Where(m => m.WorkflowTypeId == id)
                .ToListAsync();

            var workflowSteps = await db.WorkflowSteps
                .Include(m => m.WorkflowStepCategory)
                .Where(m => m.WorkflowTypeId == id)
                .ToListAsync();

            var workflowStepActions = await db.WorkflowStepsActions
                .Include(m => m.WorkflowAction)
                .Include(m => m.WorkflowStep)
                .Include(m => m.NextStep)
                .Include(m => m.NextStatus)
                .Where(m => m.WorkflowStep.WorkflowType.Id == id)
                .ToListAsync();

            var workflowTypeView = new WorkflowTypeViewDto
            {
                WorkflowType = mapper.Map<WorkflowTypeListItemDto>(workflowType),
                WorkflowActions = mapper.Map<IList<WorkflowActionListItemDto>>(workflowActions),
                WorkflowSteps = mapper.Map<IList<WorkflowStepListItemDto>>(workflowSteps),
                WorkflowStepsActions = mapper.Map<IList<WorkflowStepActionListItemDto>>(workflowStepActions)
            };

            return workflowTypeView;
        }

        #endregion
    }
}
