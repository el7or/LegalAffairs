using System;

namespace Moe.La.Core.Entities
{
    public class EducationalLevel : BaseEntity<int>
    {
        /// <summary>
        /// المرحلة الدراسية
        /// </summary>
        public string EducationLevel { get; set; }
        /// <summary>
        /// الصف
        /// </summary>
        public string Class { get; set; }
        /// <summary>
        /// الفصل
        /// </summary>
        public string ClassNumber { get; set; }
        /// <summary>
        /// عنوان الاقامة
        /// </summary>
        public string ResidenceAddress { get; set; }
        /// <summary>
        /// The Investigation Record Party Id
        /// </summary>
        public int InvestigationRecordPartyId { get; set; }
        /// <summary>
        /// The Investigation Record Party
        /// </summary>
        public InvestigationRecordParty InvestigationRecordParty { get; set; }
        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// The user's id who updated it.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// The update datetime.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}
