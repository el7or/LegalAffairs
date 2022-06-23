using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class ExportCaseJudgmentRequestListItemDto
    {
        public int Id { get; set; }

        public int? CaseId { get; set; }

        public RequestListItemDto Request { get; set; }

        public string Reason { get; set; }

        public string ReplyNote { get; set; }

        public string CaseNumberInSource { get; set; }

        public string CaseSource { get; set; }

        /// <summary>
        /// رقم المعاملة في الاتصالات الادارية
        /// </summary>
        public string TransactionNumberInAdministrativeCommunications { get; set; }
        /// <summary>
        /// تاريخ المعاملة في الاتصالات الادارية
        /// </summary>
        public DateTime? TransactionDateInAdministrativeCommunications { get; set; }

        public ICollection<ExportCaseJudgmentRequestHistoryListItemDto> History { get; set; } = new List<ExportCaseJudgmentRequestHistoryListItemDto>();

    }

    public class ExportCaseJudgmentRequestDetailsDto
    {
        public int Id { get; set; }

        public RequestDto Request { get; set; }

        public string Reason { get; set; }

        public string ReplyNote { get; set; }

        /// <summary>
        /// رقم المعاملة في الاتصالات الادارية
        /// </summary>
        public string TransactionNumberInAdministrativeCommunications { get; set; }
        /// <summary>
        /// تاريخ المعاملة في الاتصالات الادارية
        /// </summary>
        public DateTime? TransactionDateInAdministrativeCommunications { get; set; }
    }

    public class ExportCaseJudgmentRequestDto
    {
        public int Id { get; set; }

        /// <summary>
        /// The Case that the request belong to
        /// </summary>
        public int CaseId { get; set; }


        public RequestDto Request { get; set; }


    }

    public class ReplyExportCaseJudgmentRequestDto
    {
        public int Id { get; set; }

        public string ReplyNote { get; set; }

        public RequestStatuses RequestStatus { get; set; }

        //public CaseClosinReasons? CaseClosingType { get; set; }
        public string TransactionNumberInAdministrativeCommunications { get; set; }
        public DateTime? TransactionDateInAdministrativeCommunications { get; set; }

        public string ExportRefNo { get; set; }

        public DateTime? ExportRefDate { get; set; }


    }


    public class ExportCaseJudgmentRequestForPrintDto
    {
        public int CaseId { get; set; }
        public string RequestLetter { get; set; }

    }
}
