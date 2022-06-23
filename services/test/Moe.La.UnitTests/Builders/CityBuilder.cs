using Moe.La.Core.Dtos;

namespace Moe.La.UnitTests.Builders
{
    public class CityBuilder
    {
        private CityDto _city = new CityDto();
        public CityBuilder Name(string name)
        {
            _city.Name = name;
            return this;
        }
        public CityBuilder ProvinceId(int provinceId)
        {
            _city.ProvinceId = provinceId;
            return this;
        }

        public CityBuilder WithDefaultValues()
        {
            _city = new CityDto
            {
                Name = "aaaa",
                ProvinceId = 1,
            };

            return this;
        }

        public CityDto Build() => _city;
    }
}
