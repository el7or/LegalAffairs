using System;

namespace Moe.La.Core.Entities.Integration.Moeen
{
    public class Session
    {
        /// <summary>
        /// رقم معرف في النظام
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// تاريخ الجلسة
        /// </summary>
        public DateTime SessionDate { get; set; }

        /// <summary>
        /// تاريخ الجلسة هجري
        /// </summary>
        public string SessionDateHijri { get; set; }

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
