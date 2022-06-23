using Moe.La.Core.Dtos;
using System;

namespace Moe.La.UnitTests.Builders
{
    public class ReplyChangeResearcherRequestBuilder
    {
        private ReplyChangeResearcherRequestDto _replyChangeResearcherRequest = new ReplyChangeResearcherRequestDto();

        public ReplyChangeResearcherRequestBuilder Id(int id)
        {
            _replyChangeResearcherRequest.Id = id;
            return this;
        }

        public ReplyChangeResearcherRequestBuilder ReplyNote(string replyNote)
        {
            _replyChangeResearcherRequest.ReplyNote = replyNote;
            return this;
        }

        public ReplyChangeResearcherRequestBuilder CurrentResearcherId(Guid newResearcherId)
        {
            _replyChangeResearcherRequest.CurrentResearcherId = newResearcherId;
            return this;
        }

        public ReplyChangeResearcherRequestBuilder NewResearcherId(Guid newResearcherId)
        {
            _replyChangeResearcherRequest.NewResearcherId = newResearcherId;
            return this;
        }

        public ReplyChangeResearcherRequestBuilder WithDefaultValues()
        {
            _replyChangeResearcherRequest = new ReplyChangeResearcherRequestDto
            {
                Id = 1,
                ReplyNote = "abc123"
            };

            return this;
        }

        public ReplyChangeResearcherRequestDto Build() => _replyChangeResearcherRequest;
    }
}
