using System;

namespace Moe.La.Core.Dtos
{
    public class ResearcherConsultantListItemDto
    {
        public int? Id { get; set; }

        public Guid? ResearcherId { get; set; }

        public Guid? ConsultantId { get; set; }

        public string Consultant { get; set; }

        public string Researcher { get; set; }
        public string ResearcherDepartment { get; set; }
        public string ConsultantDepartment { get; set; }

        public DateTime? StartDate { get; set; }
        public string StartDateHigri { get; set; } = null;

        public DateTime? EndDate { get; set; }
        public bool Enabled { get; set; }

        public int? ResearcherDepartmentId { get; set; }
    }

    public class ResearcherConsultantDto
    {
        public int Id { get; set; }

        public Guid? ConsultantId { get; set; }

        public Guid ResearcherId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
