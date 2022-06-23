using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class CityUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_City_Given_Valid_Information()
        {
            // Arrange
            var city = new CityBuilder().WithDefaultValues().Build();
            var cityService = ServiceHelper.CreateCityService();

            // Act
            var result = await cityService.AddAsync(city);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_City_Given_Valid_Information()
        {
            // Arrange
            var city = new CityBuilder().WithDefaultValues().Build();
            var cityService = ServiceHelper.CreateCityService();

            // Act
            await cityService.AddAsync(city);
            city.Name = "الباحة";
            city.ProvinceId = 2;

            var result = await cityService.EditAsync(city);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("الباحة", result.Data.Name);
            Assert.Equal(2, result.Data.ProvinceId);
        }

        [Fact]
        public async void Delete_City_Given_Valid_Information()
        {
            // Arrange
            var city = new CityBuilder().WithDefaultValues().Build();
            var cityService = ServiceHelper.CreateCityService();

            // Act            
            var result = await cityService.AddAsync(city);
            await cityService.RemoveAsync(city.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Get_City_By_Id_Given_Valid_Information()
        {
            // Arrange
            var city = new CityBuilder().WithDefaultValues().Build();
            var cityService = ServiceHelper.CreateCityService();

            // Act
            await cityService.AddAsync(city);
            var result = await cityService.GetAsync(city.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(city.Id, result.Data.Id);
        }
    }

    public class WorkflowTypeUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_Workflow_Type_Given_Valid_Information()
        {
            // Arrange
            var workflowType = new WorkflowTypeBuilder().WithDefaultValues().Build();
            var workflowTypeService = ServiceHelper.CreateWorkflowConfigurationService();

            // Act

            // Assert
        }
    }
}
