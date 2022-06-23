using AutoMapper;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Linq;

namespace Moe.La.Core.Mapping
{
    class MoamalaMappingProfile : Profile
    {
        public MoamalaMappingProfile()
        {
            CreateMap<Moamala, MoamalaListItemDto>()
                .ForMember(res => res.IsRead, opt => opt.MapFrom(c => c.MoamalaNotifications.FirstOrDefault().IsRead))
                .ForMember(res => res.CreatedByFullName, opt => opt.MapFrom(c => c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName))
                .ForMember(res => res.Consultations, opt => opt.MapFrom(c => c.ConsultationMoamalat.Select(c => c.ConsultationId)))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.CreatedOnTime, opt => opt.MapFrom(c => DateTimeHelper.GetTime(c.CreatedOn)))
                .ForMember(res => res.PassDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.PassDate)))
                .ForMember(res => res.PassTime, opt => opt.MapFrom(c => DateTimeHelper.GetTime(c.PassDate)))
                .ForMember(res => res.AssignedTo, opt => opt.MapFrom(c => new KeyValuePairsDto<Guid> { Id = c.AssignedTo.Id, Name = c.AssignedTo.FirstName + " " + c.AssignedTo.LastName }))
                .ForMember(res => res.ConfidentialDegree, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.ConfidentialDegree, Name = EnumExtensions.GetDescription(c.ConfidentialDegree) }))
                .ForMember(res => res.Status, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.Status, Name = EnumExtensions.GetDescription(c.Status) }))
                .ForMember(res => res.Branch, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.Branch.Id, Name = c.Branch.Name }))
                .ForMember(res => res.ReceiverDepartment, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.ReceiverDepartment.Id, Name = c.ReceiverDepartment.Name }))
                .ForMember(res => res.SenderDepartment, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.SenderDepartment.Id, Name = c.SenderDepartment.Name }))
                .ForMember(res => res.WorkItemType, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.WorkItemType.Id, Name = c.WorkItemType.Name }))
                .ForMember(res => res.SubWorkItemType, opt => opt.MapFrom(c => new KeyValuePairsDto<int?> { Id = c.SubWorkItemTypeId, Name = c.SubWorkItemType == null ? null : c.SubWorkItemType.Name }));

            CreateMap<MoamalaDto, Moamala>()
                .ForMember(res => res.Id, opt => opt.Ignore())
                .ForMember(res => res.Attachments, opt => opt.Ignore())
                .AfterMap((moamalaDto, moamala) =>
                {
                    if (moamalaDto.Attachments != null && moamalaDto.Attachments.Count > 0)
                    {
                        var attachmentsToAdd = moamalaDto.Attachments.Where(a => !a.IsDeleted).ToList();

                        var removedAttachments = moamala.Attachments.Where(a => !attachmentsToAdd.Select(d => d.Id).Contains(a.Id)).ToList();


                        foreach (var attachment in removedAttachments)
                            moamala.Attachments.Remove(attachment);

                        foreach (var attachment in attachmentsToAdd)
                            if (!moamala.Attachments.Select(a => (Guid?)a.Id).Contains(attachment.Id))
                                moamala.Attachments.Add(new MoamalaAttachment
                                {
                                    MoamalaId = moamala.Id,
                                    Id = (Guid)attachment.Id
                                });
                    }

                });

            CreateMap<MoamalaRaselDto, MoamalaRasel>()
                .ForMember(res => res.Id, opt => opt.Ignore());
            //.ForMember(res => res.Attachments, opt => opt.Ignore());

            CreateMap<MoamalaRasel, MoamalaRaselDto>();

            CreateMap<MoamalaRasel, MoamalaRaselListItemDto>()
            .ForMember(res => res.HijriCreatedOn, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.CreatedOnTime, opt => opt.MapFrom(c => DateTimeHelper.GetTime(c.GregorianCreatedDate)))
                .ForMember(res => res.ItemPrivacy, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.ItemPrivacy, Name = EnumExtensions.GetDescription(c.ItemPrivacy) }))
                .ForMember(res => res.Status, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.Status, Name = EnumExtensions.GetDescription(c.Status) }));


            CreateMap<Moamala, MoamalaDetailsDto>()
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.CreatedOnTime, opt => opt.MapFrom(c => DateTimeHelper.GetTime(c.CreatedOn)))
                .ForMember(res => res.CreatedBy, opt => opt.MapFrom(c => c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName))
                .ForMember(res => res.PassDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.PassDate)))
                .ForMember(res => res.PassTime, opt => opt.MapFrom(c => DateTimeHelper.GetTime(c.PassDate)))
                .ForMember(res => res.ConfidentialDegree, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.ConfidentialDegree, Name = EnumExtensions.GetDescription(c.ConfidentialDegree) }))
                .ForMember(res => res.PassType, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.PassType, Name = EnumExtensions.GetDescription(c.PassType) }))
                .ForMember(res => res.Status, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.Status, Name = EnumExtensions.GetDescription(c.Status) }))
                .ForMember(res => res.SenderDepartment, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.SenderDepartment.Id, Name = c.SenderDepartment.Name }))
                .ForMember(res => res.Branch, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.Branch.Id, Name = c.Branch.Name }))
                .ForMember(res => res.ReceiverDepartment, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.ReceiverDepartment.Id, Name = c.ReceiverDepartment.Name }))
                .ForMember(res => res.AssignedToFullName, opt => opt.MapFrom(c => c.AssignedTo.FirstName + " " + c.AssignedTo.LastName))
                .ForMember(res => res.WorkItemType, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.WorkItemType.Id, Name = c.WorkItemType.Name }))
                .ForMember(res => res.SubWorkItemType, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.SubWorkItemType.Id, Name = c.SubWorkItemType.Name }))
                .AfterMap((moamalaEntity, moamalaDto) =>
                {
                    if (moamalaEntity.ConsultationMoamalat.Count > 0)
                    {
                        moamalaDto.ConsultationId = moamalaEntity.ConsultationMoamalat.FirstOrDefault().ConsultationId;
                        moamalaDto.ConsultationStatus = moamalaEntity.ConsultationMoamalat.FirstOrDefault().Consultation.Status;
                    }
                });


            CreateMap<MoamalaAttachment, AttachmentListItemDto>()
                  .ForMember(res => res.Name, opt => opt.MapFrom(c => c.Attachment.Name))
                  .ForMember(res => res.Size, opt => opt.MapFrom(c => c.Attachment.Size))
                  .ForMember(res => res.Id, opt => opt.MapFrom(c => c.Attachment.Id))
                  .ForMember(res => res.CreatedOn, opt => opt.MapFrom(c => c.Attachment.CreatedOn))
                  .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.Attachment.CreatedOn)))
                  .ForMember(res => res.IsDraft, opt => opt.MapFrom(c => c.Attachment.IsDraft))
                  .ForMember(res => res.AttachmentType, opt => opt.MapFrom(c => c.Attachment.AttachmentType.Name));

            CreateMap<MoamalaTransaction, MoamalaTransactionDetailsDto>()
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.TransactionType, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.TransactionType)))
                .ForMember(res => res.CreatedByUser, opt => opt.MapFrom(c => c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName));
            //.ForMember(res => res.CreatedByRole, opt => opt.MapFrom(c => c.CreatedByUser.UserRoles.FirstOrDefault().Role.NameAr));

            CreateMap<MoamalaTransactionDto, MoamalaTransaction>()
              .ForMember(c => c.Id, opt => opt.Ignore())
              .ReverseMap();
        }
    }
}
