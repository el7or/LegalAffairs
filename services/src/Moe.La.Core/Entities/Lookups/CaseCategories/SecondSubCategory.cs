using System;

namespace Moe.La.Core.Entities
{
    public class SecondSubCategory
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public int FirstSubCategoryId { get; set; }

        public bool IsActive { get; set; }

        public FirstSubCategory FirstSubCategory { get; set; }

    }
}
