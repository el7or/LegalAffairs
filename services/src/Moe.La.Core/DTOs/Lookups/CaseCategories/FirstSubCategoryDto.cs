using System;

namespace Moe.La.Core.Dtos
{
    public class FirstSubCategoryListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }
    }

    public class FirstSubCategoryDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public int? MainCategoryId { get; set; }

    }
}
