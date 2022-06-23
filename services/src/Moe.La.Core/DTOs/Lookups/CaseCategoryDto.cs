using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class CaseCategoryListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public KeyValuePairsDto<int> CaseSource { get; set; }

    }

    public class CaseCategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CaseSources? CaseSource { get; set; }
    }
}
