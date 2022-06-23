//using Moe.La.UnitTests.Builders;
//using Xunit;

//namespace Moe.La.UnitTests
//{
//    public class RoleUnitTest : BaseUnitTest
//    {
//        [Fact]
//        public async void Create_New_Role_Given_Valid_Information()
//        {
//            // Arrange
//            var role = new RoleBuilder().WithDefaultValues().Build();
//            var roleService = ServiceHelper.CreateRoleService();

//            // Act
//            var result = await roleService.AddAsync(role);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.True(result.Data.Id != null);
//        }

//        [Fact]
//        public async void Edit_Role_Given_Valid_Information()
//        {
//            // Arrange
//            var role = new RoleBuilder().WithDefaultValues().Build();
//            var roleService = ServiceHelper.CreateRoleService();

//            // Act
//            role = (await roleService.AddAsync(role)).Data;
//            role.Name = "role2";
//            role.NameAr = "اختبار";

//            var result = await roleService.EditAsync(role);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.Equal("role2", result.Data.Name);
//            Assert.Equal("اختبار", result.Data.NameAr);
//        }

//        [Fact]
//        public async void Delete_Role_Given_Valid_Information()
//        {
//            // Arrange
//            var role = new RoleBuilder().WithDefaultValues().Build();
//            var roleService = ServiceHelper.CreateRoleService();

//            // Act            
//            var result = await roleService.AddAsync(role);
//            await roleService.RemoveAsync(role.Id);

//            // Assert
//            Assert.True(result.IsSuccess);
//        }

//        [Fact]
//        public async void Get_Role_By_Id_Given_Valid_Information()
//        {
//            // Arrange
//            var role = new RoleBuilder().WithDefaultValues().Build();
//            var roleService = ServiceHelper.CreateRoleService();

//            // Act
//            await roleService.AddAsync(role);
//            var result = await roleService.GetAsync(role.Id);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.Equal(role.Id, result.Data.Id);
//        }
//    }
//}
