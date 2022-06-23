using AutoMapper;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Dtos.Requests;
using Moe.La.Core.Entities;
using Moe.La.Core.Entities.RequestsHistory;
using Moe.La.Core.Enums;
using System;
using System.Linq;

namespace Moe.La.Core.Mapping
{
    class RequestMappingProfile : Profile
    {
        public RequestMappingProfile()
        {
            CreateMap<Request, RequestListItemDto>()
                .ForMember(res => res.RequestType, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.RequestType, Name = EnumExtensions.GetDescription(c.RequestType) }))
                .ForMember(res => res.RequestStatus, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.RequestStatus, Name = EnumExtensions.GetDescription(c.RequestStatus) }))
                .ForMember(res => res.LastTransactionDate, opt => opt.MapFrom(c => c.RequestTransactions.LastOrDefault().CreatedOn))
                .ForMember(res => res.LastTransactionDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.RequestTransactions.LastOrDefault().CreatedOn)))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.UpdatedBy, opt => opt.MapFrom(c => new KeyValuePairsDto<Guid> { Id = c.UpdatedByUser.Id, Name = c.UpdatedByUser.FirstName + " " + c.UpdatedByUser.LastName }))
                .ForMember(res => res.CreatedBy, opt => opt.MapFrom(c => new KeyValuePairsDto<Guid> { Id = c.CreatedByUser.Id, Name = c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName }));

            CreateMap<Request, RequestDetailsDto>()
                .ForMember(res => res.RequestType, opt => opt.MapFrom(c => new { id = (int)c.RequestType, name = EnumExtensions.GetDescription(c.RequestType) }))
                .ForMember(res => res.RequestStatus, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.RequestStatus, Name = EnumExtensions.GetDescription(c.RequestStatus) }))
                .ForMember(res => res.CreatedByFullName, opt => opt.MapFrom(c => c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.LastTransactionDate, opt => opt.MapFrom(c => c.RequestTransactions.LastOrDefault().CreatedOn))
                .ForMember(res => res.LastTransactionDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.RequestTransactions.LastOrDefault().CreatedOn)));

            CreateMap<RequestDto, Request>()
                .ForMember(res => res.Id, opt => opt.Ignore())
                //.ForMember(res => res.ChangeResearcherRequest, opt => opt.Ignore())
                .ReverseMap();
            //.AfterMap((requestRes, request) =>
            //{
            //	//check ChangeResearcherRequest is new
            //	if (request.ChangeResearcherRequest != null && request.ChangeResearcherRequest.Id > 1)
            //		requestRes.ChangeResearcherRequest = new ChangeResearcherRequest
            //		{
            //			RequestId = request.Id,
            //			ReplyDate = request.ChangeResearcherRequest.ReplyDate,
            //			ReplyNote = request.ChangeResearcherRequest.ReplyNote
            //		};

            //});

            CreateMap<ChangeResearcherRequest, ChangeResearcherRequestDetailsDto>()
                .ReverseMap();

            CreateMap<ChangeResearcherRequestDto, ChangeResearcherRequest>()
                .ReverseMap();

            CreateMap<ChangeResearcherRequest, ChangeResearcherRequestListItemDto>()
                .ForMember(res => res.ReplyDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.ReplyDate)))
                .ForMember(res => res.CaseNumberInSource, opt => opt.MapFrom(c => c.Case.CaseNumberInSource))
                .ForMember(res => res.CaseSource, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.Case.CaseSource)))
                .ReverseMap();

            CreateMap<Request, RequestForPrintDto>()
                .ForMember(res => res.RequestType, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.RequestType, Name = EnumExtensions.GetDescription(c.RequestType) }))
                .ForMember(res => res.RequestStatus, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.RequestStatus, Name = EnumExtensions.GetDescription(c.RequestStatus) }))
                .ForMember(res => res.LastTransactionDate, opt => opt.MapFrom(c => c.RequestTransactions.LastOrDefault().CreatedOn))
                .ForMember(res => res.LastTransactionDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.RequestTransactions.LastOrDefault().CreatedOn)))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.CreatedBy, opt => opt.MapFrom(c => new KeyValuePairsDto<Guid> { Id = c.CreatedByUser.Id, Name = c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName }));

            #region ChangeResearcherToHearingRequest

            CreateMap<ChangeResearcherToHearingRequest, ChangeResearcherToHearingRequestDetailsDto>()
               .ReverseMap();

            CreateMap<ChangeResearcherToHearingRequestDto, ChangeResearcherToHearingRequest>()
                .ReverseMap();

            CreateMap<ChangeResearcherToHearingRequest, ChangeResearcherToHearingRequestListItemDto>()
                .ForMember(res => res.ReplyDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.ReplyDate)))
                .ForMember(res => res.CaseNumberInSource, opt => opt.MapFrom(c => c.Hearing.Case.CaseNumberInSource))
                .ForMember(res => res.CaseSource, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.Hearing.Case.CaseSource)))
                .ReverseMap();

            #endregion

            #region RequestNote

            CreateMap<RequestNote, RequestNoteListItemDto>()
                .ForMember(res => res.CreatedBy, opt => opt.MapFrom(a => a.CreatedByUser.FirstName + ' ' + a.CreatedByUser.LastName))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)));

            CreateMap<RequestNoteDto, RequestNote>()
                .ForMember(l => l.Id, opt => opt.Ignore())
                .ReverseMap();

            #endregion

            #region HearingLegalMemoReviewRequest 
            CreateMap<AddingLegalMemoToHearingRequest, AddingLegalMemoToHearingRequestListItemDto>()
                .ForMember(res => res.ReplyDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.ReplyDate)));
            ;
            CreateMap<AddingLegalMemoToHearingRequestDto, AddingLegalMemoToHearingRequest>();
            CreateMap<AddingLegalMemoToHearingRequest, AddingLegalMemoToHearingRequestDto>();
            CreateMap<AddingLegalMemoToHearingRequest, ReplyAddingLegalMemoToHearingRequestDto>()
                .ForMember(res => res.RequestStatus, opt => opt.MapFrom(req => req.Request.RequestStatus))
                .ReverseMap();
            CreateMap<HearingLegalMemo, HearingLegalMemoDto>().ReverseMap();
            #endregion

            #region ObjectionLegalMemoRequest
            CreateMap<AddingObjectionLegalMemoToCaseRequest, AddingObjectionLegalMemoToCaseRequestDto>().ReverseMap();
            #endregion

            #region Request history

            CreateMap<Request, RequestHistory>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CaseSupportingDocumentRequest, CaseSupportingDocumentRequestHistory>()
                .ForMember(m => m.CaseSupportingDocumentRequestId, opt => opt.MapFrom(d => d.Id))
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ExportCaseJudgmentRequest, ExportCaseJudgmentRequestHistory>()
                .ForMember(m => m.ExportCaseJudgmentRequestId, opt => opt.MapFrom(d => d.Id))
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.Case, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CaseSupportingDocumentRequestItem, CaseSupportingDocumentRequestItemHistory>()
                .ForMember(m => m.CaseSupportingDocumentRequestId, opt => opt.Ignore())
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RequestLetter, RequestLetterHistory>()
                .ForMember(m => m.RequestId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RequestHistory, RequestListItemDto>()
               .ForMember(res => res.RequestType, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.RequestType, Name = EnumExtensions.GetDescription(c.RequestType) }))
               .ForMember(res => res.RequestStatus, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.RequestStatus, Name = EnumExtensions.GetDescription(c.RequestStatus) }))
               .ForMember(res => res.LastTransactionDate, opt => opt.Ignore())
               .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
               .ForMember(res => res.CreatedBy, opt => opt.MapFrom(c => new KeyValuePairsDto<Guid> { Id = c.CreatedByUser.Id, Name = c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName }))
               .ForMember(res => res.UpdatedBy, opt => opt.MapFrom(c => new KeyValuePairsDto<Guid> { Id = c.UpdatedByUser.Id, Name = c.UpdatedByUser.FirstName + " " + c.UpdatedByUser.LastName }));

            CreateMap<CaseSupportingDocumentRequestHistory, CaseSupportingDocumentRequestHistoryListItemDto>()
                .ForMember(res => res.AttachedLetterRequestCount, opt => opt.Ignore())
                .ForMember(res => res.ConsigneeDepartment, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.ConsigneeDepartmentId, Name = c.ConsigneeDepartment.Name }))
                .ReverseMap();

            CreateMap<ExportCaseJudgmentRequestHistory, ExportCaseJudgmentRequestHistoryListItemDto>()
                .ReverseMap();

            CreateMap<CaseSupportingDocumentRequestItemHistory, CaseSupportingDocumentRequestItemDto>()
            .ForMember(res => res.Id, opt => opt.Ignore());

            CreateMap<RequestLetterHistory, RequestLetterHistoryDto>()
            .ForMember(res => res.RequestId, opt => opt.Ignore());
            #endregion

            #region Document Request
            CreateMap<CaseSupportingDocumentRequestDto, CaseSupportingDocumentRequest>()
                .ReverseMap();

            CreateMap<CaseSupportingDocumentRequest, CaseSupportingDocumentRequestListItemDto>()
                .ForMember(res => res.AttachedLetterRequestCount, opt => opt.Ignore())
                .ForMember(res => res.AttachedLetterRequestStatus, opt => opt.Ignore())
                .ForMember(res => res.ConsigneeDepartment, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.ConsigneeDepartmentId, Name = c.ConsigneeDepartment.Name }))
                .ForMember(res => res.CaseNumberInSource, opt => opt.MapFrom(c => c.Request.RequestType == RequestTypes.RequestSupportingDocuments ? c.Case.CaseNumberInSource : c.Parent.Case.CaseNumberInSource))
                .ForMember(res => res.CaseYearInSourceHijri, opt => opt.MapFrom(c => c.Request.RequestType == RequestTypes.RequestSupportingDocuments ? DateTimeHelper.GetHigriYearInt(c.Case.StartDate) : DateTimeHelper.GetHigriYearInt(c.Parent.Case.StartDate)))
                .ForMember(res => res.CaseYearInSource, opt => opt.MapFrom(c => c.Case.CaseYearInSource))
                .ForMember(res => res.CaseSource, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.Case.CaseSource)))
                .ReverseMap();

            CreateMap<CaseSupportingDocumentRequestItemDto, CaseSupportingDocumentRequestItem>()
                .ForMember(res => res.Id, opt => opt.Ignore())
                .ForMember(res => res.CaseSupportingDocumentRequest, opt => opt.Ignore());

            CreateMap<CaseSupportingDocumentRequest, ReplyCaseSupportingDocumentRequestDto>()
                .ForMember(res => res.RequestStatus, opt => opt.MapFrom(req => req.Request.RequestStatus))
                .ReverseMap();

            CreateMap<CaseSupportingDocumentRequest, CaseSupportingDocumentRequestForPrintDto>()
               .ForMember(res => res.CaseId, opt => opt.MapFrom(req => req.Case.Id))
               .ForMember(res => res.CaseDate, opt => opt.MapFrom(req => req.Case.CreatedOn))
               .ForMember(res => res.Defendant, opt => opt.MapFrom(req =>
                    req.Case.LegalStatus == MinistryLegalStatuses.Defendant ? "الوزارة" : String.Join(", ",
                    req.Case.Parties.Select(p => p.Party.Name).ToList())))
               .ForMember(res => res.Plaintiff, opt => opt.MapFrom(req =>
                    req.Case.LegalStatus == MinistryLegalStatuses.Plaintiff ? "الوزارة" : String.Join(", ",
                    req.Case.Parties.Select(p => p.Party.Name).ToList())))
              .ReverseMap();
            CreateMap<ExportCaseJudgmentRequest, ExportCaseJudgmentRequestForPrintDto>()
               .ForMember(res => res.CaseId, opt => opt.MapFrom(req => req.Case.Id))
               .ForMember(res => res.RequestLetter, opt => opt.MapFrom(req => req.Request.Letter.Text))
               .ReverseMap();

            #endregion

            #region Attached Letter Request
            CreateMap<AttachedLetterRequestDto, CaseSupportingDocumentRequest>();

            CreateMap<CaseSupportingDocumentRequest, AttachedLetterRequestDto>()
                .ForMember(res => res.CaseNumberInSource, opt => opt.MapFrom(r => r.Parent.Case.CaseNumberInSource))
                .ForMember(res => res.CaseYearInSourceHijri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriYearInt(c.Parent.Case.StartDate)))
                .ForMember(res => res.CaseSource, opt => opt.MapFrom(r => EnumExtensions.GetDescription(r.Parent.Case.CaseSource)));
            #endregion

            #region Case Closing Request

            CreateMap<ExportCaseJudgmentRequest, ExportCaseJudgmentRequestDetailsDto>()
                .ReverseMap();

            CreateMap<ExportCaseJudgmentRequestDto, ExportCaseJudgmentRequest>()
                .ForMember(res => res.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ExportCaseJudgmentRequest, ExportCaseJudgmentRequestDto>();
            //.ReverseMap();

            CreateMap<ExportCaseJudgmentRequest, ExportCaseJudgmentRequestListItemDto>()
                .ForMember(res => res.CaseNumberInSource, opt => opt.MapFrom(r => r.Case.CaseNumberInSource))
                .ForMember(res => res.CaseSource, opt => opt.MapFrom(r => EnumExtensions.GetDescription(r.Case.CaseSource)))
                .ReverseMap();

            CreateMap<ExportCaseJudgmentRequest, ReplyExportCaseJudgmentRequestDto>()
               .ForMember(res => res.RequestStatus, opt => opt.MapFrom(req => req.Request.RequestStatus))
               .ReverseMap();
            #endregion

            #region RequestMoamala

            CreateMap<RequestMoamalatDto, RequestsMoamalat>()
                .ForMember(res => res.Id, opt => opt.Ignore());
            #endregion

            #region ConsultationSupportingDocument

            CreateMap<ConsultationSupportingDocumentRequestDto, ConsultationSupportingDocumentRequest>();
            CreateMap<ConsultationSupportingDocumentRequest, ConsultationSupportingDocumentRequestDto>();
            CreateMap<ConsultationSupportingDocumentRequest, ConsultationSupportingDocumentListItemDto>();
            #endregion

            CreateMap<RequestLetterDto, RequestLetter>();

            CreateMap<RequestLetter, RequestLetterDto>()
                .ForMember(r => r.RequestStatus, opt => opt.Ignore());

            CreateMap<RequestLetter, RequestLetterDetailsDto>();
            CreateMap<RequestLetterDetailsDto, RequestLetter>();

            #region objection-permit-Request

            CreateMap<ObjectionPermitRequest, ObjectionPermitRequestDetailsDto>()
                .ReverseMap();

            CreateMap<ObjectionPermitRequestDto, ObjectionPermitRequest>()
                .ForMember(res => res.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ObjectionPermitRequest, ObjectionPermitRequestDto>();

            CreateMap<ObjectionPermitRequest, ObjectionPermitRequestListItemDto>()
                .ForMember(res => res.SuggestedOpinon, opt => opt.MapFrom(c => new KeyValuePairsDto<int> { Id = (int)c.SuggestedOpinon, Name = EnumExtensions.GetDescription(c.SuggestedOpinon) }))
                .ForMember(res => res.CaseNumberInSource, opt => opt.MapFrom(r => r.Case.CaseNumberInSource))
                .ForMember(res => res.CaseYearInSourceHijri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriYearInt(c.Case.StartDate)))
                .ForMember(res => res.CaseSource, opt => opt.MapFrom(r => EnumExtensions.GetDescription(r.Case.CaseSource)));
            CreateMap<ObjectionPermitRequestListItemDto, ObjectionPermitRequest>();

            CreateMap<ObjectionPermitRequest, ReplyObjectionPermitRequestDto>()
               .ForMember(res => res.RequestStatus, opt => opt.MapFrom(req => req.Request.RequestStatus))
               .ReverseMap();
            #endregion

        }
    }
}
