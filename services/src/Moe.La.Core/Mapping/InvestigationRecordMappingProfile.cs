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
    public class InvestigationRecordMappingProfile : Profile
    {
        public InvestigationRecordMappingProfile()
        {

            CreateMap<InvestigationRecord, InvestigationRecordDetailsDto>();

            CreateMap<InvestigationRecord, InvestigationRecordListItemDto>()
                .ForMember(res => res.RecordStatus, opt => opt.MapFrom(r => new KeyValuePairsDto<int> { Id = (int)r.RecordStatus, Name = EnumExtensions.GetDescription(r.RecordStatus) }))
                .ForMember(res => res.createdBy, opt => opt.MapFrom(r => r.CreatedByUser.FirstName + " " + r.CreatedByUser.LastName))
                .ForMember(res => res.StartDateHigri, opt => opt.MapFrom(r => DateTimeHelper.GetHigriDate(r.StartDate)))
                .ForMember(res => res.StartTime, opt => opt.MapFrom(r => DateTimeHelper.GetFullTime(r.StartDate)))
                .ForMember(res => res.EndDateHigri, opt => opt.MapFrom(r => DateTimeHelper.GetHigriDate(r.EndDate)))
                .ForMember(res => res.EndTime, opt => opt.MapFrom(r => DateTimeHelper.GetFullTime(r.EndDate)));


            CreateMap<InvestigationRecord, InvestigationRecordDto>();

            CreateMap<InvestigationRecordDto, InvestigationRecord>()
                .ForMember(res => res.Id, opt => opt.Ignore())
                .ForMember(res => res.Attachments, opt => opt.Ignore())
                .ForMember(res => res.InvestigationRecordParties, opt =>
                {
                    opt.PreCondition(r => r.InvestigationRecordParties.Count == 0);
                    opt.Ignore();
                    opt.PreCondition(r => r.InvestigationRecordParties.Count != 0);
                    opt.MapFrom(r => r.InvestigationRecordParties);
                })
                .ForMember(res => res.InvestigationRecordQuestions, opt => opt.Ignore())
                .ForMember(res => res.InvestigationRecordInvestigators, opt => opt.Ignore())
                .ForMember(res => res.Attendants, opt => opt.Ignore())
                .AfterMap((res, entity) =>
                {
                    var attachmentsToAdd = res.Attachments.Where(a => !a.IsDeleted).ToList();

                    var removedAttachments = entity.Attachments.Where(a => !attachmentsToAdd.Select(d => d.Id).Contains(a.Id)).ToList();


                    foreach (var attachment in removedAttachments)
                        entity.Attachments.Remove(attachment);

                    foreach (var attachment in attachmentsToAdd)
                        if (!entity.Attachments.Select(a => (Guid?)a.Id).Contains(attachment.Id))
                            entity.Attachments.Add(new InvestigationRecordAttachment
                            {
                                RecordId = entity.Id,
                                Id = (Guid)attachment.Id
                            });


                    ////
                    var removedQuestions = new List<InvestigationRecordQuestion>();
                    foreach (var question in entity.InvestigationRecordQuestions)
                        if (question.Id != 0 && res.InvestigationRecordQuestions.Any(i => i.Id == question.Id))
                            removedQuestions.Add(question);

                    foreach (var question in removedQuestions)
                        entity.InvestigationRecordQuestions.Remove(question);

                    foreach (var question in res.InvestigationRecordQuestions)
                        entity.InvestigationRecordQuestions.Add(new InvestigationRecordQuestion
                        {
                            QuestionId = question.QuestionId,
                            Answer = question.Answer,
                            InvestigationRecordId = entity.Id,
                            AssignedToId = res.InvestigationRecordParties.Any(p => p.IdentityNumber == question.AssignedTo.IdentityNumber) ? (int)res.InvestigationRecordParties.Where(p => p.IdentityNumber == question.AssignedTo.IdentityNumber).FirstOrDefault().Id : question.AssignedTo.Id.Value
                        });
                    ////
                    var removedInvestigators = new List<InvestigationRecordInvestigator>();
                    foreach (var investigator in entity.InvestigationRecordInvestigators)
                        if (investigator.Id != 0 && res.InvestigationRecordInvestigators.Contains(investigator.InvestigatorId))
                            removedInvestigators.Add(investigator);

                    foreach (var investigator in removedInvestigators)
                        entity.InvestigationRecordInvestigators.Remove(investigator);

                    foreach (var investigator in res.InvestigationRecordInvestigators)
                        entity.InvestigationRecordInvestigators.Add(new InvestigationRecordInvestigator
                        {
                            InvestigatorId = investigator,
                            InvestigationRecordId = entity.Id
                        });
                    ///
                    var removedAttendants = new List<InvestigationRecordAttendant>();
                    foreach (var attendant in entity.Attendants)
                        if (attendant.Id != 0 && res.Attendants.Any(i => i.Id == attendant.Id))
                            removedAttendants.Add(attendant);

                    foreach (var attendant in removedAttendants)
                        entity.Attendants.Remove(attendant);

                    foreach (var attendant in res.Attendants)
                        entity.Attendants.Add(new InvestigationRecordAttendant
                        {
                            RepresentativeOfId = attendant.RepresentativeOfId,
                            InvestigationRecordId = entity.Id,
                            IdentityNumber = attendant.IdentityNumber,
                            FullName = attendant.FullName,
                            WorkLocation = attendant.WorkLocation,
                            AssignedWork = attendant.AssignedWork,
                            Details = attendant.Details,
                            CreatedOn = DateTime.Now

                        });
                });

            CreateMap<InvestigationRecordQuestion, InvestigationRecordQuestionDto>()
                .ForMember(res => res.Question, opt => opt.MapFrom(c => c.Question.Question)).ReverseMap();

            CreateMap<InvestigationRecordQuestionDto, InvestigationRecordQuestion>();


            ///
            CreateMap<InvestigationRecordParty, InvestigationRecordPartyDto>()
                .ForMember(res => res.StaffTypeName, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.StaffType)))
                .ForMember(res => res.InvestigationRecordPartyTypeName, opt => opt.MapFrom(c => c.InvestigationRecordPartyType.Name));

            CreateMap<InvestigationRecordPartyDto, InvestigationRecordParty>()
                .ForMember(res => res.Id, opt => opt.Ignore());

            ///
            CreateMap<EducationalLevel, EducationalLevelDto>();

            CreateMap<EducationalLevelDto, EducationalLevel>()
                .ForMember(res => res.Id, opt => opt.Ignore());

            ///
            CreateMap<Evaluation, EvaluationDto>();

            CreateMap<EvaluationDto, Evaluation>()
                .ForMember(res => res.Id, opt => opt.Ignore());

            ///
            CreateMap<InvestigationPartyPenalty, InvestigationRecordPartyPenaltyDto>();

            CreateMap<InvestigationRecordPartyPenaltyDto, InvestigationPartyPenalty>()
                .ForMember(res => res.Id, opt => opt.Ignore());


            CreateMap<InvestigationRecordAttendant, InvestigationRecordAttendantDto>();

        }
    }
}
