using Moe.La.Core.Enums;
using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class LegalBoardUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_LegalBoard_Given_Valid_Information()
        {
            // Arrange
            var legalBoard = new LegalBoardBuilder().WithDefaultValues().Build();
            var legalBoardService = ServiceHelper.CreateLegalBoardService();

            // Act
            var result = await legalBoardService.AddAsync(legalBoard);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_LegalBoard_Given_Valid_Information()
        {
            // Arrange
            var legalBoard = new LegalBoardBuilder().WithDefaultValues().Build();
            var legalBoardService = ServiceHelper.CreateLegalBoardService();

            // Act
            await legalBoardService.AddAsync(legalBoard);
            legalBoard.Name = "لجنة قضائية 2";

            var result = await legalBoardService.EditAsync(legalBoard);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(legalBoard.Name, result.Data.Name);
        }

        [Fact]
        public async void Delete_LegalBoard_Given_Valid_Information()
        {
            // Arrange
            var legalBoard = new LegalBoardBuilder().WithDefaultValues().Build();
            var legalBoardService = ServiceHelper.CreateLegalBoardService();

            // Act            
            var result = await legalBoardService.AddAsync(legalBoard);
            await legalBoardService.RemoveAsync(legalBoard.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Get_LegalBoard_By_Id_Given_Valid_Information()
        {
            // Arrange
            var legalBoard = new LegalBoardBuilder().WithDefaultValues().Build();
            var legalBoardService = ServiceHelper.CreateLegalBoardService();

            // Act
            await legalBoardService.AddAsync(legalBoard);
            var result = await legalBoardService.GetAsync(legalBoard.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(legalBoard.Id, result.Data.Id);
        }
        [Fact]
        public async void Activate_Legal_Board_Given_Valid_Information()
        {
            // Arrange
            var legalBoard = new LegalBoardBuilder().WithDefaultValues().Build();
            var legalBoardService = ServiceHelper.CreateLegalBoardService();

            // Act
            await legalBoardService.AddAsync(legalBoard);
            var result = await legalBoardService.ChangeStatusAsync(legalBoard.Id, (int)LegalBoardStatuses.Activated);

            // Assert
            Assert.True(result.IsSuccess);
        }
        [Fact]
        public async void Deactivate_Legal_Board_Given_Valid_Information()
        {
            // Arrange
            var legalBoard = new LegalBoardBuilder().WithDefaultValues().Build();
            var legalBoardService = ServiceHelper.CreateLegalBoardService();

            // Act
            await legalBoardService.AddAsync(legalBoard);
            var result = await legalBoardService.ChangeStatusAsync(legalBoard.Id, (int)LegalBoardStatuses.Unactivated);

            // Assert
            Assert.True(result.IsSuccess);
        }

    }
}
