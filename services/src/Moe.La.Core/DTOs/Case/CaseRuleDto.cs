using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class CaseRuleDetailsDto
    {
        public int Id { get; set; }

        /// <summary>
        /// رقم المعاملة
        /// </summary>
        public string RefNo { get; set; }

        /// <summary>
        /// تاريخ المعاملة
        /// </summary>
        public DateTime? RefDate { get; set; }

        /// <summary>
        /// نص الحكم
        /// </summary>
        public string JudgementText { get; set; }

        /// <summary>
        /// حكم نهائي
        /// </summary>
        public bool? IsFinalJudgment { get; set; }

        /// <summary>
        /// نوع الحكم
        /// </summary>
        public KeyValuePairsDto<int> JudgementResult { get; set; }

        /// <summary>
        /// مختصر الحكم
        /// </summary>
        public string JudgmentBrief { get; set; }

        /// <summary>
        /// الجهة المعنية بالحكم
        /// </summary>
        public ICollection<KeyValuePairsDto<int>> CaseRuleGeneralManagements { get; set; }

        /// <summary>
        /// منطوق الحكم
        /// </summary>
        public string PronouncedJudgmentText { get; set; }
        /// <summary>
        /// ملخص صك الحكم
        /// </summary>
        public string JudgmentInstrumentSummary { get; set; }
    }

    public class CaseRuleDto
    {
        public int Id { get; set; }

        public int CaseId { get; set; }

        public string RefNo { get; set; }

        public DateTime? RefDate { get; set; }

        public string JudgementText { get; set; }

        public bool? IsFinalJudgment { get; set; }

        public JudgementResults? JudgementResult { get; set; }

        public string JudgmentBrief { get; set; }

        public int MinistrySectorId { get; set; }

        public ICollection<int> CaseRuleGeneralManagementIds { get; set; } = new List<int>();
    }


    public class ReceiveJdmentInstrumentDto
    {
        public int Id { get; set; }
        /// <summary>
        /// رقم الحكم في النظام
        /// </summary>
        public string RuleNumber { get; set; }
        /// <summary>
        /// رقم القضية في النظام
        /// </summary>
        public string CaseNumber { get; set; }

        /// <summary>
        /// مختصر الحكم
        /// ملخص  القضية
        /// </summary>
        public string JudgmentBrief { get; set; }

        /// <summary>
        /// نص الحكم
        /// منطوق الحكم
        /// </summary>
        public string JudgementText { get; set; }

        /// <summary>
        /// نوع الحكم
        /// </summary>
        public JudgementResults JudgementResult { get; set; }

        /// <summary>
        /// الجهة المعنية بالحكم
        /// </summary>
        public int MinistrySectorId { get; set; }

        public ICollection<int> CaseRuleMinistryDepartments { get; set; }

        /// <summary>
        /// الأسباب التي بني عليها الحكم
        /// </summary>
        public string JudgmentReasons { get; set; }

        /// <summary>
        /// التحليل والرأي
        /// </summary>
        public string Feedback { get; set; }

        /// <summary>
        /// الاستنتاجات النهائية
        /// </summary>
        public string FinalConclusions { get; set; }

        public string ImportRefNo { get; set; }

        public DateTime? ImportRefDate { get; set; }

        public string ExportRefNo { get; set; }

        public DateTime? ExportRefDate { get; set; }

        public List<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();
    }

    public class ReceiveJdmentInstrumentDetailsDto
    {
        public int Id { get; set; }

        public string RuleNumber { get; set; }

        public string CaseNumber { get; set; }

        public string JudgmentBrief { get; set; }

        public string JudgementText { get; set; }

        public bool? IsFinalJudgment { get; set; }

        public KeyValuePairsDto<int> JudgementResult { get; set; }

        public string JudgmentReasons { get; set; }

        public string Feedback { get; set; }

        public string FinalConclusions { get; set; }

        public string ImportRefNo { get; set; }

        public DateTime? ImportRefDate { get; set; }

        public string ImportRefDateHigri { get; set; }

        public string ExportRefNo { get; set; }

        public DateTime? ExportRefDate { get; set; }

        public string ExportRefDateHigri { get; set; }

        public int MinistrySectorId { get; set; }
        public string MinistrySector { get; set; }
        public ICollection<KeyValuePairsDto<int>> CaseRuleMinistryDepartments { get; set; }

        public List<AttachmentListItemDto> Attachments { get; set; }
    }
}
