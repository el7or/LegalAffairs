namespace Moe.La.Core.Models.Integration.Moeen
{
    public class NextSessionInfoStructureModel
    {
        /// <summary>
        /// تاريخ الجلسة
        /// </summary>
        public GHDateStructureModel SessionDate { get; set; }

        /// <summary>
        /// يوم الجلسة
        /// </summary>
        public string SessionDayArabic { get; set; }

        /// <summary>
        /// وقت الجلسة
        /// </summary>
        public string SessionTime { get; set; }

        /// <summary>
        /// معرف نوع الجلسة حسب معرفات ديوان المظالم
        /// </summary>
        public string SessionTypeCode { get; set; }

        /// <summary>
        /// وصف نوع الجلسة (نظر -مرافعة -نطق بالحكم -...)
        /// </summary>
        public string SessionTypeDescription { get; set; }
    }
}
