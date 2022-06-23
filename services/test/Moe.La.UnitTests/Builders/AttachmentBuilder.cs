using Microsoft.AspNetCore.Http;
using Moe.La.Core.Dtos;
using System.IO;
using System.Text;

namespace Moe.La.UnitTests.Builders
{
    class AttachmentBuilder
    {
        private AttachmentDto _attachment = new AttachmentDto();


        public AttachmentBuilder WithDefaultValues()
        {
            var attachmentFolderPath = "Attachments";
            if (!Directory.Exists(attachmentFolderPath))
            { Directory.CreateDirectory(attachmentFolderPath); }

            _attachment = new AttachmentDto
            {
                File = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test file")), 0, 0, "Data", "file1.pdf"),
                AttachmentTypeId = 1
            };

            return this;
        }

        public AttachmentDto Build() => _attachment;
    }
}
