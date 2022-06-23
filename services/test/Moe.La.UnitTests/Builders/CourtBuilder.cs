using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;

namespace Moe.La.UnitTests.Builders
{
    class CourtBuilder
    {
        private CourtDto _court = new CourtDto();

        public CourtBuilder Name(string name)
        {
            _court.Name = name;
            return this;
        }

        public CourtBuilder LitigationType(LitigationTypes litigationType)
        {
            _court.LitigationType = litigationType;
            return this;
        }

        public CourtBuilder WithDefaultValues()
        {
            _court = new CourtDto
            {
                CourtCategory = CourtCategories.MinistryOfJustice,
                Name = "المحكمة 1",
                LitigationType = LitigationTypes.FirstInstance
            };

            return this;
        }

        public CourtDto Build() => _court;
    }
}
