using System;

namespace Moe.La.Core.Entities
{
    public class InvestigationPartyPenalty : BaseEntity<int>
    {
        /// <summary>
        /// الجزاات الموقعة بالموظف
        /// </summary>
        public string Penalty { get; set; }
        /// <summary>
        /// الاسباب
        /// </summary>
        public string Reasons { get; set; }
        /// <summary>
        /// رقم القرار
        /// </summary>
        public int DecisionNumber { get; set; }
        /// <summary>
        /// التاريخ
        /// </summary>
        public DateTime Date { get; set; }
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
