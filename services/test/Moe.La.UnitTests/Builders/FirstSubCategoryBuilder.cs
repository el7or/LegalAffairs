using Moe.La.Core.Dtos;

namespace Moe.La.UnitTests.Builders
{
    public class FirstSubCategoryBuilder
    {
        private FirstSubCategoryDto _firstSubCategory = new FirstSubCategoryDto();
        public FirstSubCategoryBuilder Name(string name)
        {
            _firstSubCategory.Name = name;
            return this;
        }
        public FirstSubCategoryBuilder MainCategoryId(int mainCategoryId)
        {
            _firstSubCategory.MainCategoryId = mainCategoryId;
            return this;
        }

        public FirstSubCategoryBuilder WithDefaultValues()
        {
            _firstSubCategory = new FirstSubCategoryDto
            {
                Name = "aaaa",
            };

            return this;
        }

        public FirstSubCategoryDto Build() => _firstSubCategory;
    }
}
