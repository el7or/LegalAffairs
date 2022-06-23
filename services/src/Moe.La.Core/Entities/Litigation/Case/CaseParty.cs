using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class CaseParty : BaseEntity<int>
    {
        /// <summary>
        /// معرف القضية.
        /// </summary> 
        public int CaseId { get; set; }

        /// <summary>
        /// The case object.
        /// </summary>
        public Case Case { get; set; }

        /// <summary>
        /// الطرف 
        /// </summary>
        public int PartyId { get; set; }

        /// <summary>
        /// The party object.
        /// </summary>
        public Party Party { get; set; }

        /// <summary>
        /// صفة الطرف
        /// </summary>
        public PartyStatus? PartyStatus { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who created it.
        /// </summary>
        public AppUser CreatedByUser { get; set; }

        /// <summary>
        /// The user's id who updated it.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who updated it.
        /// </summary>
        public AppUser UpdatedByUser { get; set; }

        /// <summary>
        /// The update datetime.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}
