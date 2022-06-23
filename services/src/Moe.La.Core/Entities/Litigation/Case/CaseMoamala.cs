using System;

namespace Moe.La.Core.Entities
{
    public class CaseMoamala
    {
        public int CaseId { get; set; }

        public Case Case { get; set; }

        public int MoamalaId { get; set; }

        public Moamala Moamala { get; set; }

        /// <summary>
        /// The creation datetime.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Logical delete, if true don't show it.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
