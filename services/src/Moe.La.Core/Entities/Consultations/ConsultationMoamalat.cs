using System;

namespace Moe.La.Core.Entities
{
    public class ConsultationMoamalat
    {
        public int ConsultationId { get; set; }

        public Consultation Consultation { get; set; }

        public int MoamalaId { get; set; }

        public Moamala Moamala { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
