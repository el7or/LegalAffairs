using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;

namespace Moe.La.UnitTests.Builders.Requests
{
    public class ObjectionPermitRequestBuilder
    {
        private ObjectionPermitRequestDto _objectionPermitRequest = new ObjectionPermitRequestDto();

        public ObjectionPermitRequestBuilder Id(int id)
        {
            _objectionPermitRequest.Id = id;
            return this;
        }
        public ObjectionPermitRequestBuilder Note(string opinionReason)
        {
            _objectionPermitRequest.Note = opinionReason;
            return this;
        }

        public ObjectionPermitRequestBuilder SuggestedOpinon(SuggestedOpinon SuggestedOpinon)
        {
            _objectionPermitRequest.SuggestedOpinon = SuggestedOpinon;
            return this;
        }
        public ObjectionPermitRequestBuilder WithDefaultValues()
        {
            _objectionPermitRequest = new ObjectionPermitRequestDto()
            {
                CaseId = 1,
                RequestStatus = RequestStatuses.New,
                Note = "This is a test reason",
                SuggestedOpinon = Core.Enums.SuggestedOpinon.ObjectionAction,
                ResearcherId = TestUsers.LegalResearcherId,
            };

            return this;
        }

        public ObjectionPermitRequestDto Build() => _objectionPermitRequest;
    }
}
