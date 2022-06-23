using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class MainCategory
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public CaseSources CaseSource { get; set; }

        public string Name { get; set; }

        public ICollection<FirstSubCategory> FirstSubCategories { get; set; } = new List<FirstSubCategory>();

    }
}
