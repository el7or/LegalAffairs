using Moe.La.Core.Entities;
using System;

namespace Moe.La.Core.Dtos
{
    public class ChangeResearcherToHearingRequestListItemDto
    {
        public int Id { get; set; }

        public int HearingId { get; set; }

        public RequestListItemDto Request { get; set; }

        public DateTime? ReplyDate { get; set; }

        public string ReplyDateHigri { get; set; } = null;

        public string ReplyNote { get; set; }

        public AppUser CurrentResearcher { get; set; }

        public AppUser NewResearcher { get; set; }

        public string CaseNumberInSource { get; set; }

        public string CaseSource { get; set; }
    }

    public class ChangeResearcherToHearingRequestDetailsDto
    {
        public int Id { get; set; }

        public int HearingId { get; set; }

        public RequestDetailsDto Request { get; set; }

        public Guid? CurrentResearcherId { get; set; }

        public Guid? NewResearcherId { get; set; }
    }

    public class ChangeResearcherToHearingRequestDto
    {
        public int Id { get; set; }

        /// <summary>
        /// The Hearing  that the request belong to
        /// </summary>
        public int HearingId { get; set; }

        /// <summary>
        /// The note send with the request
        /// </summary>
        public string Note { get; set; }

        public Guid? NewResearcherId { get; set; }
    }

    public class ReplyChangeResearcherToHearingRequestDto
    {
        public int Id { get; set; }

        public string ReplyNote { get; set; }

        public Guid CurrentResearcherId { get; set; }

        public Guid? NewResearcherId { get; set; }
    }
}
