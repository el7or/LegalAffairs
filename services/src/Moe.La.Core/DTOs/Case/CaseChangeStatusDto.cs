using Moe.La.Core.Enums;

namespace Moe.La.Core.Dtos
{
    public class CaseChangeStatusDto
    {
        public int Id { get; set; }

        public CaseStatuses Status { get; set; }

        public string Note { get; set; }
    }
}