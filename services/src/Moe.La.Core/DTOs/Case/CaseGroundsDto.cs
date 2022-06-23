using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class CaseGroundsDto
    {
        public int? Id { get; set; }
        public int? CaseId { get; set; }
        public string Text { get; set; }
    }

    public class CaseGroundsListDto
    {
        public int CaseId { get; set; }

        public ICollection<CaseGroundsDto> CaseGrounds { get; set; } = new List<CaseGroundsDto>();

    }
}
