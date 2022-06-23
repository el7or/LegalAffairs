using System;

namespace Moe.La.Core.Entities
{
    /// <summary>
    /// The change request 
    /// </summary>
    public class ChangeResearcherToHearingRequest
    {
        public int Id { get; set; }

        /// <summary>
        /// The related hearing id.
        /// </summary>
        public int HearingId { get; set; }

        /// <summary>
        /// The related hearing.
        /// </summary>
        public Hearing Hearing { get; set; }

        /// <summary>
        /// The base request entity 
        /// </summary>
        public Request Request { get; set; }

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
