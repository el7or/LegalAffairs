namespace Moe.La.Core.Entities.Integration.Moeen
{
    public class Prosecutor
    {
        /// <summary>
        /// رقم معرف في النظام
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// معرف المدعي حسب معرفات ديوان المظالم
        /// </summary>
        public string ProsecutorCode { get; set; }

        /// <summary>
        /// اسم المدعي
        /// </summary>
        public string ProsecutorName { get; set; }

        /// <summary>
        /// معرف نوع المدعي حسب معرفات ديوان المظالم
        /// </summary>
        public string ParityTypeCode { get; set; }

        /// <summary>
        /// وصف نوع المدعي (فرد-شركة -....)
        /// </summary>
        public string ParityTypeDescription { get; set; }
    }
}
