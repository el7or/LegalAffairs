using Moe.La.Core.Entities;
using Xunit;

namespace Moe.La.UnitTests
{
    public class InvestigationUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Get_Investigation_List_Given_Valid_Search_Information()
        {
            // Arrange
            var service = ServiceHelper.CreateInvestigationService();

            // Act
            var result = await service.GetAllAsync(new InvestigationQueryObject { SearchText = "" });

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
