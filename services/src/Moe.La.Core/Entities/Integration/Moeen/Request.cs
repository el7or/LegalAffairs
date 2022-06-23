using System;

namespace Moe.La.Core.Entities.Integration.Moeen
{
    public class Request
    {
        /// <summary>
        /// رقم معرف في النظام
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// موضوع الطلب
        /// </summary>
        public string RequestSubject { get; set; }

        /// <summary>
        /// تاريخ تقديم الطلب
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// تاريخ تقديم الطلب هجري
        /// </summary>
        public string RequestDateHijri { get; set; }

        /// <summary>
        /// معرف نوع الطلب حسب معرفات ديوان المظالم
        /// </summary>
        public string RequestTypeCode { get; set; }

        /// <summary>
        /// وصف نوع الطلب
        /// </summary>
        public string RequestTypeDescription { get; set; }

        /// <summary>
        /// اسم مقدم الطلب
        /// </summary>
        public string RequesterName { get; set; }

        /// <summary>
        /// معرف صفة مقدم الطلب حسب معرفات ديوان المظالم
        /// </summary>
        public string RequesterParityTypeCode { get; set; }

        /// <summary>
        /// وصف صفة مقدم الطلب
        /// </summary>
        public string RequesterParityTypeDescription { get; set; }
    }
}
