using Moe.La.UnitTests.Builders;
using Moe.La.UnitTests.Builders.Requests;
using Xunit;

namespace Moe.La.UnitTests
{
    public class DocumentRequestUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Add_Document_Request_Given_Valid_Information()
        {
            //Arrange
            var documentRequest = new DocumentRequestBuilder().WithDefaultValues().Build();
            var service = ServiceHelper.CreateDocumentRequestService(TestUsers.LegalResearcherId);

            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder()
               .ResearcherId(TestUsers.LegalResearcherId)
               .ConsultantId(TestUsers.LegalConsultantId)
               .Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);

            // Act
            var result = await service.AddAsync(documentRequest);

            //Assert
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Add_Attached_Letter_Request_Given_Valid_Information()
        {
            //Arrange
            var attachedLetterRequest = new AttachedLetterRequestBuilder().WithDefaultValues().Build();
            var service = ServiceHelper.CreateDocumentRequestService(TestUsers.LegalResearcherId);

            // Act
            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder()
               .ResearcherId(TestUsers.LegalResearcherId)
               .ConsultantId(TestUsers.LegalConsultantId)
               .Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);


            var result = await service.AddAttachedLetterRequestAsync(attachedLetterRequest);

            //Assert
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Get_Document_Request_By_Id_Given_Valid_Information()
        {
            // Arrange
            var documentRequest = new DocumentRequestBuilder().WithDefaultValues().Build();
            var documentRequestService = ServiceHelper.CreateDocumentRequestService(TestUsers.LegalResearcherId);

            // Act
            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder()
               .ResearcherId(TestUsers.LegalResearcherId)
               .ConsultantId(TestUsers.LegalConsultantId)
               .Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);
            await documentRequestService.AddAsync(documentRequest);
            var result = await documentRequestService.GetAsync(documentRequest.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(documentRequest.Id, result.Data.Id);
        }

        [Fact]
        public async void Get_Attached_Letter_Request_Given_Valid_Information()
        {
            // Arrange

            var documentRequestService = ServiceHelper.CreateDocumentRequestService(TestUsers.LegalResearcherId);
            var attachedLetterRequestDto = new AttachedLetterRequestBuilder().WithDefaultValues().Build();

            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder()
               .ResearcherId(TestUsers.LegalResearcherId)
               .ConsultantId(TestUsers.LegalConsultantId)
               .Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);

            // Act
            var attachedLetterRequest = documentRequestService.AddAttachedLetterRequestAsync(attachedLetterRequestDto);
            var result = await documentRequestService.GetAttachedLetterRequestAsync((int)attachedLetterRequest.Id);

            // Assert
            Assert.True(result.IsSuccess);
            //Assert.Equal(attachedLetterRequest.Id, result.Data.Id);
        }

        [Fact]
        public async void Edit_Document_Request_Given_Valid_Information()
        {
            // Arrange
            var documentRequest = new DocumentRequestBuilder().WithDefaultValues().Build();
            var documentRequestService = ServiceHelper.CreateDocumentRequestService(TestUsers.LegalResearcherId);

            // Act
            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder()
               .ResearcherId(TestUsers.LegalResearcherId)
               .ConsultantId(TestUsers.LegalConsultantId)
               .Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);
            await documentRequestService.AddAsync(documentRequest);

            var result = await documentRequestService.EditAsync(documentRequest);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(documentRequest.ReplyNote, result.Data.ReplyNote);
        }

        [Fact]
        public async void Edit_Attached_Letter_Request_Given_Valid_Information()
        {
            // Arrange
            var attachedLetterRequest = new AttachedLetterRequestBuilder().WithDefaultValues().Build();
            var service = ServiceHelper.CreateDocumentRequestService(TestUsers.LegalResearcherId);

            // Act
            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder()
               .ResearcherId(TestUsers.LegalResearcherId)
               .ConsultantId(TestUsers.LegalConsultantId)
               .Build();

            await researchsConsultantService.AddAsync(researcherConsultantDto);
            await service.AddAttachedLetterRequestAsync(attachedLetterRequest);
            attachedLetterRequest.Request.Note = "This is updating note";
            var result = await service.EditAttachedLetterRequestAsync(attachedLetterRequest);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(attachedLetterRequest.Request.Note, result.Data.Request.Note);
        }

        [Fact]
        public async void Reply_Document_Request_Given_Valid_Information()
        {
            //Arrange
            var documentRequest = new DocumentRequestBuilder().WithDefaultValues().Build();
            var documentRequestService = ServiceHelper.CreateDocumentRequestService(TestUsers.LegalResearcherId);

            // Act
            var researchsConsultantService = ServiceHelper.CreateResearchsConsultantService();
            var researcherConsultantDto = new ResearcherConsultantBuilder()
               .ResearcherId(TestUsers.LegalResearcherId)
               .ConsultantId(TestUsers.LegalConsultantId)
               .Build();
            await researchsConsultantService.AddAsync(researcherConsultantDto);
            await documentRequestService.AddAsync(documentRequest);
            var documentRequestReply = new DocumentRequestBuilder().WithReplyDocumentRequestDefaultValues().BuildReply();
            documentRequestReply.Id = documentRequest.Id;

            var result = await documentRequestService.ReplyCaseSupportingDocumentRequest(documentRequestReply);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(documentRequestReply.RequestStatus, result.Data.Request.RequestStatus);
        }
    }
}
