using System;

namespace Moe.La.Core.Entities
{
    public class InvestigationRecordInvestigator : BaseEntity<int>
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
        /// The investigator ID.
        /// </summary>
        public Guid InvestigatorId { get; set; }

        /// <summary>
        /// The  investigator entity.
        /// </summary>
        public AppUser Investigator { get; set; }
    }
}
