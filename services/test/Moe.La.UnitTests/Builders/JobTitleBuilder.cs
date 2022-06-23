using Moe.La.Core.Dtos;

namespace Moe.La.UnitTests.Builders
{
    class JobTitleBuilder
    {
        private JobTitleDto _jobTitle = new JobTitleDto();
        public JobTitleBuilder Name(string name)
        {
            _jobTitle.Name = name;
            return this;
        }

        public JobTitleBuilder WithDefaultValues()
        {
            _jobTitle = new JobTitleDto
            {
                Name = "testc"
            };

            return this;
        }

        public JobTitleDto Build() => _jobTitle;
    }
}
