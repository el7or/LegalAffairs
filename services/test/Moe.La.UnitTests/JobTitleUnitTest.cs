using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class JobTitleUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_JobTitle_Given_Valid_Information()
        {
            // Arrange
            var jobTitle = new JobTitleBuilder().WithDefaultValues().Build();
            var jobTitleService = ServiceHelper.CreateJobTitleService();

            // Act
            var result = await jobTitleService.AddAsync(jobTitle);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_JobTitle_Given_Valid_Information()
        {
            // Arrange
            var jobTitle = new JobTitleBuilder().WithDefaultValues().Build();
            var jobTitleService = ServiceHelper.CreateJobTitleService();

            // Act
            await jobTitleService.AddAsync(jobTitle);
            jobTitle.Name = "testc";

            var result = await jobTitleService.EditAsync(jobTitle);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("testc", result.Data.Name);
        }

        [Fact]
        public async void Delete_JobTitle_Given_Valid_Information()
        {
            // Arrange
            var jobTitle = new JobTitleBuilder().WithDefaultValues().Build();
            var jobTitleService = ServiceHelper.CreateJobTitleService();

            // Act            
            var result = await jobTitleService.AddAsync(jobTitle);
            await jobTitleService.RemoveAsync(jobTitle.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Get_JobTitle_By_Id_Given_Valid_Information()
        {
            // Arrange
            var jobTitle = new JobTitleBuilder().WithDefaultValues().Build();
            var jobTitleService = ServiceHelper.CreateJobTitleService();

            // Act
            await jobTitleService.AddAsync(jobTitle);
            var result = await jobTitleService.GetAsync(jobTitle.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(jobTitle.Id, result.Data.Id);
        }
    }
}
