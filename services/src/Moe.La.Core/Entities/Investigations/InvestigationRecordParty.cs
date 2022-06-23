using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class InvestigationRecordParty : BaseEntity<int>
    {
        /// <summary>
        /// The related investigation record ID.
        /// </summary>
        public int InvestigationRecordId { get; set; }

        /// <summary>
        /// The related investigation record entity.
        /// </summary>
        public InvestigationRecord InvestigationRecord { get; set; }

        /// <summary>
        /// Party name.
        /// </summary>
        public string PartyName { get; set; }
        /// <summary>
        /// تاريخ الميلاد
        /// </summary>
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// نوع الكادر
        /// </summary>
        public StaffType StaffType { get; set; }
        /// <summary>
        /// العمل المكلف به   
        /// </summary>
        public string AssignedWork { get; set; }
        /// <summary>
        /// مقر العمل
        /// </summary>
        public string WorkLocation { get; set; }
        /// <summary>
        /// Party idetity number.
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// The related investigation record party type ID.
        /// </summary>
        public int InvestigationRecordPartyTypeId { get; set; }

        /// <summary>
        /// The related investigation record party type entity.
        /// </summary>
        public InvestigationRecordPartyType InvestigationRecordPartyType { get; set; }
        /// <summary>
        /// تاريخ المباشرة
        /// </summary>
        public DateTime CommencementDate { get; set; }
        /// <summary>
        /// تاريخ الهوية
        /// </summary>
        public DateTime IdentityDate { get; set; }
        /// <summary>
        /// حالةالتعيين
        /// </summary>
        public AppointmentStatus AppointmentStatus { get; set; }
        /// <summary>
        /// مصدر الهوية   
        /// </summary>
        public string IdentitySource { get; set; }
        /// <summary>
        /// اخر مؤهل حصل عليه
        /// </summary>
        public string LastQualificationAttained { get; set; }

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
        public ICollection<InvestigationPartyPenalty> InvestigationPartyPenalties { get; set; } = new List<InvestigationPartyPenalty>();
        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
        public ICollection<EducationalLevel> EducationalLevels { get; set; } = new List<EducationalLevel>();
    }
}
