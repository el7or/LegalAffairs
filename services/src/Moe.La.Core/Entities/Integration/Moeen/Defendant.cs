namespace Moe.La.Core.Entities.Integration.Moeen
{
    public class Defendant
    {
        /// <summary>
        /// رقم معرف في النظام
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// معرف المدعي عليه حسب معرفات ديوان المظالم
        /// </summary>
        public string DefendantCode { get; set; }

        /// <summary>
        /// اسم المدعى عليه
        /// </summary>
        public string DefendantName { get; set; }

        /// <summary>
        /// معرف نوع المدعى عليه حسب معرفات ديوان المظالم
        /// </summary>
        public string ParityTypeCode { get; set; }

        /// <summary>
        /// وصف نوع المدعى عليه (فرد-شركة -....)
        /// </summary>
        public string ParityTypeDescription { get; set; }
    }
}
