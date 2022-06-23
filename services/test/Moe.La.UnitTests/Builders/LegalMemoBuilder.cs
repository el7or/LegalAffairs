using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;

namespace Moe.La.UnitTests.Builders
{
    public class LegalMemoBuilder
    {

        private LegalMemoDto _legalMemo = new LegalMemoDto();

        public LegalMemoBuilder Name(string name)
        {
            _legalMemo.Name = name;
            return this;
        }

        public LegalMemoBuilder Status(LegalMemoStatuses status)
        {
            _legalMemo.Status = status;
            return this;
        }

        public LegalMemoBuilder Text(string text)
        {
            _legalMemo.Text = text;
            return this;
        }


        public LegalMemoBuilder WithDefaultValues()
        {
            _legalMemo = new LegalMemoDto
            {
                Name = "LegalMemo 2",
                Status = LegalMemoStatuses.New,
                Text = "some text",
                InitialCaseId = 1,
                SecondSubCategoryId = 1,
                IsResearcher = true,
                IsRead = false,
                Type = LegalMemoTypes.Pleading,
                //UpdatedBy = Guid.Parse("7bc87c5b-4da4-4ac2-9f1c-6839e642d648"),
                //UpdatedOn = DateTime.Now,
            };

            return this;
        }

        public LegalMemoDto Build() => _legalMemo;
    }
}
