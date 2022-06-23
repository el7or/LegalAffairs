using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;

namespace Moe.La.UnitTests.Builders.Requests
{
    public class ReplyObjectionPermitRequestBuilder
    {
        private ReplyObjectionPermitRequestDto _replyObjectionPermitRequest = new ReplyObjectionPermitRequestDto();

        public ReplyObjectionPermitRequestBuilder Id(int id)
        {
            _replyObjectionPermitRequest.Id = id;
            return this;
        }

        public ReplyObjectionPermitRequestBuilder ReplyNote(string replyNote)
        {
            _replyObjectionPermitRequest.ReplyNote = replyNote;
            return this;
        }

        public ReplyObjectionPermitRequestBuilder RequestStatus(RequestStatuses requestStatus)
        {
            _replyObjectionPermitRequest.RequestStatus = requestStatus;
            return this;
        }

        public ReplyObjectionPermitRequestBuilder WithDefaultValues()
        {
            _replyObjectionPermitRequest = new ReplyObjectionPermitRequestDto
            {
                Id = 0,
                RequestStatus = RequestStatuses.AcceptedFromLitigationManager,
                ReplyNote = "This is test note."
            };

            return this;
        }

        public ReplyObjectionPermitRequestDto Build() => _replyObjectionPermitRequest;
    }
}
