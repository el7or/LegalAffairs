using Moe.La.Core.Dtos;

namespace Moe.La.UnitTests.Builders
{
    class FieldMissionTypeBuilder
    {
        private FieldMissionTypeDto _fieldMissionType = new FieldMissionTypeDto();
        public FieldMissionTypeBuilder Name(string name)
        {
            _fieldMissionType.Name = name;
            return this;
        }

        public FieldMissionTypeBuilder WithDefaultValues()
        {
            _fieldMissionType = new FieldMissionTypeDto
            {
                Name = "FieldMissionTypetestedit"
            };

            return this;
        }

        public FieldMissionTypeDto Build() => _fieldMissionType;
    }
}
