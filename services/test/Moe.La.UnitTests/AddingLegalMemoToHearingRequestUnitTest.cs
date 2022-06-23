using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class AddingLegalMemoToHearingRequestUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Get_LegalMemoToHearingRequest_By_Id_Given_Valid_Information()
        {
            // Arrange
            var caseService = ServiceHelper.CreateCaseService();
            var caseDto = new CaseBuilder()
                .WithDefaultValues()
                .SecondSubCategoryId(1) // we have secondSubCategory onject from the seed data ServiceHelper.CommadDbContext.SecondSubCategories;
                .Build();
            await caseService.AddAsync(caseDto);

            var hearingService = ServiceHelper.CreateHearingService(TestUsers.LegalResearcherId);
            await hearingService.AddAsync(new HearingBuilder().WithDefaultValues().Build());


            var legalmomoToHearingRequestDto = new AddingLegalMemoToHearingRequestBuilder().WithDefaultValues().Build();
            var addingLegalMemoToHearingRequestService = ServiceHelper.CreateAddingLegalMemoToHearingRequestService(TestUsers.LegalResearcherId);

            var researcherConsultant = new ResearcherConsultantBuilder().WithDefaultValues().Build();
            var researcherConsultantService = ServiceHelper.CreateResearchsConsultantService(TestUsers.LegalResearcherId);
            await researcherConsultantService.AddAsync(researcherConsultant);

            // Act
            var res = await addingLegalMemoToHearingRequestService.AddAsync(legalmomoToHearingRequestDto);

            var result = await addingLegalMemoToHearingRequestService.GetAsync(res.Data.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Add_LegalMemoToHearingRequest_Given_Valid_Information()
        {
            // Arrange

            var caseService = ServiceHelper.CreateCaseService();
            var caseDto = new CaseBuilder()
                .WithDefaultValues()
                .SecondSubCategoryId(1) // we have secondSubCategory onject from the seed data ServiceHelper.CommadDbContext.SecondSubCategories;
                .Build();
            await caseService.AddAsync(caseDto);

            var hearingService = ServiceHelper.CreateHearingService(TestUsers.LegalResearcherId);
            await hearingService.AddAsync(new HearingBuilder().WithDefaultValues().Build());

            var legalmomoToHearingRequestDto = new AddingLegalMemoToHearingRequestBuilder().WithDefaultValues().Build();
            var addingLegalMemoToHearingRequestService = ServiceHelper.CreateAddingLegalMemoToHearingRequestService(TestUsers.LegalResearcherId);

            var researcherConsultant = new ResearcherConsultantBuilder().WithDefaultValues().Build();
            var researcherConsultantService = ServiceHelper.CreateResearchsConsultantService(TestUsers.LegalResearcherId);
            await researcherConsultantService.AddAsync(researcherConsultant);

            // Act
            var result = await addingLegalMemoToHearingRequestService.AddAsync(legalmomoToHearingRequestDto);

            // Assert
            Assert.True(result.IsSuccess);
        }
        [Fact]
        public async void Add_LegalMemoToHearingRequest_Given_Not_Valid_Researcher_With_No_Consultant_Information()
        {
            // Arrange
            var legalmomoToHearingRequestDto = new AddingLegalMemoToHearingRequestBuilder().WithDefaultValues().Build();
            var addingLegalMemoToHearingRequestService = ServiceHelper.CreateAddingLegalMemoToHearingRequestService(TestUsers.LegalResearcherId);

            // Act
            var result = await addingLegalMemoToHearingRequestService.AddAsync(legalmomoToHearingRequestDto);

            // Assert
            Assert.True(!result.IsSuccess);
        }
        [Fact]
        public async void Add_LegalMemoToHearingRequest_Given_Not_Valid_Hearing_With_Previous_Memos_Information()
        {
            // Arrange

            var caseService = ServiceHelper.CreateCaseService();
            var caseDto = new CaseBuilder()
                .WithDefaultValues()
                .SecondSubCategoryId(1) // we have secondSubCategory onject from the seed data ServiceHelper.CommadDbContext.SecondSubCategories;
                .Build();
            await caseService.AddAsync(caseDto);

            var hearingService = ServiceHelper.CreateHearingService(TestUsers.LegalResearcherId);
            await hearingService.AddAsync(new HearingBuilder().WithDefaultValues().Build());

            var legalmomoToHearingRequestDto = new AddingLegalMemoToHearingRequestBuilder().WithDefaultValues().Build();
            var addingLegalMemoToHearingRequestService = ServiceHelper.CreateAddingLegalMemoToHearingRequestService(TestUsers.LegalResearcherId);

            var researcherConsultant = new ResearcherConsultantBuilder().WithDefaultValues().Build();
            var researcherConsultantService = ServiceHelper.CreateResearchsConsultantService(TestUsers.LegalResearcherId);
            await researcherConsultantService.AddAsync(researcherConsultant);
            //Add Previous request   
            var res = await addingLegalMemoToHearingRequestService.AddAsync(legalmomoToHearingRequestDto);
            //Accept Request To Add Memo to Hearing         
            var replyAddingLegalMemoToHearingRequest = new AddingLegalMemoToHearingRequestBuilder().WithReplyAddingLegalMemoToHearingRequestDefaultValues().BuildReply();
            replyAddingLegalMemoToHearingRequest.Id = res.Data.Id;
            await addingLegalMemoToHearingRequestService.ReplyAddingMemoHearingRequest(replyAddingLegalMemoToHearingRequest);

            legalmomoToHearingRequestDto.LegalMemoId = 2;
            // Act
            //test  add another memo to the same hearing          
            var result = await addingLegalMemoToHearingRequestService.AddAsync(legalmomoToHearingRequestDto);

            // Assert
            Assert.True(!result.IsSuccess);
        }
    }
}
