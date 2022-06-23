using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System;

namespace Moe.La.UnitTests.Builders
{
    public class CaseBuilder
    {
        private BasicCaseDto _case = new BasicCaseDto();

        public CaseBuilder Id(int id)
        {
            _case.Id = id;
            return this;
        }

        public CaseBuilder CaseNumberInSource(string caseSourceNumber)
        {
            _case.CaseNumberInSource = caseSourceNumber;
            return this;
        }

        //public CaseBuilder NajizRef(string najizRef)
        //{
        //    _case.NajizRef = najizRef;
        //    return this;
        //}

        //public CaseBuilder NajizId(string najizId)
        //{
        //    _case.NajizId = najizId;
        //    return this;
        //}

        //public CaseBuilder MoeenRef(string moeenRef)
        //{
        //    _case.MoeenRef = moeenRef;
        //    return this;
        //}

        public CaseBuilder CaseSource(CaseSources caseSource)
        {
            _case.CaseSource = caseSource;
            return this;
        }

        public CaseBuilder LitigationType(LitigationTypes litigationType)
        {
            _case.LitigationType = litigationType;
            return this;
        }

        // public string MainNo { get; set; }

        // public DateTime StartDate { get; set; }

        public CaseBuilder CourtId(int courtId)
        {
            _case.CourtId = courtId;
            return this;
        }

        public CaseBuilder CircleNumber(string circleNumber)
        {
            _case.CircleNumber = circleNumber;
            return this;
        }

        public CaseBuilder Subject(string subject)
        {
            _case.Subject = subject;
            return this;
        }


        public CaseBuilder LegalStatus(MinistryLegalStatuses legalStatus)
        {
            _case.LegalStatus = legalStatus;
            return this;
        }

        public CaseBuilder PerentId(int parentId)
        {
            throw new NotImplementedException();
        }


        //public CaseBuilder CloseDate(DateTime closeDate)
        //{
        //    _case.CloseDate = closeDate;
        //    return this;
        //}

        public CaseBuilder Status(CaseStatuses caseStatus)
        {
            _case.Status = caseStatus;
            return this;
        }

        /// <summary>
        /// The related workflow instance id.
        /// </summary>
        //public CaseBuilder WorkflowInstanceId(Guid workflowInstanceId)
        //{
        //    _case.WorkflowInstanceId = workflowInstanceId;
        //    return this;
        //}

        //public CaseBuilder Parties(List<CasePartyDto> parties)
        //{
        //    _case.Parties = parties;
        //    return this;
        //}

        //public CaseBuilder CaseGrounds(ICollection<CaseGroundsDto> caseGrounds)
        //{
        //    _case.CaseGrounds = caseGrounds;
        //    return this;
        //}

        public CaseBuilder SecondSubCategoryId(int secondSubCategoryId)
        {
            _case.SecondSubCategoryId = secondSubCategoryId;
            return this;
        }

        public CaseBuilder WithDefaultValues()
        {
            _case = new BasicCaseDto
            {
                Id = 0,
                //WorkflowInstanceId = null,
                CaseSource = CaseSources.Najiz,
                CaseNumberInSource = "Najiz-1234",
                //NajizId = "1234",
                //NajizRef = "Najiz-1234",
                CourtId = 1,
                CircleNumber = "1234",
                LegalStatus = MinistryLegalStatuses.Defendant,
                LitigationType = LitigationTypes.FirstInstance,
                Status = CaseStatuses.NewCase,
                CaseDescription = "This is simple case description",
                Subject = "This is simple subject",
                StartDate = DateTime.Now.AddMonths(-1),
                //CaseGrounds = new List<CaseGroundsDto>()
                //{
                //    new CaseGroundsDto(){ Text = "سند جديد"},
                //    new CaseGroundsDto(){ Text = "سند دعوى 1"},
                //}

            };

            return this;
        }

        public BasicCaseDto Build() => _case;
    }

    public class CaseResearcherBuilder
    {
        private CaseResearchersDto _caseResearcher = new CaseResearchersDto();

        public CaseResearcherBuilder CaseId(int caseId)
        {
            _caseResearcher.CaseId = caseId;
            return this;
        }

        public CaseResearcherBuilder ResearcherId(Guid researcherId)
        {
            _caseResearcher.ResearcherId = researcherId;
            return this;
        }

        public CaseResearcherBuilder Note(string note)
        {
            _caseResearcher.Note = note;
            return this;
        }

        public CaseResearcherBuilder WithDefaultValues()
        {
            _caseResearcher = new CaseResearchersDto
            {
                CaseId = 1,
                ResearcherId = TestUsers.LegalResearcherId,
                Note = "This is simple note"
            };

            return this;
        }

        public CaseResearchersDto Build() => _caseResearcher;
    }
}
