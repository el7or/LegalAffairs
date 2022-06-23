using Moe.La.Core.Enums.Integration;

namespace Moe.La.Core.Entities.Integration.Moeen
{
    public class SupportingDocument
    {
        /// <summary>
        /// اسم الملف المرفق
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// وصف الملف المرفق
        /// </summary>
        public string AttachmentDescription { get; set; }

        /// <summary>
        /// معرف نوع المرفق بها الدعوى حسب معرفات ديوان المظالم
        /// </summary>
        public string AttachmentTypeCode { get; set; }

        /// <summary>
        /// وصف نوع المرفق (صورة هوية -صورة وكالة -....)
        /// </summary>
        public string AttachmentTypeDescription { get; set; }

        /// <summary>
        /// امتداد الملف المرفق
        /// </summary>
        public MoeenFileExtension FileExtension { get; set; }

        /// <summary>
        /// حجم الملف المرفق بال م.ب
        /// </summary>
        public string AttachmentSize { get; set; }

        /// <summary>
        /// محتوى الملف المرفق
        /// </summary>
        public byte[] AttachmentFile { get; set; }
    }
}
