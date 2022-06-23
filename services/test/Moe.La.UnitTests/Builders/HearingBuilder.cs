using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System;

namespace Moe.La.UnitTests.Builders
{
    public class HearingBuilder
    {
        private HearingDto _hearing = new HearingDto();

        public HearingBuilder Id(int id)
        {
            _hearing.Id = id;
            return this;
        }

        public HearingBuilder CaseId(int caseId)
        {
            _hearing.CaseId = caseId;
            return this;
        }

        public HearingBuilder Status(HearingStatuses status)
        {
            _hearing.Status = status;
            return this;
        }

        public HearingBuilder Type(HearingTypes type)
        {
            _hearing.Type = type;
            return this;
        }

        public HearingBuilder CourtId(int courtId)
        {
            _hearing.CourtId = courtId;
            return this;
        }

        public HearingBuilder CircleNumber(string circleNumber)
        {
            _hearing.CircleNumber = circleNumber;
            return this;
        }

        public HearingBuilder HearingNumber(int hearingNumber)
        {
            _hearing.HearingNumber = hearingNumber;
            return this;
        }

        public HearingBuilder HearingDate(DateTime hearingDate)
        {
            _hearing.HearingDate = hearingDate;
            return this;
        }

        public HearingBuilder HearingTime(string hearingTime)
        {
            _hearing.HearingTime = hearingTime;
            return this;
        }

        public HearingBuilder HearingDesc(string hearingDesc)
        {
            _hearing.HearingDesc = hearingDesc;
            return this;
        }

        public HearingBuilder ClosingReport(string closingReport)
        {
            _hearing.ClosingReport = closingReport;
            return this;
        }
        public HearingBuilder GeneralManagementId(int branchId)
        {
            _hearing.BranchId = branchId;
            return this;
        }
        public HearingBuilder LitigationTypeId(int litigationTypeId)
        {
            _hearing.LitigationTypeId = litigationTypeId;
            return this;
        }

        public HearingBuilder Motions(string motions)
        {
            _hearing.Motions = motions;
            return this;
        }

        public HearingBuilder WithDefaultValues()
        {
            _hearing = new HearingDto()
            {
                CaseId = 1,
                Status = HearingStatuses.Scheduled,
                Type = HearingTypes.Pleading,
                CourtId = 1,
                CircleNumber = "1234",
                HearingNumber = 1,
                HearingDate = DateTime.Now, //new DateTime(2020, 9, 22),
                HearingTime = "10:00",
                HearingDesc = "This is a test description",
                BranchId = 1,
                Motions = "الطلبات",
                LitigationTypeId = 1,
                IsPronouncedJudgment = false,
                AssignedToId = TestUsers.LegalResearcherId
            };

            return this;
        }

        public HearingDto Build() => _hearing;
    }
}
