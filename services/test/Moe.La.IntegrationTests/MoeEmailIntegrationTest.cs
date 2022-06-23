using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moe.La.Integration;
using Moe.La.Integration.Options;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Moe.La.IntegrationTests
{
    public class MoeEmailIntegrationTest
    {
        private readonly IOptions<MoeEmailOptions> _options;

        public MoeEmailIntegrationTest()
        {
            var options = new MoeEmailOptions()
            {
                Endpoint = "http://msbstg.moe.gov.sa/EmailRESTEndpoint/Service1.svc/SendEmail",
                Username = "Morafa3a",
                Password = "Morafa3aEmail9485@",
                From = "no_replay@moe.gov.sa"
            };

            _options = Options.Create(options);
        }

        [Fact]
        public async void Send_Email_With_To_Should_Success()
        {
            // Arrange
            var moeEmailService = new MoeEmailIntegrationService(_options, new Mock<ILogger<MoeEmailIntegrationService>>().Object);

            // Act
            var result = await moeEmailService.SendAsync("m.helal@smart-fingers.sa", "Test subject", "Test body");

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Send_Email_With_To_CC_Should_Success()
        {
            // Arrange
            var moeEmailService = new MoeEmailIntegrationService(_options, new Mock<ILogger<MoeEmailIntegrationService>>().Object);
            var to = new List<string> { "m.helal@smart-fingers.sa", "nabdulraheem@moe.gov.sa" };
            var cc = new List<string> { "m.helal@smart-fingers.sa", "nabdulraheem@moe.gov.sa" };

            // Act
            var result = await moeEmailService.SendAsync(to, cc, "Test subject", "Test body");

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Send_Email_With_To_CC_BCC_Should_Success()
        {
            // Arrange
            var moeEmailService = new MoeEmailIntegrationService(_options, new Mock<ILogger<MoeEmailIntegrationService>>().Object);
            var to = new List<string> { "m.helal@smart-fingers.sa", "nabdulraheem@moe.gov.sa" };
            var cc = new List<string> { "m.helal@smart-fingers.sa", "nabdulraheem@moe.gov.sa" };
            var bcc = new List<string> { "m.helal@smart-fingers.sa", "nabdulraheem@moe.gov.sa" };

            // Act
            var result = await moeEmailService.SendAsync(to, cc, bcc, "Test subject", "Test body");

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Send_Email_To_Invalid_Email_Address_Should_Fail()
        {
            // Arrange
            var moeEmailService = new MoeEmailIntegrationService(_options, new Mock<ILogger<MoeEmailIntegrationService>>().Object);

            // Act
            var result = await moeEmailService.SendAsync("123smart-fingers.sa", "Test subject", "Test body");

            // Assert
            Assert.True(!result.IsSuccess);
        }

        [Fact]
        public async void Send_Email_With_Invalid_Parameters_Should_Through_Exception()
        {
            // Arrange
            var moeEmailService = new MoeEmailIntegrationService(_options, new Mock<ILogger<MoeEmailIntegrationService>>().Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await moeEmailService.SendAsync("", "", ""));
        }

        [Fact]
        public async void Send_Email_With_Invalid_Parameters_Should_Through_Exception_2()
        {
            // Arrange
            var moeEmailService = new MoeEmailIntegrationService(_options, new Mock<ILogger<MoeEmailIntegrationService>>().Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await moeEmailService.SendAsync(new List<string>(), null, null));
        }

        [Fact]
        public async void Send_Email_With_Invalid_Parameters_Should_Through_Exception_3()
        {
            // Arrange
            var moeEmailService = new MoeEmailIntegrationService(_options, new Mock<ILogger<MoeEmailIntegrationService>>().Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await moeEmailService.SendAsync(new List<string>(), null, null, null));
        }

        [Fact]
        public async void Send_Email_With_Invalid_Parameters_Should_Through_Exception_4()
        {
            // Arrange
            var moeEmailService = new MoeEmailIntegrationService(_options, new Mock<ILogger<MoeEmailIntegrationService>>().Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await moeEmailService.SendAsync(null, null, null, null, null));
        }

        [Fact]
        public async void Send_Email_With_Invalid_Parameters_Should_Through_Exception_5()
        {
            // Arrange
            var moeEmailService = new MoeEmailIntegrationService(_options, new Mock<ILogger<MoeEmailIntegrationService>>().Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await moeEmailService.SendAsync(new List<string>(), new List<string>(), new List<string>(), null, null));
        }
    }
}
