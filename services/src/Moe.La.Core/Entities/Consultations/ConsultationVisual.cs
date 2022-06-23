namespace Moe.La.Core.Entities
{
    public class ConsultationVisual
    {
        public int Id { get; set; }

        public int ConsultationId { get; set; }
        public Consultation Consultation { get; set; }

        /// <summary>
        /// المادة
        /// </summary>
        public string Material { get; set; }

        /// <summary>
        /// المرئيات
        /// </summary>
        public string Visuals { get; set; }
    }
}
