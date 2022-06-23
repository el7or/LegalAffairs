using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Dtos.Workflow;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    /// <summary>
    /// Workflow configuration repository.
    /// </summary>
    public class WorkflowConfigurationRepository : RepositoryBase, IWorkflowConfigurationRepository
    {
        public WorkflowConfigurationRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        /// <summary>
        /// Add workflow action.
        /// </summary>
        /// <param name="workflowAction"><see cref="WorkflowActionDto"/> object.</param>
        /// <returns></returns>
        public async Task AddWorkflowActionAsync(WorkflowActionDto workflowAction)
        {
            if (workflowAction is null)
            {
                throw new ArgumentNullException(nameof(workflowAction));
            }

            workflowAction.Id = Guid.NewGuid();
            workflowAction.CreatedOn = DateTime.Now;

            var entityToAdd = mapper.Map<WorkflowAction>(workflowAction);
            await db.WorkflowActions.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
            mapper.Map(entityToAdd, workflowAction);
        }

        /// <summary>
        /// Edit workflow action.
        /// </summary>
        /// <param name="workflowAction"><see cref="WorkflowActionDto"/> object.</param>
        /// <returns></returns>
        public async Task EditWorkflowActionAsync(WorkflowActionDto workflowAction)
        {
            if (workflowAction is null)
            {
                throw new ArgumentNullException(nameof(workflowAction));
            }

            var entityToUpdate = await db.WorkflowActions.FindAsync(workflowAction.Id);
            mapper.Map(workflowAction, entityToUpdate);
            db.WorkflowActions.Update(entityToUpdate);
            await db.SaveChangesAsync();
            mapper.Map(entityToUpdate, workflowAction);
        }

        /// <summary>
        /// Add workflow step action.
        /// </summary>
        /// <param name="workflowStepAction"><see cref="WorkflowStepActionDto"/> object.</param>
        /// <returns></returns>
        public async Task AddWorkflowStepActionAsync(WorkflowStepActionDto workflowStepAction)
        {
            if (workflowStepAction is null)
            {
                throw new ArgumentNullException(nameof(workflowStepAction));
            }

            workflowStepAction.Id = Guid.NewGuid();
            workflowStepAction.CreatedOn = DateTime.Now;

            var entityToAdd = mapper.Map<WorkflowStepAction>(workflowStepAction);
            await db.WorkflowStepsActions.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
            mapper.Map(entityToAdd, workflowStepAction);
        }

        /// <summary>
        /// Edit workflow step action.
        /// </summary>
        /// <param name="workflowStepAction"><see cref="WorkflowStepActionDto"/> object.</param>
        /// <returns></returns>
        public async Task EditWorkflowStepActionAsync(WorkflowStepActionDto workflowStepAction)
        {
            if (workflowStepAction is null)
            {
                throw new ArgumentNullException(nameof(workflowStepAction));
            }

            var entityToUpdate = await db.WorkflowStepsActions.FindAsync(workflowStepAction.Id);
            mapper.Map(workflowStepAction, entityToUpdate);
            db.WorkflowStepsActions.Update(entityToUpdate);
            await db.SaveChangesAsync();
            mapper.Map(entityToUpdate, workflowStepAction);
        }

        /// <summary>
        /// Add workflow step.
        /// </summary>
        /// <param name="workflowStep"><see cref="WorkflowStepDto"/> object.</param>
        /// <returns></returns>
        public async Task AddWorkflowStepAsync(WorkflowStepDto workflowStep)
        {
            if (workflowStep is null)
            {
                throw new ArgumentNullException(nameof(workflowStep));
            }

            workflowStep.Id = Guid.NewGuid();
            workflowStep.CreatedOn = DateTime.Now;

            var entityToAdd = mapper.Map<WorkflowStep>(workflowStep);
            await db.WorkflowSteps.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
            mapper.Map(entityToAdd, workflowStep);
        }

        /// <summary>
        /// Update workflow step.
        /// </summary>
        /// <param name="workflowStep"><see cref="WorkflowStepDto"/> object.</param>
        /// <returns></returns>
        public async Task EditWorkflowStepAsync(WorkflowStepDto workflowStep)
        {
            if (workflowStep is null)
            {
                throw new ArgumentNullException(nameof(workflowStep));
            }

            var entityToUpdate = await db.WorkflowSteps.FindAsync(workflowStep.Id);
            mapper.Map(workflowStep, entityToUpdate);
            db.WorkflowSteps.Update(entityToUpdate);
            await db.SaveChangesAsync();
            mapper.Map(entityToUpdate, workflowStep);
        }

        /// <summary>
        /// Add workflow step persmission.
        /// </summary>
        /// <param name="workflowStepPermission"><see cref="WorkflowStepPermissionDto"/> object.</param>
        /// <returns></returns>
        public async Task AddWorkflowStepPermissionAsync(WorkflowStepPermissionDto workflowStepPermission)
        {
            if (workflowStepPermission is null)
            {
                throw new ArgumentNullException(nameof(workflowStepPermission));
            }

            workflowStepPermission.Id = Guid.NewGuid();
            workflowStepPermission.CreatedOn = DateTime.Now;

            var entityToAdd = mapper.Map<WorkflowStepPermission>(workflowStepPermission);
            await db.WorkflowStepsPermissions.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
            mapper.Map(entityToAdd, workflowStepPermission);
        }

        /// <summary>
        /// Add workflow type.
        /// </summary>
        /// <param name="workflowType"><see cref="WorkflowTypeDto"/> object.</param>
        /// <returns></returns>
        public async Task AddWorkflowTypeAsync(WorkflowTypeDto workflowType)
        {
            if (workflowType is null)
            {
                throw new ArgumentNullException(nameof(workflowType));
            }

            workflowType.Id = Guid.NewGuid();
            workflowType.CreatedOn = DateTime.Now;

            var entityToAdd = mapper.Map<WorkflowType>(workflowType);
            await db.WorkflowTypes.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
            mapper.Map(entityToAdd, workflowType);
        }

        // <summary>
        /// Update workflow type.
        /// </summary>
        /// <param name="workflowType"><see cref="WorkflowTypeDto"/> object.</param>
        /// <returns></returns>
        public async Task EditWorkflowTypeAsync(WorkflowTypeDto workflowType)
        {
            if (workflowType is null)
            {
                throw new ArgumentNullException(nameof(workflowType));
            }

            var entityToUpdate = await db.WorkflowTypes.FindAsync(workflowType.Id);
            mapper.Map(workflowType, entityToUpdate);
            db.WorkflowTypes.Update(entityToUpdate);
            await db.SaveChangesAsync();
            mapper.Map(entityToUpdate, workflowType);
        }

        /// <summary>
        /// Get the workflow initiator workflow step.
        /// </summary>
        /// <param name="workflowTypeId"><see cref="WorkflowType"/> id.</param>
        /// <returns></returns>
        public async Task<WorkflowStepDto> GetWorkflowInitiatorAsync(Guid workflowTypeId)
        {
            if (workflowTypeId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowTypeId));
            }

            var entity = await db.WorkflowSteps
                .Where(m => m.WorkflowTypeId == workflowTypeId && m.WorkflowStepCategoryId == (byte)WorkflowStepsCategories.Initiator)
                .FirstOrDefaultAsync();

            return mapper.Map<WorkflowStepDto>(entity);
        }

        /// <summary>
        /// Get the workflow step action.
        /// </summary>
        /// <param name="workflowStepId">Workflow step id.</param>
        /// <param name="workflowActionId">Workflow action id.</param>
        /// <returns></returns>
        public async Task<WorkflowStepActionDto> GetWorkflowStepActionAsync(Guid workflowStepId, Guid workflowActionId)
        {
            if (workflowStepId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowStepId));
            }

            if (workflowActionId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(workflowActionId));
            }

            var entity = await db.WorkflowStepsActions
                .Include(m => m.NextStep)
                .Include(m => m.NextStatus)
                .Where(m => m.WorkflowStepId == workflowStepId && m.WorkflowActionId == workflowActionId)
                .FirstOrDefaultAsync();

            return mapper.Map<WorkflowStepActionDto>(entity);
        }
    }
}
