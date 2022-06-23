using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class SecondSubCategoryUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_Second_Sub_Category_Given_Valid_Information()
        {
            // Arrange
            var mainCategory = new MainCategoryBuilder().WithDefaultValues().Build();
            var mainCategoryService = ServiceHelper.CreateMainCategoryService();
            await mainCategoryService.AddAsync(mainCategory);

            var firstSubCategory = new FirstSubCategoryBuilder().WithDefaultValues().Build();
            var firstSubCategoryService = ServiceHelper.CreateFirstSubCategoryService();
            await firstSubCategoryService.AddAsync(firstSubCategory);

            var secondSubCategory = new SecondSubCategoryBuilder().WithDefaultValues().Build();
            secondSubCategory.MainCategory = new Core.Dtos.MainCategoryDto { Id = mainCategory.Id, Name = mainCategory.Name };
            secondSubCategory.FirstSubCategory = new Core.Dtos.FirstSubCategoryDto { Id = firstSubCategory.Id, Name = firstSubCategory.Name };

            var secondSubCategoryService = ServiceHelper.CreateSecondSubCategoryService();

            // Act
            var result = await secondSubCategoryService.AddAsync(secondSubCategory);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }
    }

    public class MainCategoryUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_Main_Category_Given_Valid_Information()
        {
            // Arrange
            var mainCategory = new MainCategoryBuilder().WithDefaultValues().Build();
            var mainCategoryService = ServiceHelper.CreateMainCategoryService();

            // Act
            var result = await mainCategoryService.AddAsync(mainCategory);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }
    }

    public class FirstSubCategoryUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_First_Sub_Category_Given_Valid_Information()
        {
            // Arrange
            var firstSubCategory = new FirstSubCategoryBuilder().WithDefaultValues().Build();
            var firstSubCategoryService = ServiceHelper.CreateFirstSubCategoryService();

            // Act
            var result = await firstSubCategoryService.AddAsync(firstSubCategory);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }
    }
}
