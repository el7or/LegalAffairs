using Moe.La.Core.Dtos;

namespace Moe.La.UnitTests.Builders
{
    public class RequestLetterBuilder
    {
        private RequestLetterDetailsDto _requestLetter = new RequestLetterDetailsDto();
        public RequestLetterBuilder WithDefaultValues()
        {
            _requestLetter = new RequestLetterDetailsDto()
            {
                RequestId = 1,
                Text = "Letter Text"
            };

            return this;
        }

        public RequestLetterDetailsDto Build() => _requestLetter;
    }
}
