using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.UnitTests.Builders.Case
{
    public class ReceiveJudgmentInstrumentBuilder
    {
        private ReceiveJdmentInstrumentDto _receiveJdmentInstrumentDto = new ReceiveJdmentInstrumentDto();
        // private ReplyCaseSupportingDocumentRequestDto _replyDocumentRequest = new ReplyCaseSupportingDocumentRequestDto();

        public ReceiveJudgmentInstrumentBuilder Id(int id)
        {
            _receiveJdmentInstrumentDto.Id = id;
            return this;
        }

        public ReceiveJudgmentInstrumentBuilder CaseNumber(string caseNumber)
        {
            _receiveJdmentInstrumentDto.CaseNumber = caseNumber;
            return this;
        }

        public ReceiveJudgmentInstrumentBuilder RuleNumber(string ruleNumber)
        {
            _receiveJdmentInstrumentDto.RuleNumber = ruleNumber;
            return this;
        }

        public ReceiveJudgmentInstrumentBuilder JudgementText(string judgementText)
        {
            _receiveJdmentInstrumentDto.JudgementText = judgementText;
            return this;
        }

        public ReceiveJudgmentInstrumentBuilder JudgmentBrief(string judgmentBrief)
        {
            _receiveJdmentInstrumentDto.JudgmentBrief = judgmentBrief;
            return this;
        }
        public ReceiveJudgmentInstrumentBuilder JudgementResult(JudgementResults judgementResult)
        {
            _receiveJdmentInstrumentDto.JudgementResult = judgementResult;
            return this;
        }
        public ReceiveJudgmentInstrumentBuilder CaseRuleMinistryDepartments(ICollection<int> caseRuleMinistryDepartments)
        {
            _receiveJdmentInstrumentDto.CaseRuleMinistryDepartments = caseRuleMinistryDepartments;
            return this;
        }
        public ReceiveJudgmentInstrumentBuilder Feedback(string feedback)
        {
            _receiveJdmentInstrumentDto.Feedback = feedback;
            return this;
        }
        public ReceiveJudgmentInstrumentBuilder FinalConclusions(string finalConclusions)
        {
            _receiveJdmentInstrumentDto.FinalConclusions = finalConclusions;
            return this;
        }
        public ReceiveJudgmentInstrumentBuilder JudgmentReasons(string judgmentReasons)
        {
            _receiveJdmentInstrumentDto.JudgmentReasons = judgmentReasons;
            return this;
        }
        public ReceiveJudgmentInstrumentBuilder ImportRefNo(string importRefNo)
        {
            _receiveJdmentInstrumentDto.ImportRefNo = importRefNo;
            return this;
        }
        public ReceiveJudgmentInstrumentBuilder ImportRefDate(DateTime? importRefDate)
        {
            _receiveJdmentInstrumentDto.ImportRefDate = importRefDate;
            return this;
        }
        public ReceiveJudgmentInstrumentBuilder ExportRefNo(string exportRefNo)
        {
            _receiveJdmentInstrumentDto.ExportRefNo = exportRefNo;
            return this;
        }
        public ReceiveJudgmentInstrumentBuilder ExportRefDate(DateTime? exportRefDate)
        {
            _receiveJdmentInstrumentDto.ExportRefDate = exportRefDate;
            return this;
        }
        //public ReceiveJudgmentInstrumentBuilder Attachments(List<CaseRuleAttachmentDto> attachments)
        //{
        //    _receiveJdmentInstrumentDto.Attachments = attachments;
        //    return this;
        //}

        public ReceiveJudgmentInstrumentBuilder WithDefaultValues()
        {
            _receiveJdmentInstrumentDto = new ReceiveJdmentInstrumentDto()
            {
                CaseNumber = "999",
                RuleNumber = "999999",
                CaseRuleMinistryDepartments = new int[] { 1 },
                FinalConclusions = "test conslusions",
                Feedback = "test FeedBack ",
                ExportRefDate = null,
                ImportRefDate = null,
                ImportRefNo = "",
                ExportRefNo = "",
                JudgementResult = JudgementResults.Favor,
                JudgementText = "test2",
                JudgmentBrief = "tetst1",
                JudgmentReasons = "tests9",
                Attachments = null
            };

            return this;
        }


        public ReceiveJdmentInstrumentDto Build() => _receiveJdmentInstrumentDto;

    }
}
