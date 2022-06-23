using AutoMapper;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Entities.Litigation.BoardMeeting;
using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moe.La.Core.Mapping
{
    public class LegalBoardMappingProfile : Profile
    {
        public LegalBoardMappingProfile()
        {

            #region Domain Models to API Dto: 

            CreateMap<LegalBoard, LegalBoardListItemDto>()
                .ForMember(res => res.Status, opt => opt.MapFrom(l => new KeyValuePairsDto<int>
                {
                    Id = (int)l.StatusId,
                    Name = EnumExtensions.GetDescription(l.StatusId)
                }))
                .ForMember(res => res.Type, opt => opt.MapFrom(a => EnumExtensions.GetDescription(a.LegalBoardTypeId)))
                .ForMember(res => res.BoardHead, opt => opt.MapFrom(
                       a => a.LegalBoardMembers.Where(m => m.MembershipType == LegalBoardMembershipTypes.Head).Select(m => m.User.FirstName + " " + m.User.LastName).FirstOrDefault()))
                .ForMember(res => res.UpdatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.UpdatedOn)))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)));

            CreateMap<LegalBoard, LegalBoardDetailsDto>()
                .ForMember(res => res.Status, opt => opt.MapFrom(a => EnumExtensions.GetDescription(a.StatusId)))
                .ForMember(res => res.Type, opt => opt.MapFrom(a => EnumExtensions.GetDescription(a.LegalBoardTypeId)))
                .ForMember(res => res.LegalBoardMembers, opt => opt.MapFrom(
                       a => a.LegalBoardMembers.Select(m => m).ToList()));

            CreateMap<LegalBoardMemo, LegalBoardMemoDto>();

            CreateMap<LegalBoardMember, LegalBoardMemberDetailsDto>()
                .ForMember(res => res.StartDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.StartDate)))
                .ForMember(res => res.UserName, opt => opt.MapFrom(m => m.User.FirstName + ' ' + m.User.LastName))
                .ForMember(res => res.MembershipType, opt => opt.MapFrom(m => new KeyValuePairsDto<int> { Id = (int)m.MembershipType, Name = EnumExtensions.GetDescription(m.MembershipType) }));

            CreateMap<LegalBoardMemberHistory, LegalBoardMemberHistoryDto>()
                .ForMember(res => res.MembershipType, opt => opt.MapFrom(h => EnumExtensions.GetDescription(h.MembershipType)))
                .ForMember(res => res.StartDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.StartDate)))
                .ForMember(res => res.EndDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.EndDate)));

            CreateMap<LegalBoard, LegalBoardDto>();

            CreateMap<LegalBoardMember, LegalBoradMemberDto>()
                //.ForMember(l => l.MemberId, opt => opt.MapFrom(m => m.UserId))
                //.ForMember(l => l.MembershipType, opt => opt.MapFrom(m => m.MembershipType))
                .ForMember(l => l.Id, opt => opt.MapFrom(m => m.Id))
                .ReverseMap();

            CreateMap<LegalBoardMember, LegalBoardMemberHistory>()
                .ForMember(res => res.LegalBoardMemberId, opt => opt.MapFrom(res => res.Id));
            //CreateMap<LegalBoardQueryObjectDto, LegalBoardQueryObject>();

            CreateMap<LegalBoradMemberDto, LegalBoardMemberHistory>()
                .ForMember(res => res.Id, opt => opt.Ignore())
                .ForMember(res => res.LegalBoardMemberId, opt => opt.MapFrom(res => res.Id))
                .ForMember(res => res.StartDate, opt => opt.MapFrom(res => DateTime.Now))
                .ForMember(res => res.IsActive, opt => opt.MapFrom(res => true))
                .ForMember(res => res.CreatedOn, opt => opt.MapFrom(res => DateTime.Now));

            CreateMap<BoardMeeting, BoardMeetingDetailsDto>()
                .ForMember(res => res.MeetingDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.MeetingDate)))
                .ForMember(res => res.Board, opt => opt.MapFrom(c => c.Board.Name))
                .ForMember(res => res.LegalBoardType, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.Board.LegalBoardTypeId)))
                .ForMember(res => res.BoardMeetingMembersIds, opt => opt.MapFrom(c => c.BoardMeetingMembers.Select(
                    cc => cc.BoardMemberId)));

            CreateMap<BoardMeeting, BoardMeetingListItemDto>()
                .ForMember(res => res.MeetingDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.MeetingDate)))
                .ForMember(res => res.Memo, opt => opt.MapFrom(c => c.Memo.Name))
                .ForMember(res => res.Board, opt => opt.MapFrom(c => c.Board.Name));

            #endregion

            #region API Dto to Domain:

            CreateMap<LegalBoardDto, LegalBoard>()
                 .ForMember(l => l.Id, opt => opt.Ignore())
                 .ForMember(l => l.LegalBoardMembers, opt => opt.Ignore())
                 .ForMember(l => l.LegalBoardTypeId, opt => opt.MapFrom(l => l.TypeId))
                 .AfterMap((BoardDto, Board) =>
                 {
                     foreach (var member in BoardDto.LegalBoardMembers)
                         if (Board.LegalBoardMembers.Select(m => m.UserId).Contains(member.UserId))
                         {
                             Board.LegalBoardMembers.Where(m => m.Id == member.Id).FirstOrDefault().IsActive = member.IsActive;
                             Board.LegalBoardMembers.Where(m => m.Id == member.Id).FirstOrDefault().MembershipType = member.MembershipType;
                             Board.LegalBoardMembers.Where(m => m.Id == member.Id).FirstOrDefault().UpdatedOn = DateTime.Now;
                         }
                         else
                         {
                             Board.LegalBoardMembers.Add(new LegalBoardMember
                             {
                                 UserId = member.UserId,
                                 LegalBoardId = Board.Id,
                                 StartDate = DateTime.Now,
                                 MembershipType = member.MembershipType,
                                 EndDate = null,
                                 IsActive = member.IsActive,
                                 CreatedOn = DateTime.Now
                             });
                         }
                 });

            CreateMap<LegalBoardMemoDto, LegalBoardMemo>()
                .ForMember(res => res.Id, opt => opt.Ignore());

            CreateMap<BoardMeetingDto, BoardMeeting>()
                 .ForMember(res => res.Id, opt => opt.Ignore())
                 .ForMember(res => res.BoardMeetingMembers, opt => opt.Ignore())
                 .AfterMap((boardMeetingRes, boardMeetingEntity) =>
                 {
                     // remove members not available in boardMeetingRes
                     var removedMembers = new List<BoardMeetingMember>();
                     foreach (var member in boardMeetingEntity.BoardMeetingMembers)
                         if (!boardMeetingRes.BoardMeetingMembers.Contains(member.Id))
                             removedMembers.Add(member);

                     foreach (var member in removedMembers)
                         boardMeetingEntity.BoardMeetingMembers.Remove(member);

                     // add the new members
                     foreach (var member in boardMeetingRes.BoardMeetingMembers)
                     {
                         // check if not the member already exists
                         if (!boardMeetingEntity.BoardMeetingMembers.Select(n => n.Id).Contains(member))
                         {
                             boardMeetingEntity.BoardMeetingMembers.Add(
                             new BoardMeetingMember
                             {
                                 BoardMeetingId = boardMeetingEntity.Id,
                                 BoardMemberId = member,
                             });
                         }
                     };
                 });

            CreateMap<LegalBoardMember, LegalBoradMemberDto>()
                .ForMember(l => l.UserName, opt => opt.MapFrom(m => m.User.FirstName + " " + m.User.LastName))
                .ForMember(l => l.MembershipType, opt => opt.MapFrom(m => (int)m.MembershipType))
                .ReverseMap();

            #endregion
        }
    }
}
