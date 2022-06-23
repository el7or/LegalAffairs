using Moe.La.Core.Dtos;
using System;

namespace Moe.La.UnitTests.Builders
{
    public class ChangeResearcherToHearingRequestBuilder
    {
        private ChangeResearcherToHearingRequestDto _changeResearcherToHearingRequest = new ChangeResearcherToHearingRequestDto();

        public ChangeResearcherToHearingRequestBuilder Id(int id)
        {
            _changeResearcherToHearingRequest.Id = id;
            return this;
        }

        public ChangeResearcherToHearingRequestBuilder HearingId(int hearingId)
        {
            _changeResearcherToHearingRequest.HearingId = hearingId;
            return this;
        }

        public ChangeResearcherToHearingRequestBuilder Note(string note)
        {
            _changeResearcherToHearingRequest.Note = note;
            return this;
        }

        public ChangeResearcherToHearingRequestBuilder NewResearcherId(Guid newResearcherId)
        {
            _changeResearcherToHearingRequest.NewResearcherId = newResearcherId;
            return this;
        }

        public ChangeResearcherToHearingRequestBuilder WithDefaultValues()
        {
            _changeResearcherToHearingRequest = new ChangeResearcherToHearingRequestDto
            {
                HearingId = 1,
                NewResearcherId = TestUsers.LegalResearcherId2,
                Note = "Change researcher to hearing note"
            };

            return this;
        }

        public ChangeResearcherToHearingRequestDto Build() => _changeResearcherToHearingRequest;
    }
}
