using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class PartyUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_Party_Given_Valid_Information()
        {
            // Arrange
            var party = new PartyBuilder().WithDefaultValues().Build();
            var partyService = ServiceHelper.CreatePartyService();

            // Act
            var result = await partyService.AddAsync(party);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_Party_Given_Valid_Information()
        {
            // Arrange
            var party = new PartyBuilder().WithDefaultValues().Build();
            var partyService = ServiceHelper.CreatePartyService();

            // Act
            await partyService.AddAsync(party);
            party.Region = "جدة";
            party.IdentityTypeId = 1;
            party.IdentityValue = "1111111111";
            var result = await partyService.EditAsync(party);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("جدة", result.Data.Region);
            Assert.Equal("1111111111", result.Data.IdentityValue);
        }

        [Fact]
        public async void Delete_Part_Given_Valid_Information()
        {
            // Arrange
            var party = new PartyBuilder().WithDefaultValues().Build();
            var partyService = ServiceHelper.CreatePartyService();

            // Act
            var result = await partyService.AddAsync(party);
            await partyService.RemoveAsync(party.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Get_Party_By_Id_Given_Valid_Information()
        {
            // Arrange
            var party = new PartyBuilder().WithDefaultValues().Build();
            var partyService = ServiceHelper.CreatePartyService();

            // Act
            await partyService.AddAsync(party);
            var result = await partyService.GetAsync(party.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(party.Id, result.Data.Id);
        }
    }
}
