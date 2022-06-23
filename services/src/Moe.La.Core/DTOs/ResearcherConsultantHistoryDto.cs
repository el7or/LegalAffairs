using System;

namespace Moe.La.Core.Dtos
{
    public class ResearcherConsultantHistoryListItemDto
    {
        public int ResearcherConsultantId { get; set; }

        public string ResearcherConsultant { get; set; }

        public string Consultant { get; set; }

        public string Researcher { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }


    public class ResearcherConsultantHistoryDto
    {
        public int ResearcherConsultantId { get; set; }

        public Guid? ConsultantId { get; set; }

        public Guid ResearcherId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
