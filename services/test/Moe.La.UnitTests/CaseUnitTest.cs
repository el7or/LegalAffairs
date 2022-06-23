using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class CaseUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Add_Case_Given_Valid_Information()
        {
            // Arrange
            var caseDto = new CaseBuilder().WithDefaultValues()
                .SecondSubCategoryId(1)
                .Build();
            var service = ServiceHelper.CreateCaseService();

            // Act
            var result = await service.AddAsync(caseDto);

            // Assert
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_Case_Given_Valid_Information()
        {
            // Arrange
            //var secondSubCategoryDto = new SecondSubCategoryBuilder().WithDefaultValues().Build();
            //var secondSubCategoryService = ServiceHelper.CreateSecondSubCategoryService();
            //var secondSubCategory = await secondSubCategoryService.AddAsync(secondSubCategoryDto);

            var caseDto = new CaseBuilder()
                .WithDefaultValues()
                .SecondSubCategoryId(1) // we have secondSubCategory onject from the seed data ServiceHelper.CommadDbContext.SecondSubCategories;
                .Build();
            var caseService = ServiceHelper.CreateCaseService();
            await caseService.AddAsync(caseDto);

            // Act
            //caseDto.OrderDescription = "test order description";
            caseDto.CaseDescription = "test case description";

            var result = await caseService.EditAsync(caseDto);

            // Assert
            Assert.True(result.IsSuccess);
            //Assert.Equal("test order description", result.Data.OrderDescription);
            Assert.Equal("test case description", result.Data.CaseDescription);
        }

        [Fact]
        public async void Get_Case_By_Id_Given_Valid_Information()
        {
            // Arrange
            //var secondSubCategoryDto = new SecondSubCategoryBuilder().WithDefaultValues().Build();
            //var secondSubCategoryService = ServiceHelper.CreateSecondSubCategoryService();
            //var secondSubCategory = await secondSubCategoryService.AddAsync(secondSubCategoryDto);

            var caseDto = new CaseBuilder()
                .WithDefaultValues()
                .SecondSubCategoryId(1) // we have secondSubCategory onject from the seed data ServiceHelper.CommadDbContext.SecondSubCategories;
                .Build();
            var service = ServiceHelper.CreateCaseService();

            // Act
            await service.AddAsync(caseDto);
            var result = await service.GetAsync(caseDto.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(caseDto.Id, result.Data.Id);
        }

        [Fact]
        public async void Get_Cases_List_Given_Valid_Search_Information()
        {
            // Arrange
            var service = ServiceHelper.CreateCaseService();

            // Act
            var result = await service.GetAllAsync(new CaseQueryObject { CaseSource = 1 });

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Get_Cases_List_Given_Valid_Sort_Information()
        {
            // Arrange
            var service = ServiceHelper.CreateCaseService();

            // Act
            var result = await service.GetAllAsync(new CaseQueryObject { ReceivedStatus = 1 });

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Delete_Case_Given_Valid_Information()
        {
            // Arrange
            var caseDto = new CaseBuilder().WithDefaultValues()
                .SecondSubCategoryId(1)
                .Build();
            var service = ServiceHelper.CreateCaseService();

            // Act
            var result = await service.AddAsync(caseDto);
            await service.RemoveAsync(caseDto.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Assign_Researcher_With_No_Consultant_Information()
        {
            // Arrange
            var caseDto = new CaseBuilder().WithDefaultValues()
                .SecondSubCategoryId(1)
                .Build();
            var caseService = ServiceHelper.CreateCaseService();
            var newCase = await caseService.AddAsync(caseDto);


            var caseResearchersDto = new CaseResearcherBuilder()
                .WithDefaultValues()
                .CaseId(newCase.Data.Id)
                .Build();

            // Act
            var result = caseService.AssignResearcherAsync(caseResearchersDto);

            // Assert
            Assert.True(!result.Result.IsSuccess);
        }

        [Fact]
        public async void Assign_Researcher_Valid_Information()
        {
            // Arrange
            //var secondSubCategoryDto = new SecondSubCategoryBuilder().WithDefaultValues().Build();
            //var secondSubCategoryService = ServiceHelper.CreateSecondSubCategoryService();
            //var secondSubCategory = await secondSubCategoryService.AddAsync(secondSubCategoryDto);

            var caseDto = new CaseBuilder()
                .WithDefaultValues()
                .SecondSubCategoryId(1) // we have secondSubCategory onject from the seed data ServiceHelper.CommadDbContext.SecondSubCategories;
                .Build();

            var caseService = ServiceHelper.CreateCaseService();
            var newCase = await caseService.AddAsync(caseDto);

            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder().WithDefaultValues().Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);

            var caseResearchersDto = new CaseResearcherBuilder()
                .WithDefaultValues()
                .CaseId(newCase.Data.Id)
                .Build();

            // Act
            var result = caseService.AssignResearcherAsync(caseResearchersDto);

            // Assert
            Assert.True(result.Result.IsSuccess);
        }

        [Fact]
        public async void Change_Status_Valid_Information()
        {
            // Arrange
            //var secondSubCategoryDto = new SecondSubCategoryBuilder().WithDefaultValues().Build();
            //var secondSubCategoryService = ServiceHelper.CreateSecondSubCategoryService();
            //var secondSubCategory = await secondSubCategoryService.AddAsync(secondSubCategoryDto);

            var caseDto = new CaseBuilder()
                .WithDefaultValues()
                .SecondSubCategoryId(1) // we have secondSubCategory onject from the seed data ServiceHelper.CommadDbContext.SecondSubCategories;
                .Build();
            var service = ServiceHelper.CreateCaseService();

            // Act
            await service.AddAsync(caseDto);

            var caseStatus = new CaseChangeStatusDto
            {
                Id = caseDto.Id,
                Status = CaseStatuses.DoneJudgment,
                Note = ""
            };
            var result = await service.ChangeStatusAsync(caseStatus);

            // Assert
            Assert.True(result.IsSuccess);
            //Assert.Equal(caseDto.Id, result.Data.Id);
            //Assert.Equal(CaseStatuses.DoneJudgment, (CaseStatuses)result.Data.Status.Id);
        }

        [Fact]
        public async void Get_Parent_Given_Valid_Information()
        {
            // Arrange
            //var secondSubCategoryDto = new SecondSubCategoryBuilder().WithDefaultValues().Build();
            //var secondSubCategoryService = ServiceHelper.CreateSecondSubCategoryService();
            //var secondSubCategory = await secondSubCategoryService.AddAsync(secondSubCategoryDto);

            var caseService = ServiceHelper.CreateCaseService(TestUsers.LegalResearcherId);
            var caseDto = new CaseBuilder()
                .WithDefaultValues()
                .SecondSubCategoryId(1) // we have secondSubCategory onject from the seed data ServiceHelper.CommadDbContext.SecondSubCategories;
                .Build();
            var newCase = await caseService.AddAsync(caseDto);
            ///
            var researcherConsultantDto = new ResearcherConsultantBuilder().WithDefaultValues().Build();
            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            await researchsConsultantService.AddAsync(researcherConsultantDto);

            var caseResearcherDto = new CaseResearcherBuilder().WithDefaultValues().Build();
            await caseService.AssignResearcherAsync(caseResearcherDto);
            var parentCaseDto = new CaseBuilder().WithDefaultValues().Build();
            //add related case
            parentCaseDto.RelatedCaseId = newCase.Data.Id;
            parentCaseDto.SecondSubCategoryId = 1; // we have secondSubCategory onject from the seed data ServiceHelper.CommadDbContext.SecondSubCategories;
            var relatedCase = await caseService.AddAsync(parentCaseDto);


            // Act     
            var result = await caseService.GetParentCaseAsync(newCase.Data.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

    }
}
