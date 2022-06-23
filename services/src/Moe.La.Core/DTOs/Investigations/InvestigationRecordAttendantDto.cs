namespace Moe.La.Core.Dtos
{
    public class InvestigationRecordAttendantListItemDto : BaseDto<int>
    {
        public string FullName { get; set; }

        public string WorkLocation { get; set; }

        public int RepresentativeOf { get; set; }

        public string Details { get; set; }

        public string IdentityNumber { get; set; }
    }

    public class InvestigationRecordAttendantDetailsDto : BaseDto<int>
    {
        public string FullName { get; set; }

        public string WorkLocation { get; set; }

        public int RepresentativeOf { get; set; }

        public string Details { get; set; }

        public string IdentityNumber { get; set; }

    }

    public class InvestigationRecordAttendantDto
    {
        public int? Id { get; set; }

        public string FullName { get; set; }

        public string WorkLocation { get; set; }

        public string AssignedWork { get; set; }

        public int RepresentativeOfId { get; set; }

        public MinistryDepartmentDto RepresentativeOf { get; set; }

        public string Details { get; set; }

        public string IdentityNumber { get; set; }


    }

}
