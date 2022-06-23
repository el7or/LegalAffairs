using IronPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/requests")]
    [Authorize]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IChangeResearcherRequestService _changeResearcherRequestService;
        private readonly IChangeResearcherToHearingRequestService _changeResearcherToHearingRequestService;
        private readonly ICaseSupportingDocumentRequestService _documentRequestService;
        private readonly ICaseSupportingDocumentRequestHistoryService _documentRequesthistoryService;
        private readonly IAddingLegalMemoToHearingRequestService _hearingLegalMemoReviewRequestService;
        private readonly IExportCaseJudgmentRequestService _exportCaseJudgmentRequestService;
        private readonly IObjectionPermitRequestService _objectionPermitRequestService;
        private readonly IExportCaseJudgmentRequestHistoryService _exportCaseJudgmentRequestHistoryService;
        private readonly IRequestMoamalatService _requestMoamalatService;
        private readonly IConsultationSupportingDocumentRequestService _ConsultationSupportingDocumentService;
        private readonly IRequestLetterService _requestLetterService;
        private readonly IAddingObjectionLegalMemoToCaseRequestService _objectionLegalMemoRequestService;



        public RequestsController(
            IRequestService requestService,
            IChangeResearcherRequestService changeResearcherRequestService,
            IChangeResearcherToHearingRequestService changeResearcherToHearingRequestService,
            IAddingLegalMemoToHearingRequestService hearingLegalMemoReviewRequestService,
            ICaseSupportingDocumentRequestService documentRequestService,
            IExportCaseJudgmentRequestService exportCaseJudgmentRequestService,
            IConsultationSupportingDocumentRequestService ConsultationSupportingDocumentService,
            IRequestMoamalatService requestMoamalatService,
            IExportCaseJudgmentRequestHistoryService exportCaseJudgmentRequestHistoryService,
            ICaseSupportingDocumentRequestHistoryService caseSupportingDocumentRequestHistoryService,
            IObjectionPermitRequestService objectionPermitRequestService,
            IRequestLetterService requestLetterService,
            IAddingObjectionLegalMemoToCaseRequestService objectionLegalMemoRequestService)
        {
            _requestService = requestService;
            _changeResearcherRequestService = changeResearcherRequestService;
            _changeResearcherToHearingRequestService = changeResearcherToHearingRequestService;
            _documentRequestService = documentRequestService;
            _hearingLegalMemoReviewRequestService = hearingLegalMemoReviewRequestService;
            _exportCaseJudgmentRequestService = exportCaseJudgmentRequestService;
            _ConsultationSupportingDocumentService = ConsultationSupportingDocumentService;
            _requestMoamalatService = requestMoamalatService;
            _exportCaseJudgmentRequestHistoryService = exportCaseJudgmentRequestHistoryService;
            _documentRequesthistoryService = caseSupportingDocumentRequestHistoryService;
            _objectionPermitRequestService = objectionPermitRequestService;
            _requestLetterService = requestLetterService;
            _objectionLegalMemoRequestService = objectionLegalMemoRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(RequestQueryObject queryObject)
        {
            var result = await _requestService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _requestService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("change-researcher-request/{id}")]
        public async Task<IActionResult> GetChangeResearcherRequest(int id)
        {
            var result = await _changeResearcherRequestService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("change-researcher-request")]
        public async Task<IActionResult> CreateChangeResearcherRequest([FromBody] ChangeResearcherRequestDto changeResearcherRequestDto)
        {
            var result = await _changeResearcherRequestService.AddAsync(changeResearcherRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("accept-change-researcher-request")]
        public async Task<IActionResult> AcceptChangeResearcher([FromBody] ReplyChangeResearcherRequestDto replyChangeResearcherRequestDto)
        {
            var result = await _changeResearcherRequestService.AcceptChangeResearcherRequest(replyChangeResearcherRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("reject-change-researcher-request")]
        public async Task<IActionResult> Reject([FromBody] ReplyChangeResearcherRequestDto replyChangeResearcherRequestDto)
        {
            var result = await _changeResearcherRequestService.RejectChangeResearcherRequest(replyChangeResearcherRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("change-hearing-researcher-request/{id}")]
        public async Task<IActionResult> GetChangeResearcherToHearingRequest(int id)
        {
            var result = await _changeResearcherToHearingRequestService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("change-hearing-researcher-request")]
        public async Task<IActionResult> CreateChangeResearcherToHearingRequest([FromBody] ChangeResearcherToHearingRequestDto changeResearcherRequestDto)
        {
            var result = await _changeResearcherToHearingRequestService.AddAsync(changeResearcherRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("accept-change-hearing-researcher-request")]
        public async Task<IActionResult> AcceptChangeResearcherToHearing([FromBody] ReplyChangeResearcherToHearingRequestDto replyChangeResearcherRequestDto)
        {
            var result = await _changeResearcherToHearingRequestService.AcceptChangeResearcherToHearingRequest(replyChangeResearcherRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("reject-change-hearing-researcher-request")]
        public async Task<IActionResult> RejectChangeResearcherToHearing([FromBody] ReplyChangeResearcherToHearingRequestDto replyChangeResearcherRequestDto)
        {
            var result = await _changeResearcherToHearingRequestService.RejectChangeResearcherToHearingRequest(replyChangeResearcherRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("document-request/{id}")]
        public async Task<IActionResult> GetDocumentRequest(int id)
        {
            var result = await _documentRequestService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("attached-letter-request/{id}")]
        public async Task<IActionResult> GetAttachedLetterRequest(int id)
        {
            var result = await _documentRequestService.GetAttachedLetterRequestAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("document-request-history/{id}")]
        public async Task<IActionResult> GetDocumentRequestHistory(int id)
        {
            var result = await _documentRequesthistoryService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("document-request")]
        public async Task<IActionResult> DocumentRequestCreate([FromBody] CaseSupportingDocumentRequestDto documentRequestDto)
        {
            var result = await _documentRequestService.AddAsync(documentRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("document-request")]
        public async Task<IActionResult> DocumentRequestUpdate([FromBody] CaseSupportingDocumentRequestDto documentRequestDto)
        {
            var result = await _documentRequestService.EditAsync(documentRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("reply-document-request")]
        public async Task<IActionResult> ReplyDocumentRequest([FromBody] ReplyCaseSupportingDocumentRequestDto replyDocumentRequestDto)
        {
            var result = await _documentRequestService.ReplyCaseSupportingDocumentRequest(replyDocumentRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("attached-letter-request")]
        [Authorize(Policy = "LegalResearcher")]
        public async Task<IActionResult> AttachedLetterRequestCreate([FromBody] AttachedLetterRequestDto attachedRequestDto)
        {
            var result = await _documentRequestService.AddAttachedLetterRequestAsync(attachedRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("attached-letter-request")]
        [Authorize(Policy = "LegalResearcher")]
        public async Task<IActionResult> AttachedLetterRequestUpdate([FromBody] AttachedLetterRequestDto attachedRequestDto)
        {
            var result = await _documentRequestService.EditAttachedLetterRequestAsync(attachedRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("legal-memo")]
        public async Task<IActionResult> AddLegalMemo([FromBody] AddingLegalMemoToHearingRequestDto hearingLegalMemo)
        {
            var result = await _hearingLegalMemoReviewRequestService.AddAsync(hearingLegalMemo);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("hearing-legal-memo-review-request/{id}")]
        public async Task<IActionResult> GetHearingLegalMemoReviewRequest(int id)
        {
            var result = await _hearingLegalMemoReviewRequestService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("reply-hearing-legal-memo-review-request")]
        public async Task<IActionResult> ReplyHearingLegalMemoReviewRequest([FromBody] ReplyAddingLegalMemoToHearingRequestDto replyAddingLegalMemoToHearingRequest)
        {
            var result = await _hearingLegalMemoReviewRequestService.ReplyAddingMemoHearingRequest(replyAddingLegalMemoToHearingRequest);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("print-document-request/{id}")]
        public async Task<IActionResult> PrintDocumentRequest(int id)
        {

            var documentRequest = (await _documentRequestService.GetAsync(id)).Data;

            License.LicenseKey = "IRONPDF.SMARTFINGERSFORIT.IRO210218.6534.32144.812012-D39F0D0BC6-MOWOPFLN4ZL6A-EFYQABQP3ZNE-YCW5VYEFHU5Z-SK3736X45P77-ZKKDZSRW6NVP-RDW4EC-LSVWNSUZYLWEEA-PRO.1DEV.1YR-6DVKWT.RENEW.SUPPORT.18.FEB.2022";

            var request = await _documentRequestService.GetForPrintAsync(documentRequest.Id);
            var Renderer = new HtmlToPdf();
            Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.MarginTop = 30;
            Renderer.PrintOptions.MarginBottom = 30;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.CustomCssUrl = "wwwroot/css/print-style.css";
            Renderer.PrintOptions.InputEncoding = System.Text.Encoding.UTF8;
            Renderer.PrintOptions.Title = documentRequest.Id.ToString();

            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {
                HtmlFragment = StringExtensions.SharedHeader()
            };
            Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            {
                RightText = "{date}",
                LeftText = "الصفحة رقم {page} من {total-pages}",
                DrawDividerLine = true,
                FontSize = 12,
                FontFamily = "Traditional Arabic"
            };

            var requestLetter = await _requestLetterService.GetAsync(documentRequest.Id);
            string content = requestLetter.Data.Text;

            string htmlContent =
                @"
                   <div class='text-right details-page'>
                        <p class='request-body'>
                          <div class='list-item'>
                             " + content + @"
                        <h2 style='font-size: 1em;font-weight: bold;'>المستندات</h2>
                        <ol>";
            foreach (var document in request.Documents)
            {
                htmlContent += @"<li style='font-size: 1em;'>" + document.Name + @"</li>";
            }

            htmlContent += @"</ol><p class='footer-text'> وتقبلوا أطيب تحياتي ،،،</p></div></p>";
            var mainDiv = "";
            var signaturesContent = "";

            if (documentRequest.Request.RequestStatus.Id == (int)RequestStatuses.Approved)
            {
                var approveUsers = await _requestService.GetRequestApproveUsers(documentRequest.Id);
                var BranchManagerUser = approveUsers.Count > 0 ? approveUsers.Last() : null;
                 

                if (approveUsers.Count > 0)
                {
                    mainDiv = @"<div class='sigRow'>";
                    signaturesContent = "";

                    foreach (var user in approveUsers.Take(3))
                    {
                        if (!string.IsNullOrEmpty(user.Signature))
                        {
                            signaturesContent += @"<div class='sigColumn'><div class='section-header-arabic'>" + user.FirstName + " " + user.LastName + @"</div><br/>";
                            signaturesContent += @"<div class='list-item'><img style='width:220px;height:160px;' src='" + user.Signature + "'/></div><br/></div>";
                        }
                    }

                    signaturesContent += @"</div>";
                }

                if (BranchManagerUser != null)
                {
                    signaturesContent += @"<br/><br/><div class='content-left'>
                    <p class='footer-text'>المشرف العام </p>
                    <p class='footer-text'> على الإدارة العامة للشؤون القانونية </p>
                    <p class='footer-text'>" + BranchManagerUser.FirstName + " " + BranchManagerUser.LastName + @"</p>
                    <p class='footer-text'>" + "<img style='width:220px;height:160px;' src='" + BranchManagerUser.Signature + "'/>" + @"</p>
                </div>";
                }


            }
            htmlContent += mainDiv + signaturesContent + @"</div>";

            Renderer.RenderHtmlAsPdf(htmlContent);

            var PDF = Renderer.RenderHtmlAsPdf(htmlContent).BinaryData;

            return new FileContentResult(PDF, "application/pdf");
        }

        [HttpGet("print-attached-letter-request/{id}")]
        public async Task<IActionResult> PrintAttachedLetterRequest(int id)
        {
            License.LicenseKey = "IRONPDF.SMARTFINGERSFORIT.IRO210218.6534.32144.812012-D39F0D0BC6-MOWOPFLN4ZL6A-EFYQABQP3ZNE-YCW5VYEFHU5Z-SK3736X45P77-ZKKDZSRW6NVP-RDW4EC-LSVWNSUZYLWEEA-PRO.1DEV.1YR-6DVKWT.RENEW.SUPPORT.18.FEB.2022";

            var documentRequest = (await _documentRequestService.GetAsync(id)).Data;

            var Renderer = new HtmlToPdf();
            Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.MarginTop = 30;
            Renderer.PrintOptions.MarginBottom = 30;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.CustomCssUrl = "wwwroot/css/print-style.css";
            Renderer.PrintOptions.InputEncoding = System.Text.Encoding.UTF8;
            Renderer.PrintOptions.Title = documentRequest.Id.ToString();

            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {
                HtmlFragment = StringExtensions.SharedHeader()
            };
            Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            {
                RightText = "{date}",
                LeftText = "الصفحة رقم {page} من {total-pages}",
                DrawDividerLine = true,
                FontSize = 12,
                FontFamily = "Traditional Arabic"
            };

            var requestLetter = await _requestLetterService.GetAsync(documentRequest.Id);
            string content = requestLetter.Data.Text;


            string htmlContent =
                @"
                    <div class='text-right details-page'>
                        <p class='request-body'><div class='list-item'>
                             " + content + @"<p class='footer-text'> وتقبلوا أطيب تحياتي ،،،</p></div></p>";
            var mainDiv = "";
            var signaturesContent = "";
            if (documentRequest.Request.RequestStatus.Id == (int)RequestStatuses.Approved)
            {
                var approveUsers = await _requestService.GetRequestApproveUsers(documentRequest.Id);
                var BranchManagerUser = approveUsers.Count > 0 ? approveUsers.Last() : null;


                if (BranchManagerUser != null)
                {
                    signaturesContent += @"<br/><br/><div class='content-left'>
                    <p class='footer-text'>المشرف العام </p>
                    <p class='footer-text'> على الإدارة العامة للشؤون القانونية </p>
                    <p class='footer-text'>" + BranchManagerUser.FirstName + " " + BranchManagerUser.LastName + @"</p>
                    <p class='footer-text'>" + "<img style='width:220px;height:160px;' src='" + BranchManagerUser.Signature + "'/>" + @"</p>
                </div>";
                }


            }
            htmlContent += mainDiv + signaturesContent + @"</div></div></div>";

            var PDF = Renderer.RenderHtmlAsPdf(htmlContent).BinaryData;

            return new FileContentResult(PDF, "application/pdf");
        }

        [HttpPost("print-export-case-judgment-request/{id}")]
        public async Task<IActionResult> PrintExportCaseJudgmentRequest(int id)
        {
            License.LicenseKey = "IRONPDF.SMARTFINGERSFORIT.IRO210218.6534.32144.812012-D39F0D0BC6-MOWOPFLN4ZL6A-EFYQABQP3ZNE-YCW5VYEFHU5Z-SK3736X45P77-ZKKDZSRW6NVP-RDW4EC-LSVWNSUZYLWEEA-PRO.1DEV.1YR-6DVKWT.RENEW.SUPPORT.18.FEB.2022";

            //var request = await _exportCaseJudgmentRequestService.GetForPrintAsync(id);
            var Renderer = new HtmlToPdf();
            Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.MarginTop = 30;
            Renderer.PrintOptions.MarginBottom = 30;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.CustomCssUrl = "wwwroot/css/print-style.css";
            Renderer.PrintOptions.InputEncoding = System.Text.Encoding.UTF8;
            Renderer.PrintOptions.Title = id.ToString();

            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {
                HtmlFragment = StringExtensions.SharedHeader()
            };
            Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            {
                RightText = "{date}",
                LeftText = "الصفحة رقم {page} من {total-pages}",
                DrawDividerLine = true,
                FontSize = 12,
                FontFamily = "Traditional Arabic"
            };

            var requestLetter = await _requestLetterService.GetAsync(id);
            var requestDetails = await _requestService.GetAsync(id);
            string content = requestLetter.Data.Text;


            string htmlContent =
               @"
                    <div class='text-right details-page'>
                        <p class='request-body'><div class='list-item'>
                             " + content + @"<p class='footer-text'> وتقبلوا أطيب تحياتي ،،،</p></div></p>";

            var mainDiv = "";
            var signaturesContent = "";

            if (requestDetails.Data.RequestStatus.Id == (int)RequestStatuses.Approved)
            {
                var approveUsers = await _requestService.GetRequestApproveUsers(id);
                var BranchManagerUser = approveUsers.Count > 0 ? approveUsers.Last() : null;


                if (approveUsers.Count > 0)
                {
                    mainDiv = @"<div class='sigRow'>";
                    signaturesContent = "";

                    foreach (var user in approveUsers.Take(3))
                    {
                        if (!string.IsNullOrEmpty(user.Signature))
                        {
                            signaturesContent += @"<div class='sigColumn'><div class='section-header-arabic'>" + user.FirstName + " " + user.LastName + @"</div><br/>";
                            signaturesContent += @"<div class='list-item'><img style='width:220px;height:160px;' src='" + user.Signature + "'/></div><br/></div>";
                        }
                    }

                    signaturesContent += @"</div>";
                }

                if (BranchManagerUser != null)
                {
                    signaturesContent += @"<br/><br/><div class='content-left'>
                    <p class='footer-text'>المشرف العام </p>
                    <p class='footer-text'> على الإدارة العامة للشؤون القانونية </p>
                    <p class='footer-text'>" + BranchManagerUser.FirstName + " " + BranchManagerUser.LastName + @"</p>
                    <p class='footer-text'>" + "<img style='width:220px;height:160px;' src='" + BranchManagerUser.Signature + "'/>" + @"</p>
                </div>";
                }


                htmlContent += mainDiv + signaturesContent + @"</div>";

            }
            var PDF = Renderer.RenderHtmlAsPdf(htmlContent).BinaryData;

            return new FileContentResult(PDF, "application/pdf");
        }

        [HttpPost("export-case-judgment-request")]
        public async Task<IActionResult> CreateExportCaseJudgmentRequest([FromBody] ExportCaseJudgmentRequestDto exportCaseJudgmentRequestDto)
        {
            var result = await _exportCaseJudgmentRequestService.AddAsync(exportCaseJudgmentRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("export-case-judgment-request")]
        public async Task<IActionResult> UpdateExportCaseJudgmentRequest([FromBody] ExportCaseJudgmentRequestDto exportCaseJudgmentRequestDto)
        {
            var result = await _exportCaseJudgmentRequestService.EditAsync(exportCaseJudgmentRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("export-case-judgment-request/{id}")]
        public async Task<IActionResult> GetExportCaseJudgmentRequest(int id)
        {
            var result = await _exportCaseJudgmentRequestService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("export-case-judgment-request-history/{id}")]
        public async Task<IActionResult> GetExportCaseJudgmentRequestHistory(int id)
        {
            var result = await _exportCaseJudgmentRequestHistoryService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("reply-export-case-judgment-request")]
        public async Task<IActionResult> ReplyCaseClosingRequest([FromBody] ReplyExportCaseJudgmentRequestDto replyExportCaseJudgmentRequestDto)
        {
            var result = await _exportCaseJudgmentRequestService.ReplyExportCaseJudgmentRequest(replyExportCaseJudgmentRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("case-closing-reasons")]
        public IActionResult GetCaseClosingReasons()
        {
            return Ok(EnumExtensions.GetValues<CaseClosinReasons>());
        }

        [HttpPut("export-request")]
        public async Task<IActionResult> ExportRequest([FromBody] ExportRequestDto exportRequestDto)
        {
            var result = await _requestMoamalatService.ExportRequest(exportRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("print-request")]
        public async Task<IActionResult> PrintRequest([FromBody] RequestListItemDto request)
        {
            License.LicenseKey = "IRONPDF.SMARTFINGERSFORIT.IRO210218.6534.32144.812012-D39F0D0BC6-MOWOPFLN4ZL6A-EFYQABQP3ZNE-YCW5VYEFHU5Z-SK3736X45P77-ZKKDZSRW6NVP-RDW4EC-LSVWNSUZYLWEEA-PRO.1DEV.1YR-6DVKWT.RENEW.SUPPORT.18.FEB.2022";

            var _request = await _requestService.GetForPrintAsync(request.Id);


            var Renderer = new HtmlToPdf();
            Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.MarginTop = 30;
            Renderer.PrintOptions.MarginBottom = 30;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.CustomCssUrl = "wwwroot/css/print-style.css";
            Renderer.PrintOptions.InputEncoding = System.Text.Encoding.UTF8;
            Renderer.PrintOptions.Title = request.Id.ToString();

            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {
                HtmlFragment = StringExtensions.SharedHeader()
            };
            Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            {
                RightText = "{date}",
                LeftText = "الصفحة رقم {page} من {total-pages}",
                DrawDividerLine = true,
                FontSize = 12,
                FontFamily = "Traditional Arabic"
            };
            string htmlContent = @"<div class='page-header'><h2>طلب رقم " + _request.Id + @"</h2></div>
                    <div class='text-right details-page'>
                        <p class='request-body'>
                              اشارة الى الدعوى رقم </p> 
                        
                    <p class='footer-text'>تقبلوا أطيب تحياتي </p> 

                    </div>
                     ";

            var PDF = Renderer.RenderHtmlAsPdf(htmlContent).BinaryData;

            return new FileContentResult(PDF, "application/pdf");
        }

        [HttpGet("consultation-request/{id}")]
        public async Task<IActionResult> GetConsultationSupportingDocument(int id)
        {
            var result = await _ConsultationSupportingDocumentService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("consultation-request")]
        public async Task<IActionResult> ConsultationSupportingDocumentCreate([FromBody] ConsultationSupportingDocumentRequestDto ConsultationSupportingDocumentDto)
        {
            var result = await _ConsultationSupportingDocumentService.AddAsync(ConsultationSupportingDocumentDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("consultation-request")]
        public async Task<IActionResult> ConsultationSupportingDocumentUpdate([FromBody] ConsultationSupportingDocumentRequestDto ConsultationSupportingDocumentDto)
        {
            var result = await _ConsultationSupportingDocumentService.EditAsync(ConsultationSupportingDocumentDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("reply-consultation-request")]
        public async Task<IActionResult> ReplyConsultationSupportingDocument([FromBody] ReplyConsultationSupportingDocumentDto replyConsultationSupportingDocumentDto)
        {
            var result = await _ConsultationSupportingDocumentService.ReplyConsultationSupportingDocument(replyConsultationSupportingDocumentDto);
            return StatusCode((int)result.StatusCode, result);
        }

        #region objection-permit-request

        [HttpGet("objection-permit-request/{id}")]
        public async Task<IActionResult> GetObjectionPermitRequest(int id)
        {
            var result = await _objectionPermitRequestService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("case-objection-permit-request/{caseId}")]
        public async Task<IActionResult> GetCaseObjectionPermitRequest(int caseId)
        {
            var result = await _objectionPermitRequestService.GetByCaseAsync(caseId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("objection-permit-request")]
        public async Task<IActionResult> CreateObjectionPermitRequest([FromBody] ObjectionPermitRequestDto ObjectionPermitRequestDto)
        {
            var result = await _objectionPermitRequestService.AddAsync(ObjectionPermitRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("reply-objection-permit-request")]
        public async Task<IActionResult> ReplyObjectionPermitRequest([FromBody] ReplyObjectionPermitRequestDto replyObjectionPermitRequestDto)
        {
            var result = await _objectionPermitRequestService.ReplyObjectionPermitRequest(replyObjectionPermitRequestDto);
            return StatusCode((int)result.StatusCode, result);
        }

        #endregion 

        [HttpGet("objection-legal-memo-request/{id}")]
        public async Task<IActionResult> GetObjectionLegalMemoRequest(int id)
        {
            var result = await _objectionLegalMemoRequestService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("case-objection-legal-memo-request/{caseId}")]
        public async Task<IActionResult> GetCaseObjectionLegalMemoRequest(int caseId)
        {
            var result = await _objectionLegalMemoRequestService.GetByCaseAsync(caseId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("add-objection-legal-memo")]
        public async Task<IActionResult> AddObjectionLegalMemo([FromBody] AddingObjectionLegalMemoToCaseRequestDto objectionLegalMemo)
        {
            var result = await _objectionLegalMemoRequestService.AddAsync(objectionLegalMemo);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("reply-objection-legal-memo-request")]
        public async Task<IActionResult> ReplyObjectionLegalMemoRequest([FromBody] ReplyObjectionLegalMemoRequestDto replyObjectionLegalMemoequestDto)
        {
            var result = await _objectionLegalMemoRequestService.ReplyObjectionLegalMemoRequest(replyObjectionLegalMemoequestDto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
