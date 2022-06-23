using Moe.La.Core.Dtos;

namespace Moe.La.UnitTests.Builders
{
    class ProvinceBuilder
    {
        private ProvinceDto _province = new ProvinceDto();
        public ProvinceBuilder Name(string name)
        {
            _province.Name = name;
            return this;
        }

        public ProvinceBuilder WithDefaultValues()
        {
            _province = new ProvinceDto
            {
                Name = "منطقه جديدة"
            };

            return this;
        }

        public ProvinceDto Build() => _province;
    }
}
