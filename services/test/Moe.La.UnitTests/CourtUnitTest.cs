using Moe.La.Core.Enums;
using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class CourtUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_Court_Given_Valid_Information()
        {
            // Arrange
            var court = new CourtBuilder().WithDefaultValues().Build();
            var courtService = ServiceHelper.CreateCourtService();

            // Act
            var result = await courtService.AddAsync(court);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_Court_Given_Valid_Information()
        {
            // Arrange
            var court = new CourtBuilder().WithDefaultValues().Build();
            var courtService = ServiceHelper.CreateCourtService();

            // Act
            await courtService.AddAsync(court);
            court.Name = "محكمة 2";
            court.LitigationType = LitigationTypes.FirstInstance;
            court.CourtCategory = CourtCategories.QuasiJudicialCommittees;
            var result = await courtService.EditAsync(court);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("محكمة 2", result.Data.Name);
            Assert.Equal(LitigationTypes.FirstInstance, result.Data.LitigationType);
        }

        [Fact]
        public async void Delete_Part_Given_Valid_Information()
        {
            // Arrange
            var court = new CourtBuilder().WithDefaultValues().Build();
            var courtService = ServiceHelper.CreateCourtService();

            // Act
            var result = await courtService.AddAsync(court);
            await courtService.RemoveAsync(court.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Get_Court_By_Id_Given_Valid_Information()
        {
            // Arrange
            var court = new CourtBuilder().WithDefaultValues().Build();
            var courtService = ServiceHelper.CreateCourtService();

            // Act
            await courtService.AddAsync(court);
            var result = await courtService.GetAsync(court.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(court.Id, result.Data.Id);
        }
    }
}
