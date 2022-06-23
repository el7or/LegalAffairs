using AutoMapper;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Linq;

namespace Moe.La.Core.Mapping
{
    public class ConsultationMappingProfile : Profile
    {
        public ConsultationMappingProfile()
        {
            CreateMap<ConsultationGrounds, ConsultationGroundsDto>();

            CreateMap<ConsultationMerits, ConsultationMeritsDto>();

            CreateMap<ConsultationDto, Consultation>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.ConsultationGrounds, opt => opt.Ignore())
                .ForMember(c => c.ConsultationMerits, opt => opt.Ignore())
                .ForMember(c => c.ConsultationVisuals, opt => opt.Ignore())
                .AfterMap((consultationRes, consultationEntity) =>
                {
                    if (consultationRes.ConsultationGrounds.Count > 0)
                    {
                        foreach (var consultationGrounds in consultationRes.ConsultationGrounds)
                            if (!string.IsNullOrEmpty(consultationGrounds.Text))
                                consultationEntity.ConsultationGrounds.Add(new ConsultationGrounds
                                {
                                    Text = consultationGrounds.Text,
                                    ConsultationId = consultationEntity.Id
                                });
                    }

                    if (consultationRes.ConsultationMerits.Count > 0)
                    {
                        foreach (var consultationMerits in consultationRes.ConsultationMerits)
                            if (!string.IsNullOrEmpty(consultationMerits.Text))
                                consultationEntity.ConsultationMerits.Add(new ConsultationMerits
                                {
                                    Text = consultationMerits.Text,
                                    ConsultationId = consultationEntity.Id
                                });
                    }

                    if (consultationRes.IsWithNote == true)
                    {
                        foreach (var consultationVisual in consultationRes.ConsultationVisuals)
                            if (!string.IsNullOrEmpty(consultationVisual.Material))
                                consultationEntity.ConsultationVisuals.Add(new ConsultationVisual
                                {
                                    ConsultationId = consultationEntity.Id,
                                    Material = consultationVisual.Material,
                                    Visuals = consultationVisual.Visuals
                                });
                    }
                })
                .ReverseMap();


            CreateMap<Consultation, ConsultationDetailsDto>()
                .ForMember(res => res.MoamalaId, opt => opt.MapFrom(c => c.ConsultationMoamalat.Where(s => s.CreatedOn.Date == c.CreatedOn.Date).Select(r => r.MoamalaId).FirstOrDefault()))
                .ForMember(res => res.Status, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.Status, Name = EnumExtensions.GetDescription(c.Status) }))
                .ForMember(res => res.WorkItemType, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.WorkItemType.Id, Name = c.WorkItemType.Name }))
                .ForMember(res => res.SubWorkItemType, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.SubWorkItemType.Id, Name = c.SubWorkItemType.Name }))
                .ForMember(res => res.Branch, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.Branch.Id, Name = c.Branch.Name }))
                .ForMember(res => res.Department, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.Department.Id, Name = c.Department.Name }))
                .ForMember(c => c.ConsultationDateHijri, opt => opt.MapFrom(m => m.ConsultationDate.HasValue ? DateTimeHelper.GetHigriDate(m.ConsultationDate) : null));

            CreateMap<ConsultationMoamalat, ConsultationListItemDto>()
                .ForMember(c => c.MoamalaNumber, opt => opt.MapFrom(m => m.Moamala.MoamalaNumber))
                .ForMember(c => c.MoamalaDate, opt => opt.MapFrom(m => m.Moamala.CreatedOn))
                .ForMember(c => c.MoamalaDateOnHijri, opt => opt.MapFrom(m => DateTimeHelper.GetHigriDate(m.Moamala.CreatedOn)))
                .ForMember(c => c.Department, opt => opt.MapFrom(m => new KeyValuePairsDto<int> { Id = m.Moamala.ReceiverDepartmentId.Value, Name = m.Moamala.ReceiverDepartment.Name }))
                .ForMember(c => c.Subject, opt => opt.MapFrom(m => m.Moamala.Subject))
                .ForMember(c => c.WorkItemType, opt => opt.MapFrom(m => new KeyValuePairsDto<int> { Id = m.Moamala.WorkItemType.Id, Name = m.Moamala.WorkItemType.Name }))
                .ForMember(c => c.User, opt => opt.MapFrom(m => new KeyValuePairsDto<Guid> { Id = m.Moamala.AssignedToId.Value, Name = m.Moamala.AssignedTo.FirstName + " " + m.Moamala.AssignedTo.LastName }))
                .ForMember(c => c.Status, opt => opt.MapFrom(m => new KeyValuePairsDto<int> { Id = (int)m.Consultation.Status, Name = EnumExtensions.GetDescription(m.Consultation.Status) }));



        }
    }
}
