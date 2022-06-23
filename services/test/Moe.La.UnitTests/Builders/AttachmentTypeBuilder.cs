using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;

namespace Moe.La.UnitTests.Builders
{
    class AttachmentTypeBuilder
    {
        private AttachmentTypeDto _attachmentType = new AttachmentTypeDto();

        public AttachmentTypeBuilder Id(int id)
        {
            _attachmentType.Id = id;
            return this;
        }

        public AttachmentTypeBuilder Name(string name)
        {
            _attachmentType.Name = name;
            return this;
        }

        public AttachmentTypeBuilder GroupName(int? groupName)
        {
            _attachmentType.GroupName = groupName;
            return this;
        }

        public AttachmentTypeBuilder WithDefaultValues()
        {
            _attachmentType = new AttachmentTypeDto
            {
                Id = 0,
                Name = "filetype1",
                GroupName = (int)GroupNames.Case
            };

            return this;
        }

        public AttachmentTypeDto Build() => _attachmentType;
    }
}
