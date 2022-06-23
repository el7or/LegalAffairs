using Moe.La.Core.Entities;
using Xunit;

namespace Moe.La.UnitTests
{
    public class UserUnitTest : BaseUnitTest
    {
        //[Fact]
        //public async void Get_User_By_Id_Given_Valid_Information()
        //{
        //    // Arrange
        //    var user = new UserBuilder().WithDefaultValues().Build();
        //    var userService = ServiceHelper.CreateUserService();

        //    // Act
        //    user = (await userService.AddAsync(user)).Data;
        //    var result = await userService.GetAsync(user.Id.Value);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.Equal(user.Id, result.Data.Id);
        //}


        [Fact]
        public async void Get_Users_List_Given_Valid_Information()
        {
            // Arrange
            var userService = ServiceHelper.CreateUserService();

            // Act
            var result = await userService.GetAllAsync(new UserQueryObject { BranchId = 1 });

            // Assert
            Assert.True(result.IsSuccess);
        }

        //[Fact]
        //public async void Create_New_User_Given_Valid_Information()
        //{
        //    // Arrange
        //    var roleDto = new RoleBuilder().WithDefaultValues().Build();
        //    var roleService = ServiceHelper.CreateRoleService();
        //    var role = await roleService.AddAsync(roleDto);

        //    var user = new UserBuilder().WithDefaultValues().Build();
        //    user.Roles = new List<string> { role.Data.Name };
        //    var userService = ServiceHelper.CreateUserService();

        //    // Act
        //    var result = await userService.AddAsync(user);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.True(result.Data.Id != Guid.Empty);
        //}

        //[Fact]
        //public async void Create_New_User_From_Active_Directory_Given_Valid_Information()
        //{
        //    // Arrange
        //    var roleDto = new RoleBuilder().WithDefaultValues().Build();
        //    var roleService = ServiceHelper.CreateRoleService();
        //    var role = await roleService.AddAsync(roleDto);

        //    var activeDirectoryService = ServiceHelper.CreateActiveDirectoryService();
        //    var userService = ServiceHelper.CreateUserService();
        //    var searchResult = await activeDirectoryService.GetAsync("1111111199");
        //    searchResult.Data.UserIdActiveDirectory = (System.Guid)searchResult.Data.Id;
        //    searchResult.Data.Roles = new List<string>() { role.Data.Name };

        //    // Act
        //    var result = await userService.AddAsync(searchResult.Data);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.True(result.Data.Id.HasValue);
        //}

        //[Fact]
        //public async void Edit_User_Given_Valid_Information()
        //{
        //    // Arrange
        //    var roleDto = new RoleBuilder().WithDefaultValues().Build();
        //    var roleService = ServiceHelper.CreateRoleService();
        //    var role = await roleService.AddAsync(roleDto);

        //    var user = new UserBuilder().WithDefaultValues().Build();
        //    user.Roles = new List<string>() { role.Data.Name };
        //    var userService = ServiceHelper.CreateUserService();

        //    // Act
        //    user = (await userService.AddAsync(user)).Data;
        //    user.FirstName = "user2";
        //    user.LastName = "last";

        //    var result = await userService.EditAsync(user);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.Equal("user2", result.Data.FirstName);
        //    Assert.Equal("last", result.Data.LastName);
        //}

        //[Fact]
        //public async void Delete_User_Given_Valid_Information()
        //{
        //    // Arrange
        //    var user = new UserBuilder().WithDefaultValues().Build();
        //    var userService = ServiceHelper.CreateUserService();

        //    // Act            
        //    user = (await userService.AddAsync(user)).Data;
        //    var result = await userService.RemoveAsync(user.Id.Value);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //}
    }
}
