using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class FirstSubCategory
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public int MainCategoryId { get; set; }

        public MainCategory MainCategory { get; set; }

        public ICollection<SecondSubCategory> SecondSubCategories { get; set; } = new List<SecondSubCategory>();
    }
}
