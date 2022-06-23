using AutoMapper;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Linq;

namespace Moe.La.Core.Mapping
{
    public class HearingMappingProfile : Profile
    {
        public HearingMappingProfile()
        {
            #region Domain Models to API Dto:  
            CreateMap<Hearing, HearingListItemDto>()
                    .ForMember(res => res.CaseSupportingDocumentRequest, opt => opt.MapFrom(c => c.CaseSupportingDocumentRequests.ToList().LastOrDefault()))
                    .ForMember(res => res.Court, opt => opt.MapFrom(c => c.Court.Name))
                    .ForMember(res => res.Status, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.Status)))
                    .ForMember(res => res.Type, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.Type)))
                    .ForMember(res => res.AssignedTo, opt => opt.MapFrom(c => c.AssignedTo != null ? new KeyValuePairsDto<Guid>
                    {
                        Id = c.AssignedToId.Value,
                        Name = c.AssignedTo.FirstName + " " + c.AssignedTo.LastName
                    } : default))
                    .ForMember(res => res.HearingDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.HearingDate)))
                    .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)));

            CreateMap<Hearing, HearingDetailsDto>()
                .ForMember(res => res.HearingDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.HearingDate)))
                .ForMember(res => res.Court, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = c.CourtId, Name = c.Court.Name }))
                .ForMember(res => res.Status, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.Status, Name = EnumExtensions.GetDescription(c.Status) }))
                .ForMember(res => res.Type, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.Type, Name = EnumExtensions.GetDescription(c.Type) }))
                .ForMember(res => res.Motions, opt => opt.MapFrom(c => c.Motions.Replace("\n", "<br>")))
                .ForMember(res => res.AssignedTo, opt => opt.MapFrom(c => c.AssignedTo != null ? new KeyValuePairsDto<Guid>
                {
                    Id = c.AssignedToId.Value,
                    Name = c.AssignedTo.FirstName + " " + c.AssignedTo.LastName
                } : default))
                .ForMember(res => res.HearingLegalMemoReviewRequest, opt => opt.MapFrom(c => c.HearingLegalMemoReviewRequests.LastOrDefault()))
                .ForMember(res => res.LegalMemos, opt => opt.MapFrom(c => c.HearingLegalMemos.Select(cc => new KeyValuePairsDto<int> { Id = cc.Id, Name = cc.LegalMemo.Name })))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)));


            CreateMap<Hearing, HearingDto>()
                 .ForMember(res => res.BranchId, opt => opt.MapFrom(c => c.Case.BranchId));


            CreateMap<HearingLegalMemo, HearingLegalMemoDetailsDto>();

            CreateMap<HearingLegalMemoDto, HearingLegalMemo>();
            #endregion

            #region API Dto to Domain: 

            CreateMap<HearingDto, Hearing>()
                .ForMember(h => h.Id, opt => opt.Ignore())
                .ForMember(h => h.Attachments, opt => opt.Ignore())
                  .AfterMap((hearingDto, hearing) =>
                  {
                      var attachmentsToAdd = hearingDto.Attachments.Where(a => !a.IsDeleted).ToList();

                      var removedAttachments = hearing.Attachments.Where(a => !attachmentsToAdd.Select(d => d.Id).Contains(a.Id)).ToList();

                      foreach (var attachment in removedAttachments)
                          hearing.Attachments.Remove(attachment);

                      foreach (var attachment in attachmentsToAdd)
                          if (!hearing.Attachments.Select(a => (Guid?)a.Id).Contains(attachment.Id))
                              hearing.Attachments.Add(new HearingAttachment
                              {
                                  HearingId = hearing.Id,
                                  Id = (Guid)attachment.Id
                              });

                  });

            #endregion

            #region HearingUpdate
            CreateMap<HearingUpdate, HearingUpdateListItemDto>()
                   .ForMember(res => res.CreatedBy, opt => opt.MapFrom(c => c.CreatedByUser != null ? new KeyValuePairsDto<Guid>
                   {
                       Id = c.CreatedByUser.Id,
                       Name = c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName
                   } : default))
                   .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                   .ForMember(res => res.UpdateDateHijri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.UpdateDate)))
                   .ForMember(res => res.CreationTime, opt => opt.MapFrom(c => DateTimeHelper.GetFullTime(c.CreatedOn)))
                   .ForMember(res => res.UpdateTime, opt => opt.MapFrom(c => DateTimeHelper.GetFullTime(c.UpdateDate)))
                   .ForMember(res => res.Attachment, opt => opt.MapFrom(c => c.Attachments != null ? new KeyValuePairsDto<Guid>
                   {
                       Id = c.Attachments.FirstOrDefault().Attachment.Id,
                       Name = c.Attachments.FirstOrDefault().Attachment.Name
                   } : default))

                   .ForMember(h => h.Attachments, opt => opt.Ignore());

            CreateMap<HearingUpdateDetailsDto, HearingUpdate>()
               .ForMember(res => res.Attachments, opt => opt.MapFrom(c => c.Attachments));

            CreateMap<HearingUpdateListItemDto, HearingUpdate>()
               .ForMember(res => res.Attachments, opt => opt.MapFrom(c => c.Attachments));



            CreateMap<HearingUpdate, HearingUpdateDetailsDto>()
                .ForMember(res => res.CreatedBy, opt => opt.MapFrom(c => new KeyValuePairsDto<Guid> { Id = c.CreatedByUser.Id, Name = c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName }))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                   .ForMember(res => res.UpdateDateHijri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.UpdateDate)))
                   .ForMember(res => res.CreationTime, opt => opt.MapFrom(c => DateTimeHelper.GetFullTime(c.CreatedOn)))
                   .ForMember(res => res.UpdateTime, opt => opt.MapFrom(c => DateTimeHelper.GetFullTime(c.UpdateDate)));


            CreateMap<HearingUpdate, HearingUpdateDto>();

            CreateMap<HearingUpdateDto, HearingUpdate>()
                .ForMember(h => h.Id, opt => opt.Ignore())
                .ForMember(h => h.Attachments, opt => opt.Ignore())
                .AfterMap((hearingDto, hearing) =>
                {
                    var attachmentsToAdd = hearingDto.Attachments.Where(a => !a.IsDeleted).ToList();

                    var removedAttachments = hearing.Attachments.Where(a => !attachmentsToAdd.Select(d => d.Id).Contains(a.Id)).ToList();


                    foreach (var attachment in removedAttachments)
                        hearing.Attachments.Remove(attachment);

                    foreach (var attachment in attachmentsToAdd)
                        if (!hearing.Attachments.Select(a => (Guid?)a.Id).Contains(attachment.Id))
                            hearing.Attachments.Add(new HearingUpdateAttachment
                            {
                                HearingUpdateId = hearing.Id,
                                Id = (Guid)attachment.Id
                            });



                });
            #endregion
        }
    }
}
