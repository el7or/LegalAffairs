//using Moe.La.UnitTests.Builders;
//using Xunit;

//namespace Moe.La.UnitTests
//{
//    public class PartyTypeUnitTest : BaseUnitTest
//    {
//        [Fact]
//        public async void Create_New_PartyType_Given_Valid_Information()
//        {
//            // Arrange
//            var partyType = new PartyTypeBuilder().WithDefaultValues().Build();
//            var partyTypeService = ServiceHelper.CreatePartyTypeService();

//            // Act
//            var result = await partyTypeService.AddAsync(partyType);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.True(result.Data.Id > 0);
//        }

//        [Fact]
//        public async void Edit_PartyType_Given_Valid_Information()
//        {
//            // Arrange
//            var partyType = new PartyTypeBuilder().WithDefaultValues().Build();
//            var partyTypeService = ServiceHelper.CreatePartyTypeService();

//            // Act
//            await partyTypeService.AddAsync(partyType);
//            partyType.Name = "الباحة";

//            var result = await partyTypeService.EditAsync(partyType);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.Equal("الباحة", result.Data.Name);
//        }

//        [Fact]
//        public async void Delete_PartyType_Given_Valid_Information()
//        {
//            // Arrange
//            var partyType = new PartyTypeBuilder().WithDefaultValues().Build();
//            var partyTypeService = ServiceHelper.CreatePartyTypeService();

//            // Act            
//            var result = await partyTypeService.AddAsync(partyType);
//            await partyTypeService.RemoveAsync(partyType.Id);

//            // Assert
//            Assert.True(result.IsSuccess);
//        }

//        [Fact]
//        public async void Get_PartyType_By_Id_Given_Valid_Information()
//        {
//            // Arrange
//            var partyType = new PartyTypeBuilder().WithDefaultValues().Build();
//            var partyTypeService = ServiceHelper.CreatePartyTypeService();

//            // Act
//            await partyTypeService.AddAsync(partyType);
//            var result = await partyTypeService.GetAsync(partyType.Id);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.Equal(partyType.Id, result.Data.Id);
//        }
//    }
//}
