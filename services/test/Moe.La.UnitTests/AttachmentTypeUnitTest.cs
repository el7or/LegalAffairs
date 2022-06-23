using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class AttachmentTypeUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_AttachmentType_Given_Valid_Information()
        {
            // Arrange
            var attachmentType = new AttachmentTypeBuilder().WithDefaultValues().Build();
            var attachmentTypeService = ServiceHelper.CreateAttachmentTypeService();

            // Act
            var result = await attachmentTypeService.AddAsync(attachmentType);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_AttachmentType_Given_Valid_Information()
        {
            // Arrange
            var attachmentType = new AttachmentTypeBuilder().WithDefaultValues().Build();
            var attachmentTypeService = ServiceHelper.CreateAttachmentTypeService();

            // Act
            await attachmentTypeService.AddAsync(attachmentType);
            attachmentType.Name = "attachmentType1";

            var result = await attachmentTypeService.EditAsync(attachmentType);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("attachmentType1", result.Data.Name);
        }

        [Fact]
        public async void Delete_AttachmentType_Given_Valid_Information()
        {
            // Arrange
            var attachmentType = new AttachmentTypeBuilder().WithDefaultValues().Build();
            var attachmentTypeService = ServiceHelper.CreateAttachmentTypeService();

            // Act            
            var result = await attachmentTypeService.AddAsync(attachmentType);
            await attachmentTypeService.RemoveAsync(attachmentType.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Get_AttachmentType_By_Id_Given_Valid_Information()
        {
            // Arrange
            var attachmentType = new AttachmentTypeBuilder().WithDefaultValues().Build();
            var attachmentTypeService = ServiceHelper.CreateAttachmentTypeService();

            // Act
            await attachmentTypeService.AddAsync(attachmentType);
            var result = await attachmentTypeService.GetAsync(attachmentType.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(attachmentType.Id, result.Data.Id);
        }
    }
}
