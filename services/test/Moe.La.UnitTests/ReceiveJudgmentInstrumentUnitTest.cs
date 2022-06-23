using Moe.La.UnitTests.Builders;
using Moe.La.UnitTests.Builders.Case;
using Xunit;

namespace Moe.La.UnitTests
{
    public class ReceiveJudgmentInstrumentUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Add_Judgment_Instrument_Given_Valid_Information()
        {
            // Arrange
            var caseDto = new CaseBuilder().WithDefaultValues()
                .SecondSubCategoryId(1)
                .Build();
            var service = ServiceHelper.CreateCaseService();
            var caseResult = await service.AddAsync(caseDto);
            var receiveJdmentInstrumentDto = new ReceiveJudgmentInstrumentBuilder().WithDefaultValues().Build();
            receiveJdmentInstrumentDto.Id = caseResult.Data.Id;

            // Act
            var result = await service.ReceiveJudmentInstrumentAsync(receiveJdmentInstrumentDto);

            // Assert
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_Judgment_Instrument_Given_Valid_Information()
        {
            // Arrange
            var caseDto = new CaseBuilder().WithDefaultValues()
                .SecondSubCategoryId(1)
                .Build();
            var service = ServiceHelper.CreateCaseService();
            var caseResult = await service.AddAsync(caseDto);
            var receiveJdmentInstrumentDto = new ReceiveJudgmentInstrumentBuilder().WithDefaultValues().Build();
            receiveJdmentInstrumentDto.Id = caseResult.Data.Id;
            await service.ReceiveJudmentInstrumentAsync(receiveJdmentInstrumentDto);
            // Act

            var result = await service.EditReceiveJudmentInstrumentAsync(receiveJdmentInstrumentDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(receiveJdmentInstrumentDto.Id, result.Data.Id);
        }
        [Fact]
        public async void Get_Judgment_Instrument_Given_Valid_Information()
        {
            // Arrange
            var ministrydeptSerice = ServiceHelper.CreateMinistryDepartmentService();
            var ministryDepartments = await ministrydeptSerice.AddAsync(new Core.Dtos.MinistryDepartmentDto
            {
                Name = "العدل"
            });
            var caseDto = new CaseBuilder().WithDefaultValues()
                .SecondSubCategoryId(1)
                .Build();
            var service = ServiceHelper.CreateCaseService();
            var caseResult = await service.AddAsync(caseDto);
            var receiveJdmentInstrumentDto = new ReceiveJudgmentInstrumentBuilder().WithDefaultValues().Build();
            receiveJdmentInstrumentDto.Id = caseResult.Data.Id;
            await service.ReceiveJudmentInstrumentAsync(receiveJdmentInstrumentDto);
            // Act            
            var result = await service.GetReceiveJudmentInstrumentAsync(receiveJdmentInstrumentDto.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(receiveJdmentInstrumentDto.Id, result.Data.Id);
        }
    }
}
