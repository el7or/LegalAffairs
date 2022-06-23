using System;

namespace Moe.La.Core.Dtos
{
    public class DistrictListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public string City { get; set; }
    }

    public class DistrictDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? CityId { get; set; }
    }
}
