using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class ResearchsConsultantUnitTest : BaseUnitTest
    {

        [Fact]
        public async void Create_New_Researcher_Consultant_Given_Valid_Information()
        {
            // Arrange
            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder().WithDefaultValues().Build();

            // Act
            var result = await researchsConsultantService.AddAsync(researcherConsultantDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Create_New_Researcher_Consultant_End_Consultant_Information()
        {
            // Arrange
            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder().WithDefaultValues().ConsultantId(null).Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);

            // Act
            var result = await researchsConsultantService.AddAsync(researcherConsultantDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id == 0);
        }

        [Fact]
        public async void Create_New_Researcher_Consultant_Deferent_Department_Id_Information()
        {
            // Arrange

            //var roleService = ServiceHelper.CreateRoleService();

            //await roleService.RemoveAsync("LegalResearcher");
            //await roleService.RemoveAsync("LegalConsultant");

            //var researcherRoleDto = new RoleBuilder().WithDefaultValues().Name("LegalResearcher").Build();
            //var consultantRoleDto = new RoleBuilder().WithDefaultValues().Name("LegalConsultant").Build();

            //var researcherRole = await roleService.AddAsync(researcherRoleDto);
            //var consultantRole = await roleService.AddAsync(consultantRoleDto);

            //var researcher = new UserBuilder().WithDefaultValues()
            //    .LegalAffairsDepartmentId(1) // deferent department
            //    .UserName("Researcher") // deferent username
            //    .Roles(new List<string> { researcherRole.Data.Name })
            //    .Build();

            //var consultant = new UserBuilder().WithDefaultValues()
            //    .LegalAffairsDepartmentId(2) // deferent department
            //    .UserName("Consultant") // deferent username
            //    .Roles(new List<string> { consultantRole.Data.Name })
            //    .Build();

            //var userService = ServiceHelper.CreateUserService(TestUsers.LitigationManagerId);
            //researcher = (await userService.AddAsync(researcher)).Data;
            //consultant = (await userService.AddAsync(consultant)).Data;

            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();

            var researcherConsultantDto = new ResearcherConsultantBuilder()
                .ResearcherId(TestUsers.LegalResearcherId2)
                .ConsultantId(TestUsers.LegalConsultantId)
                .Build();

            await researchsConsultantService.AddAsync(researcherConsultantDto);

            // Act
            var result = await researchsConsultantService.AddAsync(researcherConsultantDto);

            // Assert
            Assert.True(result.IsSuccess == false);
        }


        //[Fact]
        //public async void Create_New_Researcher_Consultant_Deferent_Department_Id_Information()
        //{ 
        //    var researcherRoleDto = new RoleBuilder().WithDefaultValues().Name("LegalResearcher").Build();
        //    var consultantRoleDto = new RoleBuilder().WithDefaultValues().Name("LegalConsultant").Build();

        //    var roleService = ServiceHelper.CreateRoleService();
        //    var researcherRole = await roleService.AddAsync(researcherRoleDto);
        //    var consultantRole = await roleService.AddAsync(consultantRoleDto);

        //    // Arrange
        //    var researcher = new UserBuilder().WithDefaultValues()
        //        .LegalAffairsDepartmentId(1) // deferent department
        //        .UserName("Researcher") // deferent username
        //        .Roles(new List<string> { researcherRole.Data.Name })
        //        .Build();

        //    var consultant = new UserBuilder().WithDefaultValues()
        //        .LegalAffairsDepartmentId(2) // deferent department
        //        .UserName("Consultant") // deferent username
        //        .Roles(new List<string> { consultantRole.Data.Name })
        //        .Build();

        //    var userService = ServiceHelper.CreateUserService(TestUsers.AdminId);
        //    var researcherEntity = await userService.AddAsync(researcher);
        //    var consultantEntity = await userService.AddAsync(consultant);

        //    var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();

        //    var researcherConsultantDto = new ResearcherConsultantBuilder()
        //        .ResearcherId((Guid)researcherEntity.Data.Id)
        //        .ConsultantId((Guid)consultantEntity.Data.Id)
        //        .Build();

        //    await researchsConsultantService.AddAsync(researcherConsultantDto);

        //    // Act
        //    var result = await researchsConsultantService.AddAsync(researcherConsultantDto);

        //    // Assert
        //    Assert.True(result.IsSuccess == false);
        //}

        [Fact]
        public async void Create_New_Researcher_Consultant_With_His_Same_Active_Consultant_Information()
        {
            // Arrange
            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder().WithDefaultValues().Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);

            // Act
            var result = await researchsConsultantService.AddAsync(researcherConsultantDto);

            // Assert
            Assert.False(result.IsSuccess);
        }


        [Fact]
        public async void Get_Researcher_Consultant_By_Id_Given_Valid_Information()
        {
            // Arrange
            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder().WithDefaultValues().Build();

            // Act
            var createResult = await researchsConsultantService.AddAsync(researcherConsultantDto);
            var result = await researchsConsultantService.GetAsync(createResult.Data.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(createResult.Data.Id, result.Data.Id);
        }
    }
}
