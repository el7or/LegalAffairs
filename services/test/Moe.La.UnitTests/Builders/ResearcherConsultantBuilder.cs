using Moe.La.Core.Dtos;
using System;

namespace Moe.La.UnitTests.Builders
{
    public class ResearcherConsultantBuilder
    {
        private ResearcherConsultantDto _researcherConsultant = new ResearcherConsultantDto();

        public ResearcherConsultantBuilder ConsultantId(Guid? consultantId)
        {
            _researcherConsultant.ConsultantId = consultantId;
            return this;
        }

        public ResearcherConsultantBuilder ResearcherId(Guid researcherId)
        {
            _researcherConsultant.ResearcherId = researcherId;
            return this;
        }

        public ResearcherConsultantBuilder WithDefaultValues()
        {
            _researcherConsultant = new ResearcherConsultantDto
            {
                ConsultantId = TestUsers.LegalConsultantId,
                ResearcherId = TestUsers.LegalResearcherId
            };
            return this;
        }

        public ResearcherConsultantDto Build() => _researcherConsultant;

    }
}
