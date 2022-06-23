using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class ExportCaseJudgmentRequest
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
        /// The base request entity.
        /// </summary>
        public Request Request { get; set; }


        /// <summary>
        /// The reject reason or accept reason.
        /// </summary>
        public string ReplyNote { get; set; }

        /// <summary>
        /// The replay date.
        /// </summary>
        public DateTime? ReplyDate { get; set; }

        /// <summary>
        /// رقم المعاملة في الاتصالات الادارية
        /// </summary>
        public string TransactionNumberInAdministrativeCommunications { get; set; }
        /// <summary>
        /// تاريخ المعاملة في الاتصالات الادارية
        /// </summary>
        public DateTime? TransactionDateInAdministrativeCommunications { get; set; }

        public ICollection<ExportCaseJudgmentRequestHistory> History { get; set; }

    }
}
