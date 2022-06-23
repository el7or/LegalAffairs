using System;

namespace Moe.La.Core.Entities
{
    public class Country
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string NationalityAr { get; set; }

        public string NationalityEn { get; set; }

        public string ISO31661CodeAlph3 { get; set; }

    }
}
