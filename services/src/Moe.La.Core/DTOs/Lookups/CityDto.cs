using System;

namespace Moe.La.Core.Dtos
{
    public class CityListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public string Province { get; set; }
    }

    public class CityDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProvinceId { get; set; }
    }
}
