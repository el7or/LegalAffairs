namespace Moe.La.Core.Entities.Integration.Moeen
{
    public class ProsecutorRequest
    {
        /// <summary>
        /// رقم معرف في النظام
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// وصف طلب المدعي
        /// </summary>
        public string ProsecutorRequestSubject { get; set; }

        /// <summary>
        /// ترتيب الطلب
        /// </summary>
        public string ProsecutorRequestOrder { get; set; }
    }
}
