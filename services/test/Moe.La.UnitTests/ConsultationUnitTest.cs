using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class ConsultationUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Add_Consultation_Given_Valid_Information()
        {
            // Arrange
            var consultationDto = new ConsultationBuilder().WithDefaultValues().Build();
            var service = ServiceHelper.CreateConsultationService();

            // Act
            var result = await service.AddAsync(consultationDto);

            // Assert
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Edit_Consultation_Given_Valid_Information()
        {
            // Arrange
            var consultationDto = new ConsultationBuilder().WithDefaultValues().Build();
            var service = ServiceHelper.CreateConsultationService();

            // Act
            await service.AddAsync(consultationDto);
            consultationDto.Subject = "تعديل نموذج";
            consultationDto.LegalAnalysis = "تعديل التحليل";

            var result = await service.EditAsync(consultationDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(consultationDto.Subject, result.Data.Subject);
            Assert.Equal(consultationDto.LegalAnalysis, result.Data.LegalAnalysis);
        }

        [Fact]
        public async void Get_Consultation_By_Id_Given_Valid_Information()
        {
            // Arrange
            var consultationDetailsDto = new ConsultationBuilder().WithDefaultValues().Build();
            var service = ServiceHelper.CreateConsultationService();

            // Act
            await service.AddAsync(consultationDetailsDto);
            var result = await service.GetAsync((int)consultationDetailsDto.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(consultationDetailsDto.Id, result.Data.Id);
        }

    }
}
