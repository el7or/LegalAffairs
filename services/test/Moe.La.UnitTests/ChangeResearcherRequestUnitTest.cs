using Moe.La.UnitTests.Builders;
using System.Threading.Tasks;
using Xunit;

namespace Moe.La.UnitTests
{
    public class ChangeResearcherRequestUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Add_Change_Reseacher_Request_Given_Valid_Information()
        {
            // Arrange
            await Arrange();
            var changeResearcherService = ServiceHelper.CreateChangeResearcherRequestService();
            var changeResearcherRequest = new ChangeResearcherRequestBuilder().WithDefaultValues().Build();

            // Act
            var result = await changeResearcherService.AddAsync(changeResearcherRequest);

            // Assert
            Assert.True(result.Data.Id > 0);
            Assert.Equal(result.Data.CurrentResearcherId, changeResearcherRequest.CurrentResearcherId);
        }

        [Fact]
        public async void Get_Change_Researcher_Request_By_Id_Given_Valid_Information()
        {
            // Arrange
            await Arrange();
            var changeResearcherService = ServiceHelper.CreateChangeResearcherRequestService();
            var changeResearcherRequest = new ChangeResearcherRequestBuilder().WithDefaultValues().Build();
            var newRequest = await changeResearcherService.AddAsync(changeResearcherRequest);

            // Act
            var result = await changeResearcherService.GetAsync(newRequest.Data.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newRequest.Data.Id, result.Data.Id);
        }

        //[Fact]
        //public async void Accept_Change_Researcher_Request_Given_Valid_Information()
        //{
        //    // Arrange
        //    await Arrange();
        //    var changeResearcherService = ServiceHelper.CreateChangeResearcherRequestService();
        //    var changeResearcherRequest = new ChangeResearcherRequestBuilder().WithDefaultValues().Build();

        //    var newRequest = await changeResearcherService.AddAsync(changeResearcherRequest);
        //    var replyChangeResearcherRequest = new ReplyChangeResearcherRequestBuilder()
        //        .WithDefaultValues()
        //        .Id(newRequest.Data.Id)
        //        .CurrentResearcherId(TestUsers.LegalResearcherId)
        //        .NewResearcherId(TestUsers.BoardMemberId) // deferent user
        //        .Build();

        //    // Act
        //    var result = await changeResearcherService.AcceptChangeResearcherRequest(replyChangeResearcherRequest);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.True(result.Data.Id == newRequest.Data.Id);
        //}

        [Fact]
        public async void Accept_Change_Researcher_Request_Given_Invalid_Information()
        {
            // Arrange
            await Arrange();
            var changeResearcherService = ServiceHelper.CreateChangeResearcherRequestService();
            var changeResearcherRequest = new ChangeResearcherRequestBuilder().WithDefaultValues().Build();

            var newRequest = await changeResearcherService.AddAsync(changeResearcherRequest);
            var replyChangeResearcherRequest = new ReplyChangeResearcherRequestBuilder()
                .WithDefaultValues()
                .CurrentResearcherId(TestUsers.LegalResearcherId)
                .NewResearcherId(TestUsers.LegalResearcherId) // same user !!!
                .Id(newRequest.Data.Id)
                .Build();

            // Act
            var result = await changeResearcherService.AcceptChangeResearcherRequest(replyChangeResearcherRequest);

            // Assert
            Assert.True(result.IsSuccess == false);
        }

        [Fact]
        public async void Reject_Change_Researcher_Request_Given_Valid_Information()
        {
            // Arrange
            await Arrange();
            var changeResearcherService = ServiceHelper.CreateChangeResearcherRequestService();
            var changeResearcherRequest = new ChangeResearcherRequestBuilder().WithDefaultValues().Build();
            var newRequest = await changeResearcherService.AddAsync(changeResearcherRequest);
            var replyChangeResearcherRequest = new ReplyChangeResearcherRequestBuilder()
                .WithDefaultValues()
                .Id(newRequest.Data.Id)
                .Build();

            // Act
            var result = await changeResearcherService.RejectChangeResearcherRequest(replyChangeResearcherRequest);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id == newRequest.Data.Id);
        }

        /// <summary>
        /// 1- create a case
        /// 2- assign the researcher to a consultant
        /// 3- assing the researcher to the case
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
            await ServiceHelper.CreateResearchsConsultantService()
                .AddAsync(new ResearcherConsultantBuilder().WithDefaultValues().Build());
            await caseService.AssignResearcherAsync(new CaseResearcherBuilder().WithDefaultValues().Build());
        }
    }
}
