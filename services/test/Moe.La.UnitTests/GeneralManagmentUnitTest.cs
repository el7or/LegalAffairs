using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class GeneralManagementUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_GeneralManagement_Given_Valid_Information()
        {
            // Arrange
            var GeneralManagement = new GeneralManagementBuilder().WithDefaultValues().Build();
            var legalAffairsDepartmenService = ServiceHelper.CreateGeneralManagementService();

            // Act
            var result = await legalAffairsDepartmenService.AddAsync(GeneralManagement);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_GeneralManagement_Given_Valid_Information()
        {
            // Arrange
            var GeneralManagement = new GeneralManagementBuilder().WithDefaultValues().Build();
            var LegalAffairsDepartmenService = ServiceHelper.CreateGeneralManagementService();

            // Act
            await LegalAffairsDepartmenService.AddAsync(GeneralManagement);

            var result = await LegalAffairsDepartmenService.EditAsync(GeneralManagement);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(GeneralManagement.Name, result.Data.Name);
        }

        [Fact]
        public async void Delete_GeneralManagement_Given_Valid_Information()
        {
            // Arrange
            var GeneralManagement = new GeneralManagementBuilder().WithDefaultValues().Build();
            var legalAffairsDepartmenService = ServiceHelper.CreateGeneralManagementService();

            // Act            
            var result = await legalAffairsDepartmenService.AddAsync(GeneralManagement);
            await legalAffairsDepartmenService.RemoveAsync(GeneralManagement.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Get_GeneralManagement_By_Id_Given_Valid_Information()
        {
            // Arrange
            var GeneralManagement = new GeneralManagementBuilder().WithDefaultValues().Build();
            var legalAffairsDepartmenService = ServiceHelper.CreateGeneralManagementService();

            // Act
            await legalAffairsDepartmenService.AddAsync(GeneralManagement);
            var result = await legalAffairsDepartmenService.GetAsync(GeneralManagement.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(GeneralManagement.Id, result.Data.Id);
        }

    }
}
