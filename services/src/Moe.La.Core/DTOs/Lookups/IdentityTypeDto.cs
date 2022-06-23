using System;

namespace Moe.La.Core.Dtos
{
    public class IdentityTypeListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }
    }

    public class IdentityTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}