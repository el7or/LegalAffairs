using Moe.La.UnitTests.Builders.Requests;
using Xunit;

namespace Moe.La.UnitTests
{
    public class ExportCaseJudgmentRequestUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Add_Export_Case_Judgment_Request_Given_Valid_Information()
        {
            // Arrange
            var exportCaseJudgmentRequestService = ServiceHelper.CreateExportCaseJudgmentRequestService();
            var exportCaseJudgmentRequest = new ExportCaseJudgmentRequestBuilder().WithDefaultValues().Build();
            exportCaseJudgmentRequest.Request.Note = "Note";

            // Act
            var result = await exportCaseJudgmentRequestService.AddAsync(exportCaseJudgmentRequest);

            // Assert
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_Export_Case_Judgment_Request_Given_Valid_Information()
        {
            // Arrange
            var exportCaseJudgmentRequestService = ServiceHelper.CreateExportCaseJudgmentRequestService();
            var exportCaseJudgmentRequest = new ExportCaseJudgmentRequestBuilder().WithDefaultValues().Build();
            var newCaseRequest = await exportCaseJudgmentRequestService.AddAsync(exportCaseJudgmentRequest);
            exportCaseJudgmentRequest.Id = newCaseRequest.Data.Id;
            exportCaseJudgmentRequest.Request.Note = "This is a test reason 2";

            // Act
            var result = await exportCaseJudgmentRequestService.EditAsync(exportCaseJudgmentRequest);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(exportCaseJudgmentRequest.Request.Note, result.Data.Request.Note);
        }

        [Fact]
        public async void Get_Export_Case_Judgment_Request_By_Id_Given_Valid_Information()
        {
            // Arrange
            var exportCaseJudgmentRequestService = ServiceHelper.CreateExportCaseJudgmentRequestService();
            var exportCaseJudgmentRequest = new ExportCaseJudgmentRequestBuilder().WithDefaultValues().Build();
            var newRequest = await exportCaseJudgmentRequestService.AddAsync(exportCaseJudgmentRequest);

            // Act
            var result = await exportCaseJudgmentRequestService.GetAsync(newRequest.Data.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newRequest.Data.Id, result.Data.Id);
        }

        //[Fact]
        //public async void Accept_Case_Closing_Request_Given_Valid_Information()
        //{
        //    // Arrange
        //    var caseService = ServiceHelper.CreateCaseService();
        //    await caseService.AddAsync(new CaseBuilder().WithDefaultValues().Build());
        //    var caseClosingRequestService = ServiceHelper.CreateCaseClosingRequestService();
        //    var caseClosingRequest = new CaseClosingRequestBuilder().WithDefaultValues().Build();
        //    var newRequest = await caseClosingRequestService.AddAsync(caseClosingRequest);
        //    var replyCaseClosingRequest = new ReplyCaseClosingRequestBuilder()
        //        .WithDefaultValues()
        //        .RequestStatus(RequestStatuses.AcceptedFromConsultant)
        //        .Id(newRequest.Data.Id)
        //        .Build();

        //    // Act
        //    var result = await caseClosingRequestService.ReplyCaseClosingRequest(replyCaseClosingRequest);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.True(result.Data.Id == newRequest.Data.Id);
        //}

        //[Fact]
        //public async void Reject_Case_Closing_Request_Given_Valid_Information()
        //{
        //    // Arrange
        //    var caseClosingRequestService = ServiceHelper.CreateCaseClosingRequestService();
        //    var caseClosingRequest = new CaseClosingRequestBuilder().WithDefaultValues().Build();
        //    var newRequest = await caseClosingRequestService.AddAsync(caseClosingRequest);
        //    var replyCaseClosingRequest = new ReplyCaseClosingRequestBuilder()
        //        .RequestStatus(RequestStatuses.Rejected)
        //        .Id(newRequest.Data.Id)
        //        .Build();

        //    // Act
        //    var result = await caseClosingRequestService.ReplyCaseClosingRequest(replyCaseClosingRequest);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.True(result.Data.Id == newRequest.Data.Id);
        //}
    }
}
