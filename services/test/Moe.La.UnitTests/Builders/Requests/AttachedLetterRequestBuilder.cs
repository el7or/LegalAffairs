using Moe.La.Core.Dtos;

namespace Moe.La.UnitTests.Builders.Requests
{
    public class AttachedLetterRequestBuilder
    {
        private AttachedLetterRequestDto _attachedLetterRequest = new AttachedLetterRequestDto();

        public AttachedLetterRequestBuilder Id(int? id)
        {
            _attachedLetterRequest.Id = id;
            return this;
        }

        public AttachedLetterRequestBuilder ParentId(int? parentId)
        {
            _attachedLetterRequest.ParentId = parentId;
            return this;
        }

        public AttachedLetterRequestBuilder Request(RequestDto request)
        {
            _attachedLetterRequest.Request = request;
            return this;
        }

        public AttachedLetterRequestBuilder WithDefaultValues()
        {
            var documentRequest = new DocumentRequestBuilder().WithDefaultValues().Build();
            _attachedLetterRequest = new AttachedLetterRequestDto()
            {
                HearingId = 1,
                Request = new RequestBuilder().WithDefaultValues().RequestType(Core.Enums.RequestTypes.RequestAttachedLetter).Build(),
                ParentId = documentRequest.Id,
            };

            return this;
        }

        public AttachedLetterRequestDto Build() => _attachedLetterRequest;
    }
}
