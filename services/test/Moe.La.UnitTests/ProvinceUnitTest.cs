using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class ProvinceUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_Province_Given_Valid_Information()
        {
            // Arrange
            var province = new ProvinceBuilder().WithDefaultValues().Build();
            var provinceService = ServiceHelper.CreateProvinceService();

            // Act
            var result = await provinceService.AddAsync(province);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_Province_Given_Valid_Information()
        {
            // Arrange
            var province = new ProvinceBuilder().WithDefaultValues().Build();
            var provinceService = ServiceHelper.CreateProvinceService();

            // Act
            await provinceService.AddAsync(province);
            province.Name = "منطقة 2";

            var result = await provinceService.EditAsync(province);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("منطقة 2", result.Data.Name);
        }

        [Fact]
        public async void Delete_Province_Given_Valid_Information()
        {
            // Arrange
            var province = new ProvinceBuilder().WithDefaultValues().Build();
            var provinceService = ServiceHelper.CreateProvinceService();

            // Act            
            var result = await provinceService.AddAsync(province);
            await provinceService.RemoveAsync(province.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Get_Province_By_Id_Given_Valid_Information()
        {
            // Arrange
            var province = new ProvinceBuilder().WithDefaultValues().Build();
            var provinceService = ServiceHelper.CreateProvinceService();

            // Act
            await provinceService.AddAsync(province);
            var result = await provinceService.GetAsync(province.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(province.Id, result.Data.Id);
        }
    }
}
