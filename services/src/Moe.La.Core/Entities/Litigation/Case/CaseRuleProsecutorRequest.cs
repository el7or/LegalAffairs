namespace Moe.La.Core.Entities
{
    public class CaseRuleProsecutorRequest : BaseEntity<int>
    {
        /// <summary>
        /// وصف طلب المدعي
        /// </summary>
        public string ProsecutorRequestSubject { get; set; }

        /// <summary>
        /// ترتيب الطلب
        /// </summary>
        public string ProsecutorRequestOrder { get; set; }

        public int CaseId { get; set; }

        public Case Case { get; set; }
    }
}
