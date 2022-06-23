using Moe.La.Core.Dtos;

namespace Moe.La.UnitTests.Builders.Requests
{
    public class ExportCaseJudgmentRequestBuilder
    {
        private ExportCaseJudgmentRequestDto _exportCaseJudgmentRequest = new ExportCaseJudgmentRequestDto();

        public ExportCaseJudgmentRequestBuilder Id(int id)
        {
            _exportCaseJudgmentRequest.Id = id;
            return this;
        }

        //public ExportCaseJudgmentRequestBuilder ClosingReason(string Reason)
        //{
        //    _exportCaseJudgmentRequest.Reason = Reason;
        //    return this;
        //}

        //public CaseClosingRequestBuilder Request(RequestDto request)
        //{
        //    _caseClosingRequest.Request = request;
        //    return this;
        //}

        //public CaseClosingRequestBuilder ReplyNote(string replyNote)
        //{
        //    _caseClosingRequest.ReplyNote = replyNote;
        //    return this;
        //}

        public ExportCaseJudgmentRequestBuilder WithDefaultValues()
        {
            _exportCaseJudgmentRequest = new ExportCaseJudgmentRequestDto()
            {
                Request = new RequestBuilder().WithDefaultValues().Build(),
                CaseId = 1,
                //Reason = "This is a test reason"
            };

            return this;
        }

        public ExportCaseJudgmentRequestDto Build() => _exportCaseJudgmentRequest;
    }
}
