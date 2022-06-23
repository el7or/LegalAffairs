using Moe.La.Core.Dtos;

namespace Moe.La.UnitTests.Builders
{
    class PartyTypeBuilder
    {
        private PartyTypeDto _partyType = new PartyTypeDto();
        public PartyTypeBuilder Name(string name)
        {
            _partyType.Name = name;
            return this;
        }

        public PartyTypeBuilder WithDefaultValues()
        {
            _partyType = new PartyTypeDto
            {
                Name = "aaa123"
            };

            return this;
        }

        public PartyTypeDto Build() => _partyType;
    }
}
