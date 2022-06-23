using System;

namespace Moe.La.Core.Entities
{
    public class ExportCaseJudgmentRequestHistory
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
        /// The CaseClosingRequest related.
        /// </summary>
        public int ExportCaseJudgmentRequestId { get; set; }

        /// <summary>
        /// A navigation property to CaseClosingRequest.
        /// </summary>
        public ExportCaseJudgmentRequest ExportCaseJudgmentRequest { get; set; }

        public RequestHistory Request { get; set; }


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
