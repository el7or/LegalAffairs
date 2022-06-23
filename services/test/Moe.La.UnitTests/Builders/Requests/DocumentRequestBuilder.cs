using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System.Collections.Generic;

namespace Moe.La.UnitTests.Builders
{
    public class DocumentRequestBuilder
    {
        private CaseSupportingDocumentRequestDto _documentRequest = new CaseSupportingDocumentRequestDto();
        private ReplyCaseSupportingDocumentRequestDto _replyDocumentRequest = new ReplyCaseSupportingDocumentRequestDto();

        public DocumentRequestBuilder Id(int id)
        {
            _documentRequest.Id = id;
            return this;
        }

        public DocumentRequestBuilder ParentId(int? parentId)
        {
            _documentRequest.ParentId = parentId;
            return this;
        }

        public DocumentRequestBuilder Request(RequestDto request)
        {
            _documentRequest.Request = request;
            return this;
        }

        public DocumentRequestBuilder SenderDepartment(int senderDepartment)
        {
            _documentRequest.ConsigneeDepartmentId = senderDepartment;
            return this;
        }

        public DocumentRequestBuilder ReplyNote(string replyNote)
        {
            _documentRequest.ReplyNote = replyNote;
            return this;
        }

        public DocumentRequestBuilder Documents(ICollection<CaseSupportingDocumentRequestItemDto> documents)
        {
            _documentRequest.Documents = documents;
            return this;
        }

        public DocumentRequestBuilder WithDefaultValues()
        {
            _documentRequest = new CaseSupportingDocumentRequestDto()
            {
                Request = new RequestBuilder().WithDefaultValues().RequestType(RequestTypes.RequestSupportingDocuments).Build(),
                ParentId = null,
                ConsigneeDepartmentId = 1,
                ReplyNote = "This is a test note",
                Documents = null
            };

            return this;
        }

        public DocumentRequestBuilder WithReplyDocumentRequestDefaultValues()
        {
            _replyDocumentRequest = new ReplyCaseSupportingDocumentRequestDto()
            {
                ReplyNote = "test",
                RequestStatus = RequestStatuses.Accepted,
                ConsigneeDepartmentId = 1
            };

            return this;
        }

        public CaseSupportingDocumentRequestDto Build() => _documentRequest;
        public ReplyCaseSupportingDocumentRequestDto BuildReply() => _replyDocumentRequest;

    }
}
