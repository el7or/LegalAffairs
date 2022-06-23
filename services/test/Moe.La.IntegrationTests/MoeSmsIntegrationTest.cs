using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moe.La.Integration;
using Moe.La.Integration.Options;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Moe.La.IntegrationTests
{
    public class MoeSmsIntegrationTest
    {
        private readonly IOptions<MoeSmsOptions> _options;

        public MoeSmsIntegrationTest()
        {
            var options = new MoeSmsOptions()
            {
                Endpoint = "https://msbstg.moe.gov.sa/SMSRESTEndpoint/Service1.svc/SendMoheSMS",
                Username = "Morafa3a",
                Password = "Morafa3aSMS1784@",
                Sender = "MOE",
                CheckActivation = false,
            };

            _options = Options.Create(options);
        }

        [Fact]
        public async void Send_SMS_With_Valid_Information_Should_Success()
        {
            // Arrange
            var moeSmsService = new MoeSmsIntegrationService(_options, new Mock<ILogger<MoeSmsIntegrationService>>().Object);

            // Act
            var result = await moeSmsService.SendAsync("502297717", "رسالة تجريبية من نظام مرافعة!");

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Send_SMS_To_Multiple_With_Valid_Information_Should_Success()
        {
            // Arrange
            var moeSmsService = new MoeSmsIntegrationService(_options, new Mock<ILogger<MoeSmsIntegrationService>>().Object);
            var numbers = new List<string> { "502297717" };
            // Act
            var result = await moeSmsService.SendAsync(numbers, "رسالة تجريبية من نظام مرافعة!");

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
