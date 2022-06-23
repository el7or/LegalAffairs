using IronPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/legal-memos")]
    [Authorize]

    public class LegalMemosController : ControllerBase
    {
        private readonly ILegalMemoService _legalMemoService;

        public LegalMemosController(ILegalMemoService legalMemoService)
        {
            _legalMemoService = legalMemoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(LegalMemoQueryObject queryObject)
        {
            var result = await _legalMemoService.GetAllAsync(queryObject);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _legalMemoService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LegalMemoDto legalMemoDto)
        {
            var result = await _legalMemoService.AddAsync(legalMemoDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LegalMemoDto legalMemoDto)
        {
            var result = await _legalMemoService.EditAsync(legalMemoDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("delete-legal-memo")]
        public async Task<IActionResult> Delete([FromBody] DeletionLegalMemoDto deletionLegalMemoDto)
        {
            var result = await _legalMemoService.RemoveAsync(deletionLegalMemoDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("legal-memo-status")]
        public IActionResult GetLegalMemoStatus()
        {
            return Ok(EnumExtensions.GetValues<LegalMemoStatuses>());
        }

        [HttpGet("legal-memo-types")]
        public IActionResult GetLegalMemoTypes()
        {
            return Ok(EnumExtensions.GetValues<LegalMemoTypes>());
        }

        [HttpPut("change-legal-memo-status")]
        public async Task<IActionResult> ChangeLegalMemoStatus(int legalMemoId, int legalMemoStatusId)
        {
            var result = await _legalMemoService.ChangeLegalMemoStatusAsync(legalMemoId, legalMemoStatusId);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPut("rais-to-boardshead")]
        public async Task<IActionResult> RaisToAllBoardsHead(int id)
        {
            var result = await _legalMemoService.RaisToAllBoardsHeadAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _legalMemoService.ApproveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("reject/{id}")]
        public async Task<IActionResult> Reject(int id, string note)
        {
            var result = await _legalMemoService.RejectAsync(id, note);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("read-legal-memo")]
        public async Task<IActionResult> ReadLegalMemo(int legalMemoId)
        {
            var result = await _legalMemoService.ReadLegalMemoAsync(legalMemoId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("print")]
        public IActionResult PrintLegalMemo([FromBody] LegalMemoDetailsDto legalMemo)
        {
            License.LicenseKey = "IRONPDF.SMARTFINGERSFORIT.IRO210218.6534.32144.812012-D39F0D0BC6-MOWOPFLN4ZL6A-EFYQABQP3ZNE-YCW5VYEFHU5Z-SK3736X45P77-ZKKDZSRW6NVP-RDW4EC-LSVWNSUZYLWEEA-PRO.1DEV.1YR-6DVKWT.RENEW.SUPPORT.18.FEB.2022";

            var Renderer = new HtmlToPdf();
            Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.MarginTop = 30;
            Renderer.PrintOptions.MarginBottom = 30;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.CustomCssUrl = "wwwroot/css/print-style.css";
            Renderer.PrintOptions.InputEncoding = System.Text.Encoding.UTF8;
            Renderer.PrintOptions.Title = legalMemo.Name;
            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {
                HtmlFragment = StringExtensions.SharedHeader(),
                //DrawDividerLine = true

            };
            Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            {
                //"D:\Smart Fingers Work\Project\src\Moe.La.WebApi\PrintFiles\images\MOELogo.png"
                RightText = "{date}",
                LeftText = "الصفحة رقم {page} من {total-pages}",
                DrawDividerLine = true,
                FontSize = 12,
                FontFamily = "Traditional Arabic"
            };
            string htmlContent =
                @"
                    <div class='main-header'><p style='font-size: 1.6em;text-align: center;'>مذكرة " + legalMemo.Name + @"</p></div>
                    <div class='text-right details-page' style='text-align: justify;'>
                    <h2>" + legalMemo.Text + "</h2>";

            byte[] PDF;
            if (legalMemo.Status.Id != (int)LegalMemoStatuses.Approved)
                PDF = Renderer.RenderHtmlAsPdf(htmlContent).AddForegroundOverlayPdf("wwwroot/images/my-background-draft.pdf").BinaryData;
            else
                PDF = Renderer.RenderHtmlAsPdf(htmlContent).BinaryData;

            return new FileContentResult(PDF, "application/pdf");
        }

        [HttpPost("print-hearing-memo")]
        public async Task<IActionResult> PrintHearingMemoLegalMemo([FromBody] HearingLegalMemoDto hearingLegalMemo)
        {
            License.LicenseKey = "IRONPDF.SMARTFINGERSFORIT.IRO210218.6534.32144.812012-D39F0D0BC6-MOWOPFLN4ZL6A-EFYQABQP3ZNE-YCW5VYEFHU5Z-SK3736X45P77-ZKKDZSRW6NVP-RDW4EC-LSVWNSUZYLWEEA-PRO.1DEV.1YR-6DVKWT.RENEW.SUPPORT.18.FEB.2022";
            var result = await _legalMemoService.GetForPrintAsync(hearingLegalMemo.LegalMemoId, hearingLegalMemo.HearingId);

            var Renderer = new HtmlToPdf();
            Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.MarginTop = 30;
            Renderer.PrintOptions.MarginBottom = 30;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.CustomCssUrl = "wwwroot/css/print-style.css";
            Renderer.PrintOptions.InputEncoding = System.Text.Encoding.UTF8;
            Renderer.PrintOptions.Title = hearingLegalMemo.HearingId.ToString();

            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {
                HtmlFragment = StringExtensions.SharedHeader(),
                //DrawDividerLine = true
            };

            Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            {
                //"D:\Smart Fingers Work\Project\src\Moe.La.WebApi\PrintFiles\images\MOELogo.png"
                RightText = "{date}",
                LeftText = "الصفحة رقم {page} من {total-pages}",
                DrawDividerLine = true,
                FontSize = 12,
                FontFamily = "Traditional Arabic"
            };

            var circleNumber = GetCircleNumberInArabic(result.Data.CircleNumber);

            string htmlContent =
                @"
                    <div class='text-right details-page'>
                        <p class='request-body'><div class='list-item'>
                          <div>
                            <div class='header-subtitle-right'> فضيلة رئيس الدائرة الإدارية " + circleNumber + @" ب" + result.Data.Court + @" </div>
                            <div class='header-subtitle-left'> وفقه الله</div>
                          </div><br />
                          <div class='regards-text'>السلام عليكم ورحمة الله وبركاته،</div>
                          <div class='section-arabic'>
                           إشارة إلى الدعوى المقامة أمام " + result.Data.Court + @" برقم " + result.Data.CaseSourceNumber + @" لعام " + DateTimeHelper.GetHigriYearInt(result.Data.StartDate.Value) + @" هـ <br />
                           المقامة من/ " + result.Data.Plaintiff + @" ضد/ " + result.Data.Defendant + @" بشأن " + result.Data.Subject + @" <br />
                           عليه فإننا نورد ما يلى <br />
                           " + result.Data.Text + @"
                          </div>
                          <p class='footer-text'> وتقبلوا أطيب تحياتي ،،،</p></div></p>";

            var signaturesContent = "";
            signaturesContent += @"<br/><br/><div class='content-left'>
                    <p class='footer-text'>ممثل وزارة التعليم </p>
                    <p class='footer-text'> " + result.Data.Researcher + @" </p>
                </div>";

            htmlContent += signaturesContent + @"</div>";

            var PDF = Renderer.RenderHtmlAsPdf(htmlContent).BinaryData;

            return new FileContentResult(PDF, "application/pdf");
        }

        private static string GetCircleNumberInArabic(string circle_Number)
        {
            var circleNumber = "";
            switch (circle_Number)
            {
                case "1":
                    circleNumber = "الأولى";
                    break;
                case "2":
                    circleNumber = "الثانية";
                    break;
                case "3":
                    circleNumber = "الثالثة";
                    break;
                case "4":
                    circleNumber = "الرابعة";
                    break;
                case "5":
                    circleNumber = "الخامسة";
                    break;
                case "6":
                    circleNumber = "السادسة";
                    break;
                case "7":
                    circleNumber = "السابعة";
                    break;
                case "8":
                    circleNumber = "الثامنة";
                    break;
                case "9":
                    circleNumber = "التاسعة";
                    break;
                case "10":
                    circleNumber = "العاشرة";
                    break;
                case "11":
                    circleNumber = "الاحد عشر";
                    break;
                case "12":
                    circleNumber = "الاثنى عشر";
                    break;
                case "13":
                    circleNumber = "الثالثة عشر";
                    break;
                case "14":
                    circleNumber = "الرابعة عشر";
                    break;
                case "15":
                    circleNumber = "الخامسة عشر";
                    break;
                case "16":
                    circleNumber = "السادسة عشر";
                    break;
                case "17":
                    circleNumber = "السابعة عشر";
                    break;
                case "18":
                    circleNumber = "الثامنة عشر";
                    break;
                case "19":
                    circleNumber = "التاسعة عشر";
                    break;
                case "20":
                    circleNumber = "العشرين";
                    break;
                case "21":
                    circleNumber = "الاحد والعشرين";
                    break;
                case "22":
                    circleNumber = "الثانية والعشرين";
                    break;
                case "23":
                    circleNumber = "الثالثة والعشرين";
                    break;
                case "24":
                    circleNumber = "الرابعة والعشرين";
                    break;
                case "25":
                    circleNumber = "الخامسة والعشرين";
                    break;
                case "26":
                    circleNumber = "السادسة والعشرين";
                    break;
                case "27":
                    circleNumber = "السابعة والعشرين";
                    break;
                case "28":
                    circleNumber = "الثامنة والعشرين";
                    break;
                case "29":
                    circleNumber = "التاسعة والعشرين";
                    break;
                case "30":
                    circleNumber = "الثلاثين";
                    break;
            }

            return circleNumber;
        }

        [HttpGet("GetAllByCaseID")]
        public async Task<IActionResult> GetAllByCaseID(int caseId)
        {
            var result = await _legalMemoService.GetAllLegalMemoByCaseIdAsync(caseId);
            return StatusCode((int)result.StatusCode, result);
        }

        #region LegalMemoNotes

        [HttpGet("notes")]
        public async Task<IActionResult> GetNotesAll(LegalMemoNoteQueryObject queryObject)
        {
            var result = await _legalMemoService.GetNotesAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("note/{id}")]
        public async Task<IActionResult> GetNote(int id)
        {
            var result = await _legalMemoService.GetNoteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("create-note")]
        public async Task<IActionResult> CreateNote([FromBody] LegalMemoNoteDto legalMemoNoteDto)
        {
            var result = await _legalMemoService.AddNoteAsync(legalMemoNoteDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("update-note")]
        public async Task<IActionResult> UpdateNote([FromBody] LegalMemoNoteDto legalMemoNoteDto)
        {
            var result = await _legalMemoService.EditNoteAsync(legalMemoNoteDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("note/{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var result = await _legalMemoService.RemoveNoteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region LegalMemoHistory

        [HttpGet("history")]
        public async Task<IActionResult> GetHistoryAll(LegalMemoQueryObject queryObject)
        {
            var result = await _legalMemoService.GetHistoryAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

    }
}
