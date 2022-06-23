namespace Moe.La.Core.Entities
{
    public class InvestigationRecordAttendant : BaseEntity<int>
    {

        /// <summary>
        /// Attendant fullName.
        /// </summary>

        public string FullName { get; set; }

        /// <summary>
        /// Attendant workLocation.
        /// </summary>
        public string WorkLocation { get; set; }

        /// <summary>
        /// the Work Assigned toAttendant.
        /// </summary>
        public string AssignedWork { get; set; }

        /// <summary>
        /// Attendant ministry department.
        /// </summary>
        public int RepresentativeOfId { get; set; }
        public MinistryDepartment RepresentativeOf { get; set; }

        /// <summary>
        /// details about Attendant.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Attendant idetity number.
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// The related investigation record ID.
        /// </summary>
        public int InvestigationRecordId { get; set; }

        /// <summary>
        /// The related investigation record entity.
        /// </summary>
        public InvestigationRecord InvestigationRecord { get; set; }
    }
}
