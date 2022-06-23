using Moe.La.Core.Enums;

namespace Moe.La.Core.Entities
{
    public class LetterTemplate : BaseEntity<int>
    {
        public string Name { get; set; }

        public LetterTemplateTypes Type { get; set; }

        public string Text { get; set; }

        public string Thumbnail { get; set; }
    }
}
