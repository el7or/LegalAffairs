using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System;

namespace Moe.La.UnitTests.Builders
{
    public class RequestBuilder
    {
        private RequestDto _request = new RequestDto();

        public RequestBuilder Id(int id)
        {
            _request.Id = id;
            return this;
        }

        public RequestBuilder RequestType(RequestTypes requestType)
        {
            _request.RequestType = requestType;
            return this;
        }

        public RequestBuilder RequestStatus(RequestStatuses requestStatus)
        {
            _request.RequestStatus = requestStatus;
            return this;
        }

        //public RequestBuilder CaseId(int? caseId)
        //{
        //    _request.CaseId = caseId;
        //    return this;
        //}

        public RequestBuilder SendingType(SendingTypes sendingType)
        {
            _request.SendingType = sendingType;
            return this;
        }

        public RequestBuilder ReceiverId(Guid? receiverId)
        {
            _request.ReceiverId = receiverId;
            return this;
        }

        public RequestBuilder ReceiverGeneralManagementId(int? receiverGeneralManagementId)
        {
            _request.ReceiverBranchId = receiverGeneralManagementId;
            return this;
        }

        public RequestBuilder ReceiverDepartmentId(int? receiverDepartmentId)
        {
            _request.ReceiverDepartmentId = receiverDepartmentId;
            return this;
        }

        public RequestBuilder ReceiverRoleId(Guid? receiverRoleId)
        {
            _request.ReceiverRoleId = receiverRoleId;
            return this;
        }

        public RequestBuilder RelatedRequestId(int? relatedRequestId)
        {
            _request.RelatedRequestId = relatedRequestId;
            return this;
        }

        public RequestBuilder Note(string note)
        {
            _request.Note = note;
            return this;
        }

        public RequestBuilder WithDefaultValues()
        {
            _request = new RequestDto
            {
                Id = 0,
                Note = "This is a test note",
                RequestStatus = RequestStatuses.New,
                ReceiverId = ApplicationConstants.SystemAdministratorId, // Just for now.
                RequestType = RequestTypes.RequestResearcherChange,
                Letter = new RequestLetterBuilder().WithDefaultValues().Build()
            };

            return this;
        }

        public RequestDto Build() => _request;
    }
}
