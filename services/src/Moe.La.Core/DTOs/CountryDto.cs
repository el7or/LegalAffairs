namespace Moe.La.Core.Dtos
{
    public class CountryListItemDto
    {
        public int Id { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string NationalityAr { get; set; }

        public string NationalityEn { get; set; }

        public string ISO31661CodeAlph3 { get; set; }
    }

    public class CountryDetailsDto : BaseDto<int>
    {
        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string NationalityAr { get; set; }

        public string NationalityEn { get; set; }

        public string ISO31661CodeAlph3 { get; set; }
    }

    public class CountryDto
    {
        public int? Id { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string NationalityAr { get; set; }

        public string NationalityEn { get; set; }

        public string ISO31661CodeAlph3 { get; set; }
    }
}
