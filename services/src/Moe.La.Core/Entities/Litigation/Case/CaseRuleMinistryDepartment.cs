namespace Moe.La.Core.Entities
{
    public class CaseRuleMinistryDepartment : BaseEntity<int>
    {
        public int CaseRuleId { get; set; }

        public CaseRule CaseRule { get; set; }

        public int MinistryDepartmentId { get; set; }

        public MinistryDepartment MinistryDepartment { get; set; }
    }
}
