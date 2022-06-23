using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;

namespace Moe.La.UnitTests.Builders
{
    public class SecondSubCategoryBuilder
    {
        private SecondSubCategoryDto _secondSubCategory = new SecondSubCategoryDto();
        public SecondSubCategoryBuilder Name(string name)
        {
            _secondSubCategory.Name = name;
            return this;
        }
        public SecondSubCategoryBuilder CaseSource(CaseSources caseSources)
        {
            _secondSubCategory.CaseSource = caseSources;
            return this;
        }

        public SecondSubCategoryBuilder FirstSubCategory(FirstSubCategoryDto firstSubCategory)
        {
            _secondSubCategory.FirstSubCategory = firstSubCategory;
            return this;
        }

        public SecondSubCategoryBuilder MainCategory(MainCategoryDto mainCategory)
        {
            _secondSubCategory.MainCategory = mainCategory;
            return this;
        }

        public SecondSubCategoryBuilder WithDefaultValues()
        {
            _secondSubCategory = new SecondSubCategoryDto
            {
                Name = "aaaa",
                CaseSource = CaseSources.Najiz,
                MainCategory = new MainCategoryBuilder().WithDefaultValues().Build(),
                FirstSubCategory = new FirstSubCategoryBuilder().WithDefaultValues().Build()
            };

            return this;
        }

        public SecondSubCategoryDto Build() => _secondSubCategory;
    }
}
