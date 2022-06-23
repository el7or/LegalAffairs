using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class MainCategoryListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

    }

    public class MainCategoryDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public CaseSources CaseSource { get; set; }

    }
}
