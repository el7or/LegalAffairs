//using Moe.La.UnitTests.Builders;
//using Xunit;

//namespace Moe.La.UnitTests
//{
//    public class FieldMissionTypeUnitTest : BaseUnitTest
//    {
//        [Fact]
//        public async void Create_New_FieldMissionType_Given_Valid_Information()
//        {
//            // Arrange
//            var fieldMissionType = new FieldMissionTypeBuilder().WithDefaultValues().Build();
//            var fieldMissionTypeService = ServiceHelper.CreateFieldMissionTypeService();

//            // Act
//            var result = await fieldMissionTypeService.AddAsync(fieldMissionType);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.True(result.Data.Id > 0);
//        }

//        [Fact]
//        public async void Edit_FieldMissionType_Given_Valid_Information()
//        {
//            // Arrange
//            var fieldMissionType = new FieldMissionTypeBuilder().WithDefaultValues().Build();
//            var fieldMissionTypeService = ServiceHelper.CreateFieldMissionTypeService();

//            // Act
//            await fieldMissionTypeService.AddAsync(fieldMissionType);
//            fieldMissionType.Name = "FieldMissionTypetestedit";

//            var result = await fieldMissionTypeService.EditAsync(fieldMissionType);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.Equal("FieldMissionTypetestedit", result.Data.Name);
//        }

//        [Fact]
//        public async void Delete_FieldMissionType_Given_Valid_Information()
//        {
//            // Arrange
//            var fieldMissionType = new FieldMissionTypeBuilder().WithDefaultValues().Build();
//            var fieldMissionTypeService = ServiceHelper.CreateFieldMissionTypeService();

//            // Act            
//            var result = await fieldMissionTypeService.AddAsync(fieldMissionType);
//            await fieldMissionTypeService.RemoveAsync(fieldMissionType.Id);

//            // Assert
//            Assert.True(result.IsSuccess);
//        }

//        [Fact]
//        public async void Get_FieldMissionType_By_Id_Given_Valid_Information()
//        {
//            // Arrange
//            var fieldMissionType = new FieldMissionTypeBuilder().WithDefaultValues().Build();
//            var fieldMissionTypeService = ServiceHelper.CreateFieldMissionTypeService();

//            // Act
//            await fieldMissionTypeService.AddAsync(fieldMissionType);
//            var result = await fieldMissionTypeService.GetAsync(fieldMissionType.Id);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.Equal(fieldMissionType.Id, result.Data.Id);
//        }
//    }
//}
