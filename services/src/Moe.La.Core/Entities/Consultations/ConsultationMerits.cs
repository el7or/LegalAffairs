namespace Moe.La.Core.Entities
{
    public class ConsultationMerits
    {
        public int Id { get; set; }

        public int ConsultationId { get; set; }

        public Consultation Consultation { get; set; }

        /// <summary>
        /// حقل نصي
        /// </summary>
        public string Text { get; set; }
    }
}
