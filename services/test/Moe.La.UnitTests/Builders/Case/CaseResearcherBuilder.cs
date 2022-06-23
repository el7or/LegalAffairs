using Moe.La.Core.Dtos;
using System;

namespace Moe.La.UnitTests.Builders.Case
{
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

        public CaseResearcherBuilder WithDefaultValues()
        {
            _caseResearcher = new CaseResearchersDto
            {
                CaseId = 1,
                ResearcherId = TestUsers.LegalResearcherId
            };

            return this;
        }

        public CaseResearchersDto Build() => _caseResearcher;
    }
}
