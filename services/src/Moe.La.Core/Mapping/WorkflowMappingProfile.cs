using AutoMapper;
using Moe.La.Core.Dtos.Workflow;
using Moe.La.Core.Entities;

namespace Moe.La.Core.Mapping
{
    public class WorkflowMappingProfile : Profile
    {
        public WorkflowMappingProfile()
        {
            // WorkflowType.
            CreateMap<WorkflowType, WorkflowTypeDto>().ReverseMap();

            // WorkflowAction.
            CreateMap<WorkflowAction, WorkflowActionDto>().ReverseMap();
            CreateMap<WorkflowAction, WorkflowActionListItemDto>()
                .ForMember(m => m.WorkflowTypeArName, opt => opt.MapFrom(prop => prop.WorkflowType.TypeArName))
                .ReverseMap();

            // WorkflowStatus.
            CreateMap<WorkflowStatus, WorkflowStatusDto>().ReverseMap();

            // WorkflowStep.
            CreateMap<WorkflowStep, WorkflowStepDto>().ReverseMap();
            CreateMap<WorkflowStep, WorkflowStepListItemDto>()
                .ForMember(m => m.WorkflowStepCategoryArName, opt => opt.MapFrom(prop => prop.WorkflowStepCategory.CategoryArName))
                .ForMember(m => m.WorkflowTypeArName, opt => opt.MapFrom(prop => prop.WorkflowType.TypeArName))
                .ReverseMap();

            // WorkflowInstanceLog.
            CreateMap<WorkflowInstanceLog, WorkflowInstanceLogDto>().ReverseMap();

            // WorkflowInstance.
            CreateMap<WorkflowInstance, WorkflowInstanceDto>().ReverseMap();

            // WorkflowStepPermission.
            CreateMap<WorkflowStepPermission, WorkflowStepPermissionDto>().ReverseMap();

            // WorkflowStepCategory.
            CreateMap<WorkflowStepCategory, WorkflowStepCategoryDto>().ReverseMap();

            // WorkflowStepAction.
            CreateMap<WorkflowStepAction, WorkflowStepActionDto>().ReverseMap();
            CreateMap<WorkflowStepAction, WorkflowStepActionListItemDto>()
                .ForMember(m => m.WorkflowActionName, opt => opt.MapFrom(prop => prop.WorkflowAction.ActionArName))
                .ForMember(m => m.WorkflowStepName, opt => opt.MapFrom(prop => prop.WorkflowStep.StepArName))
                .ForMember(m => m.NextStepName, opt => opt.MapFrom(prop => prop.NextStep.StepArName))
                .ForMember(m => m.NextStatusName, opt => opt.MapFrom(prop => prop.NextStatus.StatusArName))
                .ReverseMap();
        }
    }
}
