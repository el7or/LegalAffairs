using IronPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/requestletters")]
    [Authorize]
    public class RequestLettersController : ControllerBase
    {
        private readonly IRequestLetterService _requestLetterService;

        public RequestLettersController(IRequestLetterService requestLetterService)
        {
            _requestLetterService = requestLetterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(QueryObject queryObject)
        {
            var result = await _requestLetterService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _requestLetterService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestLetterDto requestLetterDto)
        {
            var result = await _requestLetterService.AddAsync(requestLetterDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RequestLetterDto requestLetterDto)
        {
            var result = await _requestLetterService.EditAsync(requestLetterDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _requestLetterService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("get-by-request")]
        public async Task<IActionResult> GetByRequestIdAsync(int id)
        {
            var result = await _requestLetterService.GetByRequestIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("replace-document-request-content/{requestId}/{letterId}")]
        public async Task<IActionResult> ReplaceDocumentRequestContent(int requestId, int letterId)
        {
            var result = await _requestLetterService.ReplaceDocumentRequestContent(requestId, letterId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("replace-case-close-request-content/{caseId}/{letterId}")]
        public async Task<IActionResult> ReplaceCaseCloseRequestContent(int caseId, int letterId)
        {
            var result = await _requestLetterService.ReplaceCaseCloseRequestContent(caseId, letterId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("replace-department")]
        public IActionResult ReplaceDeplartment([FromBody] ReplaceDepartmentDto data)
        {
            var result = _requestLetterService.ReplaceDeplartment(data.Contnet, data.DepartmentName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("print-requestletter")]
        public async Task<IActionResult> PrintLetter(int id)
        {
            var result = await _requestLetterService.GetAsync(id);
            var requestLetter = result.Data;

            IronPdf.License.LicenseKey = "IRONPDF.SMARTFINGERSFORIT.IRO210218.6534.32144.812012-D39F0D0BC6-MOWOPFLN4ZL6A-EFYQABQP3ZNE-YCW5VYEFHU5Z-SK3736X45P77-ZKKDZSRW6NVP-RDW4EC-LSVWNSUZYLWEEA-PRO.1DEV.1YR-6DVKWT.RENEW.SUPPORT.18.FEB.2022";
            bool isLicensed = IronPdf.License.IsLicensed;

            var Renderer = new HtmlToPdf();
            Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.MarginTop = 30;
            Renderer.PrintOptions.MarginBottom = 30;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.CustomCssUrl = "wwwroot/css/print-style.css";
            Renderer.PrintOptions.InputEncoding = System.Text.Encoding.UTF8;
            Renderer.PrintOptions.Title = requestLetter.RequestId.ToString();
            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {
                HtmlFragment = StringExtensions.SharedHeader()
            };

            var PDF = Renderer.RenderHtmlAsPdf(requestLetter.Text).BinaryData;

            return new FileContentResult(PDF, "application/pdf");
        }
    }
}