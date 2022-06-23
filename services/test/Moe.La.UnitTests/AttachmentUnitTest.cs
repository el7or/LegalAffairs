using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class AttachmentUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Get_Attachments_List_Given_Valid_Search_Information()
        {
            // Arrange
            var service = ServiceHelper.CreateAttachmentService();

            // Act
            var result = await service.GetAllAsync(new AttachmentQueryObject { GroupName = GroupNames.Case, GroupId = 1 });

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Create_New_Attachment_Given_Valid_Information()
        {
            // Arrange
            var attachment = new AttachmentBuilder().WithDefaultValues().Build();
            var attachmentService = ServiceHelper.CreateAttachmentService();

            // Act
            var result = await attachmentService.AddAsync(attachment);

            // Assert
            Assert.True(result.IsSuccess);
            //Assert.True(result.Data.Id != null);
        }

        [Fact]
        public async void Delete_Attachment_Given_Valid_Information()
        {
            // Arrange
            var attachment = new AttachmentBuilder().WithDefaultValues().Build();
            var attachmentService = ServiceHelper.CreateAttachmentService();

            // Act            
            var result = await attachmentService.AddAsync(attachment);
            await attachmentService.RemoveAsync(result.Data.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
