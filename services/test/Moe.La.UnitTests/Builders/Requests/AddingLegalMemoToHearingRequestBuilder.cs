using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;

namespace Moe.La.UnitTests.Builders
{
    public class AddingLegalMemoToHearingRequestBuilder
    {
        private AddingLegalMemoToHearingRequestDto _addingLegalMemoToHearingRequest = new AddingLegalMemoToHearingRequestDto();
        private ReplyAddingLegalMemoToHearingRequestDto _replyAddingLegalMemoToHearingRequest = new ReplyAddingLegalMemoToHearingRequestDto();
        public AddingLegalMemoToHearingRequestBuilder Id(int id)
        {
            _addingLegalMemoToHearingRequest.Id = id;
            return this;
        }

        public AddingLegalMemoToHearingRequestBuilder LegalMemoId(int legalMemoId)
        {
            _addingLegalMemoToHearingRequest.LegalMemoId = legalMemoId;
            return this;
        }
        public AddingLegalMemoToHearingRequestBuilder WithDefaultValues()
        {
            _addingLegalMemoToHearingRequest = new AddingLegalMemoToHearingRequestDto
            {
                ReplyNote = "test",
                HearingId = 1,
                LegalMemoId = 1
            };

            return this;
        }

        public AddingLegalMemoToHearingRequestBuilder WithReplyAddingLegalMemoToHearingRequestDefaultValues()
        {
            _replyAddingLegalMemoToHearingRequest = new ReplyAddingLegalMemoToHearingRequestDto()
            {
                ReplyNote = "test",
                RequestStatus = RequestStatuses.Accepted
            };

            return this;
        }
        public AddingLegalMemoToHearingRequestDto Build() => _addingLegalMemoToHearingRequest;
        public ReplyAddingLegalMemoToHearingRequestDto BuildReply() => _replyAddingLegalMemoToHearingRequest;
    }
}
