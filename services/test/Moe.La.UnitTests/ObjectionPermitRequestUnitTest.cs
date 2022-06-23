using Moe.La.UnitTests.Builders.Requests;
using Xunit;

namespace Moe.La.UnitTests
{
    public class ObjectionPermitRequestUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Add_Objectiom_Permit_Request_Given_Valid_Information()
        {
            // Arrange
            var objectionPermitRequestService = ServiceHelper.CreateObjectionPermitRequestService();
            var objectionPermitRequest = new ObjectionPermitRequestBuilder().WithDefaultValues().Build();

            // Act
            var result = await objectionPermitRequestService.AddAsync(objectionPermitRequest);

            // Assert
            Assert.True(result.Data.Id > 0);
        }
        [Fact]
        public async void Get_Objectiom_Permit_Request_By_Case_Id_Given_Valid_Information()
        {
            // Arrange
            var objectionPermitRequestService = ServiceHelper.CreateObjectionPermitRequestService();
            var objectionPermitRequest = new ObjectionPermitRequestBuilder().WithDefaultValues().Build();
            var newRequest = await objectionPermitRequestService.AddAsync(objectionPermitRequest);

            // Act
            var result = await objectionPermitRequestService.GetByCaseAsync(newRequest.Data.CaseId.Value);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newRequest.Data.Id, result.Data.Id);
        }


        [Fact]
        public async void Get_Objectiom_Permit_Request_By_Id_Given_Valid_Information()
        {
            // Arrange
            var objectionPermitRequestService = ServiceHelper.CreateObjectionPermitRequestService();
            var objectionPermitRequest = new ObjectionPermitRequestBuilder().WithDefaultValues().Build();
            var newRequest = await objectionPermitRequestService.AddAsync(objectionPermitRequest);

            // Act
            var result = await objectionPermitRequestService.GetAsync(newRequest.Data.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newRequest.Data.Id, result.Data.Id);
        }

        [Fact]
        public async void Reply_Objection_Permit_Request_Given_Valid_Information()
        {
            // Arrange
            var objectionPermitRequestService = ServiceHelper.CreateObjectionPermitRequestService();
            var objectionPermitRequest = new ObjectionPermitRequestBuilder().WithDefaultValues().Build();
            var newRequest = await objectionPermitRequestService.AddAsync(objectionPermitRequest);
            var replyObjectionPermitRequest = new ReplyObjectionPermitRequestBuilder().WithDefaultValues().Build();
            replyObjectionPermitRequest.Id = newRequest.Data.Id;

            var replyRequest = await objectionPermitRequestService.ReplyObjectionPermitRequest(replyObjectionPermitRequest);

            // Act
            var result = await objectionPermitRequestService.GetAsync(replyRequest.Data.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(replyRequest.Data.Id == result.Data.Id);
        }

    }
}
