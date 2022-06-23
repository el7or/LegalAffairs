using AutoMapper;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;

namespace Moe.La.Core.Mapping
{
    public class AttachmentMappingProfile : Profile
    {
        public AttachmentMappingProfile()
        {
            CreateMap<Attachment, AttachmentListItemDto>()
             .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(a => DateTimeHelper.GetHigriDate(a.CreatedOn)))
             .ForMember(res => res.AttachmentType, opt => opt.MapFrom(a => a.AttachmentType.Name));
            ///

            //CreateMap<AttachmentDto, Attachment>()
            //   .ForMember(c => c.Id, opt => opt.Ignore())
            //   .ReverseMap();

            CreateMap<AttachmentType, AttachmentTypeListItemDto>()
                .ForMember(res => res.GroupName, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.GroupName)))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(a => DateTimeHelper.GetHigriDate(a.CreatedOn)));

            CreateMap<CaseAttachment, AttachmentListItemDto>()
            .ForMember(res => res.Name, opt => opt.MapFrom(a => a.Attachment.Name))
            .ForMember(res => res.Size, opt => opt.MapFrom(a => a.Attachment.Size))
            .ForMember(res => res.AttachmentTypeId, opt => opt.MapFrom(a => a.Attachment.AttachmentTypeId))
            .ForMember(res => res.AttachmentType, opt => opt.MapFrom(a => a.Attachment.AttachmentType.Name))
            .ForMember(res => res.CreatedOn, opt => opt.MapFrom(a => a.Attachment.CreatedOn))
            .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(a => DateTimeHelper.GetHigriDate(a.Attachment.CreatedOn)));

            CreateMap<CaseRuleAttachment, AttachmentListItemDto>()
            .ForMember(res => res.Name, opt => opt.MapFrom(a => a.Attachment.Name))
            .ForMember(res => res.Size, opt => opt.MapFrom(a => a.Attachment.Size))
            .ForMember(res => res.AttachmentTypeId, opt => opt.MapFrom(a => a.Attachment.AttachmentTypeId))
            .ForMember(res => res.AttachmentType, opt => opt.MapFrom(a => a.Attachment.AttachmentType.Name))
            .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(a => DateTimeHelper.GetHigriDate(a.Attachment.CreatedOn)));

            CreateMap<HearingAttachment, AttachmentListItemDto>()
            .ForMember(res => res.Name, opt => opt.MapFrom(a => a.Attachment.Name))
            .ForMember(res => res.Size, opt => opt.MapFrom(a => a.Attachment.Size))
            .ForMember(res => res.AttachmentTypeId, opt => opt.MapFrom(a => a.Attachment.AttachmentTypeId))
            .ForMember(res => res.AttachmentType, opt => opt.MapFrom(a => a.Attachment.AttachmentType.Name))
            .ForMember(res => res.CreatedOn, opt => opt.MapFrom(a => a.Attachment.CreatedOn))
            .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(a => DateTimeHelper.GetHigriDate(a.Attachment.CreatedOn)));

            CreateMap<HearingUpdateAttachment, AttachmentListItemDto>()
            .ForMember(res => res.Name, opt => opt.MapFrom(a => a.Attachment.Name))
            .ForMember(res => res.Size, opt => opt.MapFrom(a => a.Attachment.Size))
            .ForMember(res => res.AttachmentTypeId, opt => opt.MapFrom(a => a.Attachment.AttachmentTypeId))
            .ForMember(res => res.AttachmentType, opt => opt.MapFrom(a => a.Attachment.AttachmentType.Name))
            .ForMember(res => res.CreatedOn, opt => opt.MapFrom(a => a.Attachment.CreatedOn))
            .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(a => DateTimeHelper.GetHigriDate(a.Attachment.CreatedOn)));

            CreateMap<InvestigationRecordAttachment, AttachmentListItemDto>()
            .ForMember(res => res.Name, opt => opt.MapFrom(a => a.Attachment.Name))
            .ForMember(res => res.Size, opt => opt.MapFrom(a => a.Attachment.Size))
            .ForMember(res => res.AttachmentTypeId, opt => opt.MapFrom(a => a.Attachment.AttachmentTypeId))
            .ForMember(res => res.AttachmentType, opt => opt.MapFrom(a => a.Attachment.AttachmentType.Name))
            .ForMember(res => res.CreatedOn, opt => opt.MapFrom(a => a.Attachment.CreatedOn))
            .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(a => DateTimeHelper.GetHigriDate(a.Attachment.CreatedOn)));

            CreateMap<MoamalaAttachment, AttachmentListItemDto>()
            .ForMember(res => res.Name, opt => opt.MapFrom(a => a.Attachment.Name))
            .ForMember(res => res.Size, opt => opt.MapFrom(a => a.Attachment.Size))
            .ForMember(res => res.AttachmentTypeId, opt => opt.MapFrom(a => a.Attachment.AttachmentTypeId))
            .ForMember(res => res.AttachmentType, opt => opt.MapFrom(a => a.Attachment.AttachmentType.Name))
            .ForMember(res => res.CreatedOn, opt => opt.MapFrom(a => a.Attachment.CreatedOn))
            .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(a => DateTimeHelper.GetHigriDate(a.Attachment.CreatedOn)));

        }
    }
}
