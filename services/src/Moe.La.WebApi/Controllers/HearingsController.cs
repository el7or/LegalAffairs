using IronPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/hearings")]
    [Authorize]
    public class HearingsController : ControllerBase
    {
        private readonly IHearingService _hearingService;
        private readonly IHearingUpdateService _hearingUpdateService;


        public HearingsController(IHearingService hearingService, IHearingUpdateService hearingUpdateService)
        {
            _hearingService = hearingService;
            _hearingUpdateService = hearingUpdateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(HearingQueryObject queryObject)
        {
            var result = await _hearingService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _hearingService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HearingDto hearingDto)
        {
            var result = await _hearingService.AddAsync(hearingDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] HearingDto hearingDto)
        {
            var result = await _hearingService.EditAsync(hearingDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _hearingService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        //[HttpPost("close-and-create")]
        //public async Task<IActionResult> CloseAndCreate([FromBody] HearingCloseCreateDto hearing)
        //{
        //    var result = await _hearingService.CloseAndCreateAsync(hearing);
        //    return StatusCode((int)result.StatusCode, result);
        //}

        [HttpPut("receiving-judgment")]
        public async Task<IActionResult> ReceivingJudgment([FromBody] ReceivingJudgmentDto receivingJudgmentDto)
        {
            var result = await _hearingService.ReceivingJudgmentAsync(receivingJudgmentDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("hearing-status")]
        public IActionResult GetHearingStatus()
        {
            return Ok(EnumExtensions.GetValues<HearingStatuses>());
        }

        [HttpGet("hearing-type")]
        public IActionResult GetHearingTypes()
        {
            return Ok(EnumExtensions.GetValues<HearingTypes>());
        }

        [HttpGet("max-hearing-number/{caseId}")]
        public async Task<IActionResult> GetMaxHearingNumber(int caseId)
        {
            var result = await _hearingService.GetMaxHearingNumberAsync(caseId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("first-hearing-id/{caseId}")]
        public async Task<IActionResult> GetFirstHearingId(int caseId)
        {
            var result = await _hearingService.GetFirstHearingIdAsync(caseId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("hearing-number-exists")]
        public async Task<IActionResult> IsHearingNumberExists([FromBody] HearingNumberDto hearingDto)
        {
            var result = await _hearingService.IsHearingNumberExistsAsync(hearingDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("upcoming/{days}")]
        public async Task<IActionResult> GetUpcomingHearings(int days)
        {
            var result = await _hearingService.GetUpcomingHearingsAsync(days);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("upcoming-notifications-consultant/{days}")]
        public async Task<IActionResult> SendUpcomingHearingsNotificationsToConsultant(int days)
        {
            var result = await _hearingService.SendUpcomingHearingsNotificationsToConsultantAsync(days);
            return StatusCode((int)result.StatusCode, result);

        }

        [HttpGet("upcoming-notifications-administrator/{days}")]
        public async Task<IActionResult> SendUpcomingHearingsNotificationsToAdministrator(int days)
        {
            var result = await _hearingService.SendUpcomingHearingsNotificationsToAdministratorAsync(days);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("closing-notifications-consultant/{days}")]
        public async Task<IActionResult> SendClosingHearingsNotificationsToConsultant(int days)
        {
            var result = await _hearingService.SendClosingHearingsNotificationsToConsultantAsync(days);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("closing-notifications-administrator/{days}")]
        public async Task<IActionResult> SendClosingHearingsNotificationsToAdministrator(int days)
        {
            var result = await _hearingService.SendClosingHearingsNotificationsToAdminstratorAsync(days);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("make-pdf-report")]
        public async Task<IActionResult> MakePdfReportAsync(HearingQueryObject hearingQueryObjectDto)
        {
            License.LicenseKey = "IRONPDF.SMARTFINGERSFORIT.IRO210218.6534.32144.812012-D39F0D0BC6-MOWOPFLN4ZL6A-EFYQABQP3ZNE-YCW5VYEFHU5Z-SK3736X45P77-ZKKDZSRW6NVP-RDW4EC-LSVWNSUZYLWEEA-PRO.1DEV.1YR-6DVKWT.RENEW.SUPPORT.18.FEB.2022";

            var Renderer = new HtmlToPdf();
            Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.MarginTop = 30;
            Renderer.PrintOptions.MarginBottom = 30;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.CustomCssUrl = "wwwroot/css/print-style.css";
            Renderer.PrintOptions.InputEncoding = System.Text.Encoding.UTF8;
            Renderer.PrintOptions.Title = "قائمة الجلسات";
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

            // get all hearings            
            ReturnResult<QueryResultDto<HearingListItemDto>> result = await _hearingService.GetAllAsync(hearingQueryObjectDto);
            IEnumerable<HearingListItemDto> hearingList = result.Data.Items;

            // Add data to the pdf
            string htmlContent =
                @"
                <table>
                  <thead>
                     <tr>                      
                      <th>المكلف بالحضور</th>                                            
                      <th>اسم القضية</th>
                      <th>رقم القضية في النظام</th>
                      <th>رقم القضية في المصدر</th>
                      <th>المحكمة</th>
                      <th> حالة الجلسة</th>
                      <th>وقت الجلسة</th>
                      <th>تاريخ الجلسة</th>
                      <th>#</th>
                    </tr>
                  </thead>
                  <tbody>";

            int i = 1;
            foreach (var _hearing in hearingList)
            {
                htmlContent += @"<tr>                      
                      <td>" + (_hearing.AssignedTo == null ? "لم يتم التكليف بعد" : _hearing.AssignedTo.Name) + @"</td>
                      <td>" + _hearing.Case.Subject + @"</td>
                      <td>" + _hearing.Case.Id + @"</td>
                      <td>" + _hearing.Case.CaseNumberInSource + @"</td>
                      <td>" + _hearing.Court + @"</td>
                      <td>" + _hearing.Status + @"</td>
                      <td>" + _hearing.HearingTime + @"</td>
                      <td style='direction: rtl !important'><div>" + _hearing.HearingDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "</div><div>" + DateTimeHelper.GetHigriDate(_hearing.HearingDate) + @"</div></td>
                      <td>" + i + @"</td>
                    </tr>";
                i++;
            }
            htmlContent += @"</tbody>
                </table>";

            var PDF = Renderer.RenderHtmlAsPdf(htmlContent).BinaryData;

            return new FileContentResult(PDF, "application/pdf");
        }

        [HttpGet("assign-to/{hearingId}/{attendantId}")]
        [Authorize(Policy = "LitigationManagerOrBranchManager")]
        public async Task<IActionResult> AssignHearingTo(int hearingId, Guid attendantId)
        {
            var result = await _hearingService.AssignHearingToAsync(hearingId, attendantId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("hearing-updates")]
        public async Task<IActionResult> GetAllHearingUpdates(HearingUpdateQueryObject queryObject)
        {
            var result = await _hearingUpdateService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("hearing-updates/{id}")]
        public async Task<IActionResult> GetHearingUpdate(int id)
        {
            var result = await _hearingUpdateService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("add-hearing-update")]
        public async Task<IActionResult> CreateHearingUpdate([FromBody] HearingUpdateDto hearingUpdateDto)
        {
            var result = await _hearingUpdateService.AddAsync(hearingUpdateDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("edit-hearing-update")]
        public async Task<IActionResult> EditHearingUpdate([FromBody] HearingUpdateDto hearingUpdateDto)
        {
            var result = await _hearingUpdateService.EditAsync(hearingUpdateDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("consultants-and-researchers")]
        public async Task<IActionResult> GetConsultantsAndResearchers(UserQueryObject queryObject)
        {
            var result = await _hearingService.GetConsultantsAndResearchers(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
