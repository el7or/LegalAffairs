using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class RequestUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Add_Request_Given_Valid_Information()
        {
            // Arrange
            var request = new RequestBuilder().WithDefaultValues().Build();
            var service = ServiceHelper.CreateRequestService();

            // Act


            // Assert
        }

    }
}
