using Moe.La.Core.Entities;
using System;

namespace Moe.La.Core.Dtos
{
    public class ChangeResearcherRequestListItemDto
    {
        public int Id { get; set; }

        public int? CaseId { get; set; }

        public RequestListItemDto Request { get; set; }

        public int? LegalMemoId { get; set; }

        public DateTime? ReplyDate { get; set; }
        public string ReplyDateHigri { get; set; } = null;

        public string ReplyNote { get; set; }

        public AppUser CurrentResearcher { get; set; }

        public AppUser NewResearcher { get; set; }

        public string CaseNumberInSource { get; set; }

        public string CaseSource { get; set; }
    }

    public class ChangeResearcherRequestDetailsDto
    {
        public int Id { get; set; }

        public int CaseId { get; set; }

        public RequestDetailsDto Request { get; set; }

        public int? LegalMemoId { get; set; }

        public Guid? CurrentResearcherId { get; set; }

        public int CaseYearInSourceHijri { get; set; }

    }

    public class ChangeResearcherRequestDto
    {
        public int Id { get; set; }

        /// <summary>
        /// The Case that the request belong to
        /// </summary>

        public int? CaseId { get; set; }

        /// <summary>
        /// The note send with the request
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Legal memo id in case Board Head Senior
        /// </summary>
        public int? LegalMemoId { get; set; }

        /// <summary>
        /// The researcher id who will be changed due to the request.
        /// </summary>
        public Guid? CurrentResearcherId { get; set; }
    }

    public class ReplyChangeResearcherRequestDto
    {
        public int Id { get; set; }

        public string ReplyNote { get; set; }

        public Guid CurrentResearcherId { get; set; }

        public Guid? NewResearcherId { get; set; }
    }
}
