using System;

namespace Moe.La.Core.Entities.Integration.Moeen
{
    public class Ruling
    {
        /// <summary>
        /// رقم معرف في النظام
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// معرف الدائرة مصدرة الحكم حسب معرفات ديوان المظالم
        /// </summary>
        public string CircuitID { get; set; }

        /// <summary>
        /// اسم الدائرة مصدرة الحكم
        /// </summary>
        public string CircuitDescription { get; set; }

        /// <summary>
        /// تاريخ الحكم
        /// </summary>
        public DateTime RulingDate { get; set; }

        /// <summary>
        /// تاريخ الحكم هجري
        /// </summary>
        public string RulingDateHijri { get; set; }

        /// <summary>
        /// منطوق الحكم
        /// </summary>
        public string RulingSpoken { get; set; }

        /// <summary>
        /// التاريخ المحدد لاستلام الحكم
        /// </summary>
        public DateTime RulingDeliveryDate { get; set; }

        /// <summary>
        /// التاريخ المحدد لاستلام الحكم هجري
        /// </summary>
        public string RulingDeliveryDateHijri { get; set; }
    }
}
