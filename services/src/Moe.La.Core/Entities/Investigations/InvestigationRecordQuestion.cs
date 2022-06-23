using System;

namespace Moe.La.Core.Entities
{
    public class InvestigationRecordQuestion : BaseEntity<int>
    {
        /// <summary>
        /// The related investigaton record ID.
        /// </summary>
        public int InvestigationRecordId { get; set; }

        /// <summary>
        /// The related investigation record entity.
        /// </summary>
        public InvestigationRecord InvestigationRecord { get; set; }

        /// <summary>
        /// Investigation question.
        /// </summary>
        public int? QuestionId { get; set; }

        public InvestigationQuestion Question { get; set; }

        /// <summary>
        /// Investigation question's answer.
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// assigned to one of parties 
        /// </summary>
        public int AssignedToId { get; set; }
        public InvestigationRecordParty AssignedTo { get; set; }


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
