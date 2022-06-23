using Moe.La.Core.Dtos;

namespace Moe.La.UnitTests.Builders
{
    class IdentityTypeBuilder
    {
        private IdentityTypeDto _identityType = new IdentityTypeDto();
        public IdentityTypeBuilder Name(string name)
        {
            _identityType.Name = name;
            return this;
        }
        public IdentityTypeBuilder WithDefaultValues()
        {
            _identityType = new IdentityTypeDto
            {
                Name = "aaa123"
            };

            return this;
        }

        public IdentityTypeDto Build() => _identityType;
    }
}
