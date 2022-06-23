using Moe.La.Core.Entities;
using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class HearingUnitTest : BaseUnitTest
    {

        [Fact]
        public async void Add_Hearing_Given_Valid_Information()
        {
            // Arrange
            //var secondSubCategoryDto = new SecondSubCategoryBuilder().WithDefaultValues().Build();
            //var secondSubCategoryService = ServiceHelper.CreateSecondSubCategoryService();
            //var secondSubCategory = await secondSubCategoryService.AddAsync(secondSubCategoryDto);

            var service = ServiceHelper.CreateCaseService();
            var caseDto = new CaseBuilder()
                .WithDefaultValues()
                .SecondSubCategoryId(1) // we have secondSubCategory onject from the seed data ServiceHelper.CommadDbContext.SecondSubCategories;
                .Build();
            var caseResult = await service.AddAsync(caseDto);

            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder().WithDefaultValues().Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);

            var hearingDto = new HearingBuilder().WithDefaultValues().Build();
            var hearingService = ServiceHelper.CreateHearingService();
            hearingDto.CaseId = caseResult.Data.Id;

            // Act
            var result = await hearingService.AddAsync(hearingDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_Hearing_Given_Valid_Information()
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
            var caseResult = await service.AddAsync(caseDto);

            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder().WithDefaultValues().Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);

            var hearingDto = new HearingBuilder().WithDefaultValues().Build();
            var hearingService = ServiceHelper.CreateHearingService();
            hearingDto.CaseId = caseResult.Data.Id;

            // Act
            await hearingService.AddAsync(hearingDto);
            hearingDto.HearingNumber = 999;

            var result = await hearingService.EditAsync(hearingDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(999, result.Data.HearingNumber);
        }

        [Fact]
        public async void Get_Hearing_By_Id_Given_Valid_Information()
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
            var caseResult = await caseService.AddAsync(caseDto);

            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder().WithDefaultValues().Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);

            var hearingDto = new HearingBuilder().WithDefaultValues().Build();
            var hearingService = ServiceHelper.CreateHearingService();
            hearingDto.CaseId = caseResult.Data.Id;

            // Act
            var newHearing = await hearingService.AddAsync(hearingDto);
            var result = await hearingService.GetAsync(newHearing.Data.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(hearingDto.Id, result.Data.Id);
        }

        [Fact]
        public async void Get_Hearing_List_Given_Valid_Search_Information()
        {
            // Arrange
            var service = ServiceHelper.CreateHearingService();

            // Act
            var result = await service.GetAllAsync(new HearingQueryObject { CaseId = 1 });

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Assign_Hearing_To_User_Given_Valid_Information()
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
            var _case = await service.AddAsync(caseDto);

            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder().WithDefaultValues().Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);

            var hearingDto = new HearingBuilder().WithDefaultValues().Build();
            var hearingService = ServiceHelper.CreateHearingService();
            hearingDto.CaseId = _case.Data.Id;
            var hearing = await hearingService.AddAsync(hearingDto);

            // Act
            var result = await hearingService.AssignHearingToAsync(hearing.Data.Id, TestUsers.LegalResearcherId2);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
