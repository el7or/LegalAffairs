namespace Moe.La.Core.Models.Integration.Moeen
{
    public class ProsecutorInfoStructureModel
    {
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
