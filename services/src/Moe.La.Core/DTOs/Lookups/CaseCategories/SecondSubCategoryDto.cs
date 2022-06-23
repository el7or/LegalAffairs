using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class SecondSubCategoryListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public KeyValuePairsDto<int> CaseSource { get; set; }

        public KeyValuePairsDto<int> FirstSubCategory { get; set; }

        public KeyValuePairsDto<int> MainCategory { get; set; }
    }

    public class SecondSubCategoryDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public bool? IsActive { get; set; }

        public CaseSources? CaseSource { get; set; } = null;

        public FirstSubCategoryDto FirstSubCategory { get; set; } = null;

        public MainCategoryDto MainCategory { get; set; } = null;

    }
}
