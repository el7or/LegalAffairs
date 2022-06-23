using System;

namespace Moe.La.Core.Dtos
{
    public class SaveJudgementCaseDto
    {
        public string Judgement { get; set; }

        public string JudgementNumber { get; set; }

        public int? JudgementResultId { get; set; }

        public DateTime JudgementDate { get; set; }

        public string ConsultantProposal { get; set; }
    }
}
