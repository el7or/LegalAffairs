using Moe.La.Core.Dtos;
using System;

namespace Moe.La.UnitTests.Builders
{
    public class ChangeResearcherRequestBuilder
    {
        private ChangeResearcherRequestDto _changeResearcherRequest = new ChangeResearcherRequestDto();

        public ChangeResearcherRequestBuilder Id(int id)
        {
            _changeResearcherRequest.Id = id;
            return this;
        }

        //public ChangeResearcherRequestBuilder Request(RequestDto request)
        //{
        //    _changeResearcherRequest.Request = request;
        //    return this;
        //}

        public ChangeResearcherRequestBuilder LegalMemoId(int? legalMemoId)
        {
            _changeResearcherRequest.LegalMemoId = legalMemoId;
            return this;
        }

        public ChangeResearcherRequestBuilder ResearcherIdToBeChanged(Guid researcherIdToBeChanged)
        {
            _changeResearcherRequest.CurrentResearcherId = researcherIdToBeChanged;
            return this;
        }

        public ChangeResearcherRequestBuilder WithDefaultValues()
        {
            _changeResearcherRequest = new ChangeResearcherRequestDto
            {
                //Request = new RequestBuilder().WithDefaultValues().RequestType(Core.Enums.RequestType.RequestResearcherChange).Build(),
                LegalMemoId = null,
                CurrentResearcherId = TestUsers.LegalResearcherId,
                CaseId = 1,
                Note = "Change researcher note"
            };

            return this;
        }

        public ChangeResearcherRequestDto Build() => _changeResearcherRequest;
    }
}
