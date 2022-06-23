using System;

namespace Moe.La.Core.Dtos
{
    public class ExportCaseJudgmentRequestHistoryListItemDto
    {
        public int Id { get; set; }

        public RequestListItemDto Request { get; set; }

        /// <summary>
        /// The closing reason.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// The reject reason or accept reason.
        /// </summary>
        public string ReplyNote { get; set; }

        /// <summary>
        /// The replay date.
        /// </summary>
        public DateTime? ReplyDate { get; set; }


    }

}
