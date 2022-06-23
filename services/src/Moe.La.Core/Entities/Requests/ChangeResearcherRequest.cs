using System;

namespace Moe.La.Core.Entities
{
    /// <summary>
    /// The change request 
    /// </summary>
    public class ChangeResearcherRequest
    {
        public int Id { get; set; }

        /// <summary>
        /// The related case id.
        /// </summary>
        public int? CaseId { get; set; }

        /// <summary>
        /// The related case.
        /// </summary>
        public Case Case { get; set; }

        /// <summary>
        /// The base request entity 
        /// </summary>
        public Request Request { get; set; }

        /// <summary>
        /// Legal memo id in case Board Head Senior
        /// </summary>
        public int? LegalMemoId { get; set; }

        /// <summary>
        /// The researcher id who will be changed due to the request.
        /// </summary>
        public Guid CurrentResearcherId { get; set; }

        /// <summary>
        /// Current researcher navigation property.
        /// </summary>
        public AppUser CurrentResearcher { get; set; }

        /// <summary>
        /// The reseacher id who will be substituted with.
        /// </summary>
        public Guid? NewResearcherId { get; set; }

        /// <summary>
        /// New researcher navigation property.
        /// </summary>
        public AppUser NewResearcher { get; set; }

        /// <summary>
        /// The replay date.
        /// </summary>
        public DateTime? ReplyDate { get; set; }

        /// <summary>
        /// The reject reason or accept reason.
        /// </summary>
        public string ReplyNote { get; set; }
    }
}
