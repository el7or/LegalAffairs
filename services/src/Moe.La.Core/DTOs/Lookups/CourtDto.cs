using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class CourtListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public string LitigationType { get; set; }

        public string CourtCategory { get; set; }

        public string Code { get; set; }
    }

    public class CourtDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public LitigationTypes? LitigationType { get; set; }

        public CourtCategories CourtCategory { get; set; }

        public string Code { get; set; }
    }
}
