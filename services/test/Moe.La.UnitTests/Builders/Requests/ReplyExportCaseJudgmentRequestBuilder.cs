using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;

namespace Moe.La.UnitTests.Builders.Requests
{
    public class ReplyExportCaseJudgmentRequestBuilder
    {
        private ReplyExportCaseJudgmentRequestDto _replyExportCaseJudgmentRequest = new ReplyExportCaseJudgmentRequestDto();

        public ReplyExportCaseJudgmentRequestBuilder Id(int id)
        {
            _replyExportCaseJudgmentRequest.Id = id;
            return this;
        }

        public ReplyExportCaseJudgmentRequestBuilder ReplyNote(string replyNote)
        {
            _replyExportCaseJudgmentRequest.ReplyNote = replyNote;
            return this;
        }

        public ReplyExportCaseJudgmentRequestBuilder RequestStatus(RequestStatuses requestStatus)
        {
            _replyExportCaseJudgmentRequest.RequestStatus = requestStatus;
            return this;
        }

        public ReplyExportCaseJudgmentRequestBuilder WithDefaultValues()
        {
            _replyExportCaseJudgmentRequest = new ReplyExportCaseJudgmentRequestDto
            {
                Id = 0,
                RequestStatus = RequestStatuses.Returned,
                ReplyNote = "This is test note."
            };

            return this;
        }

        public ReplyExportCaseJudgmentRequestDto Build() => _replyExportCaseJudgmentRequest;
    }
}
