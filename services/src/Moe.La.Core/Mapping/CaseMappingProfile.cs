using AutoMapper;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moe.La.Core.Mapping
{
    class CaseMappingProfile : Profile
    {
        public CaseMappingProfile()
        {
            CreateMap<CaseGrounds, CaseGroundsDto>();

            CreateMap<CaseGroundsDto, CaseGrounds>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<CaseMoamala, CaseMoamalatDto>();

            CreateMap<CaseMoamalatDto, CaseMoamala>();

            CreateMap<Case, CaseListItemDto>()
                .ForMember(res => res.CaseSource, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.CaseSource, Name = EnumExtensions.GetDescription(c.CaseSource) }))
                .ForMember(res => res.LitigationType, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.LitigationType, Name = EnumExtensions.GetDescription(c.LitigationType) }))
                .ForMember(res => res.Branch, opt => opt.MapFrom(c => c.Branch.Name))
                .ForMember(res => res.AddUser, opt => opt.MapFrom(c => c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName))
                .ForMember(res => res.Court, opt => opt.MapFrom(c => c.Court.Name))
                .ForMember(res => res.SecondSubCategory, opt => opt.MapFrom(c => c.SecondSubCategory.Name))
                .ForMember(res => res.CaseGrounds, opt => opt.MapFrom(c => c.CaseGrounds.Select(cc => cc.Text)))
                .ForMember(res => res.Status, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.Status, Name = EnumExtensions.GetDescription(c.Status) }))
                .ForMember(res => res.LegalStatus, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.LegalStatus)))
                .ForMember(res => res.HearingsCount, opt => opt.MapFrom(c => c.Hearings.Count()))
                .ForMember(res => res.RelatedCaseRef, opt => opt.MapFrom(c => c.RelatedCaseId == null ? "" : c.CaseNumberInSource))
                .ForMember(res => res.Researchers, opt => opt.MapFrom(c => c.Researchers.Select(cc => new KeyValuePairsDto<Guid> { Id = cc.Researcher.Id, Name = cc.Researcher.FirstName + " " + cc.Researcher.LastName })))
                .ForMember(res => res.StartDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.StartDate)))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.CreatedOnTime, opt => opt.MapFrom(c => DateTimeHelper.GetTime(c.CreatedOn)))
                .ForMember(res => res.IsFinalJudgment, opt => opt.MapFrom(c => c.CaseRule != null ? c.CaseRule.IsFinalJudgment.Value : false))
                .ForMember(res => res.RemainingObjetcion, opt => opt.MapFrom(c => c.ReceivingJudgmentDate != null ? (int?)(c.ReceivingJudgmentDate.Value.AddDays(30).Date - DateTime.Now.Date).TotalDays : null))
                .ForMember(res => res.ObjectionJudgmentLimitDateHijri, opt => opt.MapFrom(c => c.ReceivingJudgmentDate.HasValue ? DateTimeHelper.GetHigriDate(c.ReceivingJudgmentDate.Value.AddDays(30)) : ""))
                .ForMember(res => res.IsCaseDataCompleted, opt => opt.MapFrom(c => c.CourtId != null && !String.IsNullOrWhiteSpace(c.Subject) && !String.IsNullOrWhiteSpace(c.CircleNumber)))
                .ForMember(res => res.PronouncingJudgmentDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.PronouncingJudgmentDate)))
                .ForMember(res => res.ReceivingJudgmentDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.ReceivingJudgmentDate)))
                .ForMember(res => res.FinishedPronouncedHearing, opt => opt.MapFrom(c => c.Hearings.Any(h => h.IsPronouncedJudgment == true && h.Status == Enums.HearingStatuses.Closed)))
                .ForMember(h => h.Parties, opt => opt.Ignore())
                .AfterMap((caseRes, caseEntity) =>
                {

                    foreach (var party in caseRes.Parties.Select(p => p.Party))
                    {
                        caseEntity.Parties.Add(new PartyDto
                        {
                            Id = party.Id,
                            Name = party.Name,
                            PartyType = party.PartyType,
                            IdentityTypeId = party.IdentityTypeId,
                            IdentityValue = party.IdentityValue,
                            IdentitySource = party.IdentitySource,
                            IdentityStartDate = party.IdentityStartDate,
                            IdentityExpireDate = party.IdentityExpireDate,
                            NationalityId = party.NationalityId,
                            CommertialRegistrationNumber = party.CommertialRegistrationNumber,
                            BirthDate = party.BirthDate,
                            Gender = party.Gender,
                            Mobile = party.Mobile,
                            ProvinceId = party.ProvinceId,
                            CityId = party.CityId,
                            Region = party.Region,
                            DistrictId = party.DistrictId,
                            Street = party.Street,
                            BuidlingNumber = party.BuidlingNumber,
                            PostalCode = party.PostalCode,
                            AddressDetails = party.AddressDetails,
                            TelephoneNumber = party.TelephoneNumber,
                            Email = party.Email
                        });
                    }
                });


            CreateMap<Case, CaseDetailsDto>()
                .ForMember(res => res.CaseYearInSourceHijri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriYearInt(c.StartDate)))
                .ForMember(res => res.StartDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.StartDate)))
                .ForMember(res => res.Court, opt => opt.MapFrom(c => c.Court != null ? new KeyValuePairsDto<int> { Id = c.Court.Id, Name = c.Court.Name } : null))
                .ForMember(res => res.CaseSource, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.CaseSource, Name = EnumExtensions.GetDescription(c.CaseSource) }))
                .ForMember(res => res.LitigationType, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.LitigationType, Name = EnumExtensions.GetDescription(c.LitigationType) }))
                .ForMember(res => res.LegalStatus, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.LegalStatus, Name = EnumExtensions.GetDescription(c.LegalStatus) }))
                .ForMember(res => res.Status, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.Status, Name = EnumExtensions.GetDescription(c.Status) }))
                .ForMember(res => res.SecondSubCategory, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.SecondSubCategory.Id, Name = c.SecondSubCategory.Name }))
                .ForMember(res => res.BranchName, opt => opt.MapFrom(c => c.Branch.Name))
                .ForMember(res => res.Researchers, opt => opt.MapFrom(c => c.Researchers.Select(cc => new KeyValuePairsDto<Guid> { Id = cc.Researcher.Id, Name = cc.Researcher.FirstName + " " + cc.Researcher.LastName })))
                .ForMember(res => res.CaseRuleId, opt => opt.MapFrom(c => c.CaseRule != null ? c.CaseRule.Id : 0))
                .ForMember(res => res.JudgementResults, opt => opt.MapFrom(c => c.CaseRule.JudgementResult))
                .ForMember(res => res.FinishedPronouncedHearing, opt => opt.MapFrom(c => c.Hearings.Any(h => h.IsPronouncedJudgment == true && h.Status == Enums.HearingStatuses.Closed)))
                .ForMember(res => res.RemainingObjetcion, opt => opt.MapFrom(c => c.ReceivingJudgmentDate != null ? (int?)(c.ReceivingJudgmentDate.Value.AddDays(30).Date - DateTime.Now.Date).TotalDays : null))
                .ForMember(res => res.AttachmentsCount, opt => opt.MapFrom(c => c.Attachments.Count() + c.Hearings.Where(h => h.Attachments.Count() > 0).Count()))
                .ForMember(res => res.IsFinalJudgment, opt => opt.MapFrom(c => c.CaseRule != null ? c.CaseRule.IsFinalJudgment : false))
                .ForMember(res => res.ReceivingJudgmentDateHijri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.ReceivingJudgmentDate)))
                .ForMember(res => res.PronouncingJudgmentDateHijri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.PronouncingJudgmentDate)))
                .ForMember(res => res.ObjectionJudgmentLimitDateHijri, opt => opt.MapFrom(c => c.ReceivingJudgmentDate.HasValue ? DateTimeHelper.GetHigriDate(c.ReceivingJudgmentDate.Value.AddDays(30)) : ""))
                .ForMember(res => res.StartDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.StartDate)))
                .ForMember(res => res.SubCategory, opt => opt.MapFrom(c => c.SecondSubCategory))
                .ForMember(res => res.CaseMoamalat, opt => opt.MapFrom(c => c.CaseMoamalat))
                .ForMember(h => h.CaseParties, opt => opt.Ignore())
                .AfterMap((caseRes, caseEntity) =>
                {
                    foreach (var caseParty in caseRes.Parties)
                    {
                        caseEntity.CaseParties.Add(new CasePartyDto()
                        {
                            Id = caseParty.Id,
                            CaseId = caseParty.CaseId,
                            Party = new PartyDto()
                            {
                                Id = caseParty.Party.Id,
                                Name = caseParty.Party.Name,
                                PartyType = caseParty.Party.PartyType,
                                PartyTypeName = EnumExtensions.GetDescription(caseParty.Party.PartyType),
                            },
                            PartyId = caseParty.PartyId,
                            PartyStatusName = EnumExtensions.GetDescription(caseParty.PartyStatus),
                            PartyStatus = caseParty.PartyStatus
                        });
                    }
                })
                .ForMember(h => h.Parties, opt => opt.Ignore())
                .AfterMap((caseRes, caseEntity) =>
                {

                    foreach (var party in caseRes.Parties.Select(p => p.Party))
                    {
                        caseEntity.Parties.Add(new PartyDto
                        {
                            Id = party.Id,
                            Name = party.Name,
                            PartyType = party.PartyType,
                            IdentityTypeId = party.IdentityTypeId,
                            IdentityValue = party.IdentityValue,
                            IdentitySource = party.IdentitySource,
                            IdentityStartDate = party.IdentityStartDate,
                            IdentityExpireDate = party.IdentityExpireDate,
                            NationalityId = party.NationalityId,
                            CommertialRegistrationNumber = party.CommertialRegistrationNumber,
                            BirthDate = party.BirthDate,
                            Gender = party.Gender,
                            Mobile = party.Mobile,
                            ProvinceId = party.ProvinceId,
                            CityId = party.CityId,
                            Region = party.Region,
                            DistrictId = party.DistrictId,
                            Street = party.Street,
                            BuidlingNumber = party.BuidlingNumber,
                            PostalCode = party.PostalCode,
                            AddressDetails = party.AddressDetails,
                            TelephoneNumber = party.TelephoneNumber,
                        });
                    }
                });



            CreateMap<Case, Case>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(h => h.Parties, opt => opt.Ignore())
                .ForMember(h => h.Researchers, opt => opt.Ignore())
                .ForMember(c => c.CaseGrounds, opt => opt.Ignore())
                .AfterMap((caseRes, caseEntity) =>
                {
                    //add case grounds
                    caseEntity.CaseGrounds = caseRes.CaseGrounds.Select(g => new CaseGrounds { Text = g.Text, CreatedOn = DateTime.Now }).ToList();

                    // add case parties
                    caseEntity.Parties = caseRes.Parties.Select(g => new CaseParty { PartyId = g.PartyId, PartyStatus = g.PartyStatus, CreatedOn = DateTime.Now }).ToList();

                    // add case researcher
                    caseEntity.Researchers.Add(caseRes.Researchers.Select(g => new CaseResearcher { ResearcherId = g.ResearcherId, CreatedOn = DateTime.Now }).LastOrDefault());

                }); ;

            CreateMap<CaseDto, Case>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(h => h.Attachments, opt => opt.Ignore())
                .ForMember(h => h.Parties, opt => opt.Ignore())
                .ForMember(c => c.CaseGrounds, opt => opt.Ignore())
                .ForMember(res => res.CaseYearInSource, opt => opt.MapFrom(c => c.StartDate.Year));

            CreateMap<BasicCaseDto, Case>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(res => res.CaseYearInSource, opt => opt.MapFrom(c => c.StartDate.Year));

            CreateMap<NextCaseDto, Case>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            //.ForMember(res => res.CaseYearInSource, opt => opt.MapFrom(c => c.StartDate.Year))

            CreateMap<Case, CaseDto>()
                .ForMember(res => res.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(res => res.HearingsCount, opt => opt.MapFrom(c => c.Hearings.Count()));

            CreateMap<Case, CaseHistory>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(res => res.CaseId, opt => opt.MapFrom(c => c.Id));

            CreateMap<NajizCaseDto, Case>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<InitialCaseDto, Case>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.SecondSubCategory, opt => opt.Ignore());

            CreateMap<ObjectionCaseDto, Case>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.SecondSubCategory, opt => opt.Ignore());

            #region Moeen
            CreateMap<MoeenCaseDto, MoeenCase>()
               .ForMember(c => c.Id, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<MoeenHearingDto, MoeenHearing>()
              .ForMember(c => c.Id, opt => opt.Ignore())
              .ReverseMap();

            CreateMap<MoeenPartyDto, MoeenParty>()
              .ForMember(c => c.Id, opt => opt.Ignore())
              .ReverseMap();

            #endregion

            #region Najiz
            CreateMap<NajizCaseDto, NajizCase>()
               .ForMember(c => c.Id, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<NajizHearingDto, NajizHearing>()
              .ForMember(c => c.Id, opt => opt.Ignore())
              .ReverseMap();

            CreateMap<NajizPartyDto, NajizParty>()
              .ForMember(c => c.Id, opt => opt.Ignore())
              .ReverseMap();

            #endregion

            CreateMap<CaseResearchersDto, CaseResearcher>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<CaseResearcher, CaseResearchersDto>()
                .ForMember(c => c.CreatedByRole, opt => opt.MapFrom(c => c.CreatedByUser.UserRoles.Select(r => r.Role.Id).FirstOrDefault()))
                .ForMember(c => c.CreatedByBranchId, opt => opt.MapFrom(c => c.CreatedByUser.BranchId));

            CreateMap<CaseRuleDto, CaseRule>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CaseTransaction, CaseTransactionDetailsDto>()
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.CreatedOnTime, opt => opt.MapFrom(c => DateTimeHelper.GetFullTime(c.CreatedOn)))
                .ForMember(res => res.TransactionType, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.TransactionType)))
                .ForMember(res => res.CreatedByUser, opt => opt.MapFrom(c => c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName));

            CreateMap<CaseTransactionDto, CaseTransaction>()
              .ForMember(c => c.Id, opt => opt.Ignore())
              .ReverseMap();
            CreateMap<ReceiveJdmentInstrumentDto, CaseRule>()
              .ForMember(c => c.Attachments, opt => opt.Ignore())
              .ForMember(c => c.CaseRuleMinistryDepartments, opt => opt.Ignore())
              .AfterMap((caseRuleRes, caseRuleEntity) =>
              {
                  var removedDepartments = new List<CaseRuleMinistryDepartment>();
                  foreach (var department in caseRuleEntity.CaseRuleMinistryDepartments)
                      if (!caseRuleRes.CaseRuleMinistryDepartments.Contains(department.Id))
                          removedDepartments.Add(department);

                  foreach (var department in removedDepartments)
                      caseRuleEntity.CaseRuleMinistryDepartments.Remove(department);
                  foreach (var caseRuleMinistryDepartment in caseRuleRes.CaseRuleMinistryDepartments)
                      caseRuleEntity.CaseRuleMinistryDepartments.Add(new CaseRuleMinistryDepartment
                      {
                          MinistryDepartmentId = caseRuleMinistryDepartment,
                          CaseRuleId = caseRuleEntity.Id
                      });
                  if (caseRuleRes.Attachments != null && caseRuleRes.Attachments.Count > 0)
                  {
                      var attachmentsToAdd = caseRuleRes.Attachments.Where(a => !a.IsDeleted).ToList();
                      var removedAttachments = caseRuleEntity.Attachments.Where(a => !attachmentsToAdd.Select(d => d.Id).Contains(a.Id)).ToList();

                      foreach (var attachment in removedAttachments)
                          caseRuleEntity.Attachments.Remove(attachment);

                      foreach (var attachment in attachmentsToAdd)
                          if (!caseRuleEntity.Attachments.Select(a => (Guid?)a.Id).Contains(attachment.Id))
                              caseRuleEntity.Attachments.Add(new CaseRuleAttachment
                              {
                                  CaseRuleId = caseRuleEntity.Id,
                                  Id = (Guid)attachment.Id
                              });
                  }


              });

            CreateMap<CaseRule, ReceiveJdmentInstrumentDto>();

            CreateMap<CaseRule, ReceiveJdmentInstrumentDetailsDto>()
               .ForMember(res => res.CaseRuleMinistryDepartments, opt => opt.MapFrom(c => c.CaseRuleMinistryDepartments.Select(md => new KeyValuePairsDto<int> { Id = md.MinistryDepartment.Id, Name = md.MinistryDepartment.Name })))
               .ForMember(res => res.JudgementResult, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.JudgementResult, Name = EnumExtensions.GetDescription(c.JudgementResult) }))
               .ForMember(res => res.ImportRefDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.ImportRefDate)))
               .ForMember(res => res.MinistrySector, opt => opt.MapFrom(c => c.MinistrySector.Name))
               .ForMember(res => res.ExportRefDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.ExportRefDate)));


            CreateMap<CaseRuleProsecutorRequest, CaseRuleProsecutorRequestDto>().ReverseMap();
        }
    }
}
