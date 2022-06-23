using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class LetterTemplateDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public LetterTemplateTypes Type { get; set; }

        public string Text { get; set; }

        public string Thumbnail { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
