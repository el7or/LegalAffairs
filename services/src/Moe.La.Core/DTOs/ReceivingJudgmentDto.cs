using System;

namespace Moe.La.Core.Dtos
{
    public class ReceivingJudgmentDto
    {
        public int CaseId { get; set; }

        public int HearingId { get; set; }

        /// <summary>
        /// هل تم النطق بالحكم تحفظ في الجلسات
        /// </summary>
        public bool? IsPronouncedJudgment { get; set; }

        public DateTime HearingDate { get; set; }

        /// <summary>
        /// تاريخ النطق بالحكم
        /// </summary>
        public DateTime? PronouncingJudgmentDate { get; set; }

        /// <summary>
        /// تاريخ موعد استلام الحكم
        /// </summary>
        public DateTime? ReceivingJudgmentDate { get; set; }

    }
}