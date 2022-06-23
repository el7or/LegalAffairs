using System;

namespace Moe.La.Core.Entities
{
    public class Evaluation : BaseEntity<int>
    {
        public int Percentage { get; set; }
        public int Year { get; set; }
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
