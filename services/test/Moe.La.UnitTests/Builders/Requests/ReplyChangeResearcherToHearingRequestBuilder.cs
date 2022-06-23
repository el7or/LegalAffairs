using Moe.La.Core.Dtos;
using System;

namespace Moe.La.UnitTests.Builders
{
    public class ReplyChangeResearcherToHearingRequestBuilder
    {
        private ReplyChangeResearcherToHearingRequestDto _replyChangeResearcherToHearingRequest = new ReplyChangeResearcherToHearingRequestDto();

        public ReplyChangeResearcherToHearingRequestBuilder Id(int id)
        {
            _replyChangeResearcherToHearingRequest.Id = id;
            return this;
        }

        public ReplyChangeResearcherToHearingRequestBuilder ReplyNote(string replyNote)
        {
            _replyChangeResearcherToHearingRequest.ReplyNote = replyNote;
            return this;
        }

        public ReplyChangeResearcherToHearingRequestBuilder CurrentResearcherId(Guid currentResearcherId)
        {
            _replyChangeResearcherToHearingRequest.CurrentResearcherId = currentResearcherId;
            return this;
        }

        public ReplyChangeResearcherToHearingRequestBuilder NewResearcherId(Guid newResearcherId)
        {
            _replyChangeResearcherToHearingRequest.NewResearcherId = newResearcherId;
            return this;
        }

        public ReplyChangeResearcherToHearingRequestBuilder WithDefaultValues()
        {
            _replyChangeResearcherToHearingRequest = new ReplyChangeResearcherToHearingRequestDto
            {
                Id = 1,
                ReplyNote = "reply note 1"
            };

            return this;
        }

        public ReplyChangeResearcherToHearingRequestDto Build() => _replyChangeResearcherToHearingRequest;
    }
}
