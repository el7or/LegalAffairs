using Moe.La.UnitTests.Builders;
using System.Threading.Tasks;
using Xunit;

namespace Moe.La.UnitTests
{
    public class ChangeResearcherToHearingRequestUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Add_Change_Reseacher_Request_Given_Valid_Information()
        {
            // Arrange
            await Arrange();
            var changeResearcherService = ServiceHelper.CreateChangeResearcherToHearingRequestService();
            var changeResearcherRequest = new ChangeResearcherToHearingRequestBuilder().WithDefaultValues().Build();

            // Act
            var result = await changeResearcherService.AddAsync(changeResearcherRequest);

            // Assert
            Assert.True(result.Data.Id > 0);
            Assert.Equal(result.Data.NewResearcherId, changeResearcherRequest.NewResearcherId);
        }

        [Fact]
        public async void Get_Change_Researcher_Request_By_Id_Given_Valid_Information()
        {
            // Arrange
            await Arrange();
            var changeResearcherService = ServiceHelper.CreateChangeResearcherToHearingRequestService();
            var changeResearcherRequest = new ChangeResearcherToHearingRequestBuilder().WithDefaultValues().Build();
            var newRequest = await changeResearcherService.AddAsync(changeResearcherRequest);

            // Act
            var result = await changeResearcherService.GetAsync(newRequest.Data.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newRequest.Data.Id, result.Data.Id);
        }

        [Fact]
        public async void Accept_Change_Researcher_Request_Given_Valid_Information()
        {
            // Arrange
            await Arrange();
            var changeResearcherService = ServiceHelper.CreateChangeResearcherToHearingRequestService();
            var changeResearcherRequest = new ChangeResearcherToHearingRequestBuilder().WithDefaultValues().Build();

            var newRequest = await changeResearcherService.AddAsync(changeResearcherRequest);
            var replyChangeResearcherRequest = new ReplyChangeResearcherToHearingRequestBuilder()
                .WithDefaultValues()
                .Id(newRequest.Data.Id)
                .CurrentResearcherId(TestUsers.LegalResearcherId)
                .NewResearcherId(TestUsers.LegalResearcherId2) // deferent user
                .Build();

            // Act
            var result = await changeResearcherService.AcceptChangeResearcherToHearingRequest(replyChangeResearcherRequest);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id == newRequest.Data.Id);
        }

        [Fact]
        public async void Accept_Change_Researcher_Request_Given_Invalid_Information()
        {
            // Arrange
            await Arrange();
            var changeResearcherService = ServiceHelper.CreateChangeResearcherToHearingRequestService();
            var changeResearcherRequest = new ChangeResearcherToHearingRequestBuilder().WithDefaultValues().Build();

            var newRequest = await changeResearcherService.AddAsync(changeResearcherRequest);
            var replyChangeResearcherRequest = new ReplyChangeResearcherToHearingRequestBuilder()
                .WithDefaultValues()
                .CurrentResearcherId(TestUsers.LegalResearcherId)
                .NewResearcherId(TestUsers.LegalResearcherId) // same user !!!
                .Id(newRequest.Data.Id)
                .Build();

            // Act
            var result = await changeResearcherService.AcceptChangeResearcherToHearingRequest(replyChangeResearcherRequest);

            // Assert
            Assert.True(result.IsSuccess == false);
        }

        [Fact]
        public async void Reject_Change_Researcher_Request_Given_Valid_Information()
        {
            // Arrange
            await Arrange();
            var changeResearcherService = ServiceHelper.CreateChangeResearcherToHearingRequestService();
            var changeResearcherRequest = new ChangeResearcherToHearingRequestBuilder().WithDefaultValues().Build();
            var newRequest = await changeResearcherService.AddAsync(changeResearcherRequest);
            var replyChangeResearcherRequest = new ReplyChangeResearcherToHearingRequestBuilder()
                .WithDefaultValues()
                .Id(newRequest.Data.Id)
                .Build();

            // Act
            var result = await changeResearcherService.RejectChangeResearcherToHearingRequest(replyChangeResearcherRequest);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id == newRequest.Data.Id);
        }

        /// <summary>
        /// 1- create a case
        /// 2- assign the researcher to a consultant
        /// 3- assing the researcher to the case
        /// 4- add hearing to the case
        /// </summary>
        /// <returns></returns>
        private async Task Arrange()
        {
            var mainCategory = new MainCategoryBuilder().WithDefaultValues().Build();
            var mainCategoryService = ServiceHelper.CreateMainCategoryService();
            await mainCategoryService.AddAsync(mainCategory);

            var firstSubCategory = new FirstSubCategoryBuilder().WithDefaultValues().Build();
            var firstSubCategoryService = ServiceHelper.CreateFirstSubCategoryService();
            firstSubCategory.MainCategoryId = mainCategory.Id;
            await firstSubCategoryService.AddAsync(firstSubCategory);

            var secondSubCategory = new SecondSubCategoryBuilder().WithDefaultValues().Build();
            secondSubCategory.MainCategory = new Core.Dtos.MainCategoryDto { Id = mainCategory.Id, Name = mainCategory.Name, CaseSource = mainCategory.CaseSource };
            secondSubCategory.FirstSubCategory = new Core.Dtos.FirstSubCategoryDto { Id = firstSubCategory.Id, Name = firstSubCategory.Name, MainCategoryId = mainCategory.Id };

            var secondSubCategoryService = ServiceHelper.CreateSecondSubCategoryService();
            await secondSubCategoryService.AddAsync(secondSubCategory);

            var caseDto = new CaseBuilder().WithDefaultValues().Build();
            caseDto.SecondSubCategoryId = secondSubCategory.Id.Value;
            var caseService = ServiceHelper.CreateCaseService();
            await caseService.AddAsync(caseDto);

            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder().WithDefaultValues().Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);

            var hearingDto = new HearingBuilder().WithDefaultValues().Build();
            var hearingService = ServiceHelper.CreateHearingService();
            hearingDto.CaseId = caseDto.Id;
            await hearingService.AddAsync(hearingDto);
        }
    }
}
