using Moe.La.Core.Enums;
using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class MoamalaUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Get_Moamala_By_Id_Given_Valid_Information()
        {
            // Arrange
            var moamalaDetailsDto = new MoamalaBuilder().WithDefaultValues().Build();
            var service = ServiceHelper.CreateMoamalaService(TestUsers.DistributorId);

            // Act
            var entityAdded = await service.AddAsync(moamalaDetailsDto);
            var result = await service.GetAsync(entityAdded.Data.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(moamalaDetailsDto.Id, result.Data.Id);
        }
        [Fact]
        public async void Get_Moamala_By_Id_Given_Invalid_Information()
        {
            // Arrange
            var service = ServiceHelper.CreateMoamalaService(TestUsers.DistributorId);

            // Act 
            var result = await service.GetAsync(5);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async void Referral_Moamala_To_Branch_Valid_Information()
        {
            // Arrange
            var moamalaDetailsDto = new MoamalaBuilder()
                .WithDefaultValues()
                .ReferralNote("Referral Note")
                .Build();

            var moamalaChangeStatus = new MoamalaChangeStatusBuilder()
                .WithDefaultValues()
                .BranchId(1)
                .Build();
            var service = ServiceHelper.CreateMoamalaService();

            // Act
            await service.AddAsync(moamalaDetailsDto);
            var result = await service.ChangeStatusAsync(moamalaChangeStatus);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Assign_Moamala_Invalid_Information()
        {
            // Arrange
            var moamalaDetailsDto = new MoamalaBuilder()
                .WithDefaultValues()
                .ReferralNote("Referral Note")
                .Build();

            var moamalaChangeStatus = new MoamalaChangeStatusBuilder()
                .WithDefaultValues()
                .Status(MoamalaStatuses.Assigned)
                .AssignedToId(null)
                .Build();
            var service = ServiceHelper.CreateMoamalaService();

            // Act
            await service.AddAsync(moamalaDetailsDto);
            var result = await service.ChangeStatusAsync(moamalaChangeStatus);

            // Assert
            Assert.False(result.IsSuccess);
        }
        [Fact]
        public async void Referral_Moamala_To_Department_Valid_Information()
        {
            // Arrange
            var moamalaDetailsDto = new MoamalaBuilder()
                .WithDefaultValues()
                .ReferralNote("Referral Note")
                .Build();

            var moamalaChangeStatus = new MoamalaChangeStatusBuilder()
                .WithDefaultValues()
                .BranchId(1)
                .DepartmentId(1)
                .Build();
            var service = ServiceHelper.CreateMoamalaService();

            // Act
            await service.AddAsync(moamalaDetailsDto);
            var result = await service.ChangeStatusAsync(moamalaChangeStatus);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Assign_Moamala_Given_Valid_Information()
        {
            // Arrange
            var moamalaDetailsDto = new MoamalaBuilder().WithDefaultValues().Build();

            var moamalaChangeStatus = new MoamalaChangeStatusBuilder()
                .WithDefaultValues()
                .Status(MoamalaStatuses.Assigned)
                .AssignedToId(TestUsers.LegalResearcherId)
                .Build();
            var service = ServiceHelper.CreateMoamalaService();

            // Act
            await service.AddAsync(moamalaDetailsDto);
            var result = await service.ChangeStatusAsync(moamalaChangeStatus);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Return_Assigned_Moamala_Given_Valid_Information()
        {
            // Arrange
            var moamalaDetailsDto = new MoamalaBuilder().WithDefaultValues().Build();

            var moamalaChangeStatus = new MoamalaChangeStatusBuilder()
                .WithDefaultValues()
                .Status(MoamalaStatuses.Assigned)
                .AssignedToId(TestUsers.LegalResearcherId)
                .Build();
            var service = ServiceHelper.CreateMoamalaService();

            // Act
            await service.AddAsync(moamalaDetailsDto);
            await service.ChangeStatusAsync(moamalaChangeStatus);
            var result = await service.ReturnAsync(moamalaDetailsDto.Id, "Return note");

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Return_Branch_Moamala_Given_Valid_Information()
        {
            // Arrange
            var moamalaDetailsDto = new MoamalaBuilder().WithDefaultValues().Build();

            var moamalaChangeStatus = new MoamalaChangeStatusBuilder()
                .WithDefaultValues()
                .Status(MoamalaStatuses.Referred)
                .BranchId(1)
                .Build();
            var service = ServiceHelper.CreateMoamalaService();

            // Act
            await service.AddAsync(moamalaDetailsDto);
            await service.ChangeStatusAsync(moamalaChangeStatus);
            var result = await service.ReturnAsync(moamalaDetailsDto.Id, "Return note");

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Return_Department_Moamala_Given_Valid_Information()
        {
            // Arrange
            var moamalaDetailsDto = new MoamalaBuilder().WithDefaultValues().Build();

            var moamalaChangeStatus = new MoamalaChangeStatusBuilder()
                .WithDefaultValues()
                .Status(MoamalaStatuses.Referred)
                .BranchId(1)
                .DepartmentId(1)
                .Build();
            var service = ServiceHelper.CreateMoamalaService();

            // Act
            await service.AddAsync(moamalaDetailsDto);
            await service.ChangeStatusAsync(moamalaChangeStatus);
            var result = await service.ReturnAsync(moamalaDetailsDto.Id, "Return note");

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
