using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;

namespace Moe.La.UnitTests.Builders
{
    public class MainCategoryBuilder
    {
        private MainCategoryDto _mainCategory = new MainCategoryDto();
        public MainCategoryBuilder Name(string name)
        {
            _mainCategory.Name = name;
            return this;
        }
        public MainCategoryBuilder CaseSource(CaseSources caseSources)
        {
            _mainCategory.CaseSource = caseSources;
            return this;
        }

        public MainCategoryBuilder WithDefaultValues()
        {
            _mainCategory = new MainCategoryDto
            {
                Name = "aaaa",
                CaseSource = CaseSources.Najiz
            };

            return this;
        }

        public MainCategoryDto Build() => _mainCategory;
    }
}
