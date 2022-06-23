using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class IdentityTypeUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_IdentityType_Given_Valid_Information()
        {
            // Arrange
            var identityType = new IdentityTypeBuilder().WithDefaultValues().Build();
            var identityTypeService = ServiceHelper.CreateIdentityTypeService();

            // Act
            var result = await identityTypeService.AddAsync(identityType);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_IdentityType_Given_Valid_Information()
        {
            // Arrange
            var identityType = new IdentityTypeBuilder().WithDefaultValues().Build();
            var identityTypeService = ServiceHelper.CreateIdentityTypeService();

            // Act
            await identityTypeService.AddAsync(identityType);
            identityType.Name = "aaa123";


            var result = await identityTypeService.EditAsync(identityType);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("aaa123", result.Data.Name);
        }

        [Fact]
        public async void Delete_IdentityType_Given_Valid_Information()
        {
            // Arrange
            var identityType = new IdentityTypeBuilder().WithDefaultValues().Build();
            var identityTypeService = ServiceHelper.CreateIdentityTypeService();

            // Act            
            var result = await identityTypeService.AddAsync(identityType);
            await identityTypeService.RemoveAsync(identityType.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Get_IdentityType_By_Id_Given_Valid_Information()
        {
            // Arrange
            var identityType = new IdentityTypeBuilder().WithDefaultValues().Build();
            var identityTypeService = ServiceHelper.CreateIdentityTypeService();

            // Act
            await identityTypeService.AddAsync(identityType);
            var result = await identityTypeService.GetAsync(identityType.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(identityType.Id, result.Data.Id);
        }
    }
}
