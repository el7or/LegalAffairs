using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class LegalMemoUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_LegalMemo_Given_Valid_Information()
        {
            // Arrange
            var legalMemo = new LegalMemoBuilder().WithDefaultValues().Build();
            var legalMemoService = ServiceHelper.CreateLegalMemoService();

            // Act
            var result = await legalMemoService.AddAsync(legalMemo);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_LegalMemo_Given_Valid_Information()
        {
            // Arrange
            var mainCategory = new MainCategoryBuilder().WithDefaultValues().Build();
            var mainCategoryService = ServiceHelper.CreateMainCategoryService();
            await mainCategoryService.AddAsync(mainCategory);

            var firstSubCategory = new FirstSubCategoryBuilder().WithDefaultValues().Build();
            var firstSubCategoryService = ServiceHelper.CreateFirstSubCategoryService();
            await firstSubCategoryService.AddAsync(firstSubCategory);

            var secondSubCategory = new SecondSubCategoryBuilder().WithDefaultValues().Build();
            secondSubCategory.MainCategory = new MainCategoryDto { Id = mainCategory.Id, Name = mainCategory.Name };
            secondSubCategory.FirstSubCategory = new FirstSubCategoryDto { Id = firstSubCategory.Id, Name = firstSubCategory.Name };

            var secondSubCategoryService = ServiceHelper.CreateSecondSubCategoryService();
            await secondSubCategoryService.AddAsync(secondSubCategory);


            var legalMemo = new LegalMemoBuilder().WithDefaultValues().Build();
            var legalMemoService = ServiceHelper.CreateLegalMemoService();

            // Act            
            await legalMemoService.AddAsync(legalMemo);
            legalMemo.Name = "memo 1113";
            legalMemo.Text = "text";
            var result = await legalMemoService.EditAsync(legalMemo);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("memo 1113", result.Data.Name);
            Assert.Equal("text", result.Data.Text);
        }

        [Fact]
        public async void Delete_Part_Given_Valid_Information()
        {
            // Arrange
            var legalMemo = new LegalMemoBuilder().WithDefaultValues().Build();
            var legalMemoService = ServiceHelper.CreateLegalMemoService();
            DeletionLegalMemoDto deletionLegalMemoDto = new DeletionLegalMemoDto() { Id = 1, DeletionReason = "سبب الحذف" };
            // Act
            await legalMemoService.AddAsync(legalMemo);
            var result = await legalMemoService.RemoveAsync(deletionLegalMemoDto);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Get_LegalMemo_By_Id_Given_Valid_Information()
        {
            // Arrange

            var legalMemo = new LegalMemoBuilder().WithDefaultValues().Build();
            var legalMemoService = ServiceHelper.CreateLegalMemoService();

            var caseDto = new CaseBuilder().WithDefaultValues()
                .SecondSubCategoryId(1)
                .Build();
            var service = ServiceHelper.CreateCaseService();
            await service.AddAsync(caseDto);

            // Act
            await legalMemoService.AddAsync(legalMemo);
            var result = await legalMemoService.GetAsync(legalMemo.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(legalMemo.Id, result.Data.Id);
        }

        [Fact]
        public async void Accept_LegalMemo_Given_Valid_Information()
        {
            // Arrange
            var legalMemo = new LegalMemoBuilder().WithDefaultValues().Build();
            var legalMemoService = ServiceHelper.CreateLegalMemoService();

            // Act
            await legalMemoService.AddAsync(legalMemo);
            var result = await legalMemoService.ChangeLegalMemoStatusAsync(legalMemo.Id, (int)LegalMemoStatuses.Accepted);

            // Assert
            Assert.True(result.IsSuccess);
        }
        [Fact]
        public async void Approved_LegalMemo_Given_Valid_Information()
        {
            // Arrange
            var legalMemo = new LegalMemoBuilder().WithDefaultValues().Build();
            var legalMemoService = ServiceHelper.CreateLegalMemoService();

            // Act
            await legalMemoService.AddAsync(legalMemo);
            var result = await legalMemoService.ChangeLegalMemoStatusAsync(legalMemo.Id, (int)LegalMemoStatuses.Approved);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Returned_LegalMemo_Given_Valid_Information()
        {
            // Arrange
            var legalMemo = new LegalMemoBuilder().WithDefaultValues().Build();
            var legalMemoService = ServiceHelper.CreateLegalMemoService();

            // Act
            await legalMemoService.AddAsync(legalMemo);
            var result = await legalMemoService.ChangeLegalMemoStatusAsync(legalMemo.Id, (int)LegalMemoStatuses.Returned);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Read_LegalMemo_Given_Valid_Information()
        {
            // Arrange
            var legalMemo = new LegalMemoBuilder().WithDefaultValues().Build();
            var legalMemoService = ServiceHelper.CreateLegalMemoService();

            // Act
            await legalMemoService.AddAsync(legalMemo);
            var result = await legalMemoService.ReadLegalMemoAsync(legalMemo.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Get_legal_memo_List_Given_Valid_Search_Information()
        {
            // Arrange
            var service = ServiceHelper.CreateLegalMemoService();

            LegalMemoQueryObject queryObject = new LegalMemoQueryObject { Name = "" };
            // Act
            var result = await service.GetAllAsync(queryObject);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
