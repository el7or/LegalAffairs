using AutoMapper;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Common;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using System;
using System.Linq;

namespace Moe.La.Core.Mapping
{
    class LegalMemoMappingProfile : Profile
    {
        public LegalMemoMappingProfile()
        {
            #region LegalMemo

            CreateMap<LegalMemo, LegalMemoListItemDto>()
                .ForMember(res => res.CreatedByUser, opt => opt.MapFrom(c => new KeyValuePairsDto<Guid>
                {
                    Id = c.CreatedByUser.Id,
                    Name = c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName
                }))
                .ForMember(res => res.ChangedUser, opt => opt.MapFrom(l => new KeyValuePairsDto<Guid?>
                {
                    Id = l.InitialCase.Researchers.Where(c => !c.IsDeleted).Select(c => c.Researcher).FirstOrDefault().Id,
                    Name = l.InitialCase.Researchers.Where(c => !c.IsDeleted).Select(c => c.Researcher).FirstOrDefault().FirstName + " " + l.InitialCase.Researchers.Where(c => !c.IsDeleted).Select(c => c.Researcher).FirstOrDefault().LastName
                }))

                .ForMember(res => res.Status, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.Status, Name = EnumExtensions.GetDescription(c.Status) }))
                .ForMember(res => res.Type, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.Type, Name = EnumExtensions.GetDescription(c.Type) }))
                //.ForMember(res => res.Status, opt => opt.MapFrom(l => l.StatusId.ToString()))
                .ForMember(res => res.SecondSubCategory, opt => opt.MapFrom(l => l.SecondSubCategory.Name))
                .ForMember(res => res.UpdatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.UpdatedOn)))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.RaisedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.RaisedOn)))
                .ForMember(res => res.CreationTime, opt => opt.MapFrom(c => DateTimeHelper.GetFullTime(c.CreatedOn)))
                .ForMember(res => res.UpdateTime, opt => opt.MapFrom(c => DateTimeHelper.GetFullTime(c.UpdatedOn)));

            CreateMap<LegalMemo, LegalMemoDetailsDto>()
                .ForMember(res => res.CreatedByUser, opt => opt.MapFrom(c => new KeyValuePairsDto<Guid>
                {
                    Id = c.CreatedByUser.Id,
                    Name = c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName
                }))
                .ForMember(res => res.ChangedUser, opt => opt.MapFrom(l => new KeyValuePairsDto<Guid?>
                {
                    Id = l.InitialCase.Researchers.Where(c => !c.IsDeleted).Select(c => c.Researcher).FirstOrDefault().Id,
                    Name = l.InitialCase.Researchers.Where(c => !c.IsDeleted).Select(c => c.Researcher).FirstOrDefault().FirstName + " " + l.InitialCase.Researchers.Where(c => !c.IsDeleted).Select(c => c.Researcher).FirstOrDefault().LastName
                }))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.UpdatedByUser, opt => opt.MapFrom(c => c.UpdatedByUser.FirstName + " " + c.UpdatedByUser.LastName))
                .ForMember(res => res.SecondSubCategory, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.SecondSubCategory.Id, Name = c.SecondSubCategory.Name }))
                .ForMember(res => res.Type, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.Type, Name = EnumExtensions.GetDescription(c.Type) }))
                .ForMember(res => res.Status, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.Status, Name = EnumExtensions.GetDescription(c.Status) }))
                .ForMember(res => res.BoardMeetingId, opt => opt.MapFrom(b => b.BoardMeetings.FirstOrDefault().Id))
                .ForMember(res => res.InitialCaseNumber, opt => opt.MapFrom(b => b.InitialCase.CaseNumberInSource));


            CreateMap<Hearing, LegalMemoForPrintDetailsDto>()
                .ForMember(res => res.Researcher, opt => opt.MapFrom(req => req.Case.Researchers.FirstOrDefault().Researcher.FirstName + " " + req.Case.Researchers.FirstOrDefault().Researcher.LastName))
                .ForMember(res => res.CircleNumber, opt => opt.MapFrom(req => req.Case.CircleNumber))
                .ForMember(res => res.CaseSourceNumber, opt => opt.MapFrom(req => req.Case.CaseNumberInSource))
                .ForMember(res => res.Text, opt => opt.MapFrom(req => req.HearingLegalMemoReviewRequests.LastOrDefault().LegalMemo.Text))
                .ForMember(res => res.StartDate, opt => opt.MapFrom(req => req.Case.StartDate))
                .ForMember(res => res.Subject, opt => opt.MapFrom(req => req.Case.Subject))
                .ForMember(res => res.Court, opt => opt.MapFrom(req => req.Case.Court.Name))
                .ForMember(res => res.Defendant, opt => opt.MapFrom(req =>
                   req.Case.LegalStatus == MinistryLegalStatuses.Defendant ? "الوزارة" : PrintHelper.TransformParties(req.Case.Parties.Select(c => new PartyDto
                   {
                       Name = c.Party.Name,
                       PartyType = c.Party.PartyType,
                       IdentityTypeId = c.Party.IdentityTypeId,
                       IdentityValue = c.Party.IdentityValue,
                       CommertialRegistrationNumber = c.Party.CommertialRegistrationNumber
                   }).ToList())))
               .ForMember(res => res.Plaintiff, opt => opt.MapFrom(req =>
                   req.Case.LegalStatus == MinistryLegalStatuses.Plaintiff ? "الوزارة" : PrintHelper.TransformParties(req.Case.Parties.Select(c => new PartyDto
                   {
                       Name = c.Party.Name,
                       PartyType = c.Party.PartyType,
                       IdentityTypeId = c.Party.IdentityTypeId,
                       IdentityValue = c.Party.IdentityValue,
                       CommertialRegistrationNumber = c.Party.CommertialRegistrationNumber
                   }).ToList())))
               .ReverseMap();


            CreateMap<LegalMemo, LegalMemoDto>();

            CreateMap<LegalMemoDto, LegalMemo>()
                .ForMember(l => l.Id, opt => opt.Ignore())
                .ForMember(l => l.SecondSubCategory, opt => opt.Ignore());
            //.AfterMap((memoRes, momoModel) =>
            //{
            //    var removedCategories =LegalMemoSecondSubCategory
            //    foreach (var type in momoModel.CaseCategories)
            //        if (memoRes.SecondSubCategoryId==type.SecondSubCategoryId)
            //            removedCategories.Add(type);

            //    foreach (var type in removedCategories)
            //        momoModel.CaseCategories.Remove(type);

            //    foreach (var id in memoRes.CaseCategories)
            //        if (!momoModel.CaseCategories.Select(c => c.LegalMemoId).Contains(id))
            //            momoModel.CaseCategories.Add(new LegalMemoSecondSubCategory
            //            {
            //                SecondSubCategoryId = id,
            //                LegalMemoId = momoModel.Id
            //            });
            //});

            //CreateMap<LegalMemoQueryObjectDto, LegalMemoQueryObject>();

            #endregion


            #region LegalMemoNote

            CreateMap<LegalMemoNote, LegalMemoNoteListItemDto>()
                .ForMember(res => res.CreatedBy, opt => opt.MapFrom(a => new KeyValuePairsDto<Guid> { Id = a.CreatedBy, Name = a.CreatedByUser.FirstName + ' ' + a.CreatedByUser.LastName }))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.CreationTime, opt => opt.MapFrom(c => DateTimeHelper.GetFullTime(c.CreatedOn)))
                .ForMember(res => res.BoardName, opt => opt.MapFrom(b => b.LegalBoard.Name));


            CreateMap<LegalMemoNoteDto, LegalMemoNote>()
                .ForMember(l => l.Id, opt => opt.Ignore())
                .ReverseMap();

            //CreateMap<LegalMemoNoteQueryObjectDto, LegalMemoNoteQueryObject>();

            #endregion


            #region LegalMemosHistory

            CreateMap<LegalMemosHistoryDto, LegalMemoHistory>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<LegalMemo, LegalMemosHistoryDto>()
                .ForMember(m => m.LegalMemoId, opt => opt.MapFrom(prop => prop.Id))
                .ReverseMap();

            CreateMap<LegalMemo, LegalMemoHistory>()
                .ForMember(m => m.LegalMemoId, opt => opt.MapFrom(prop => prop.Id))
                .ReverseMap();
            #endregion
        }
    }
}
