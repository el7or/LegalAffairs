using IronPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Services;
using System.Globalization;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/moamalat")]
    //[Authorize(Roles = "Distributor,LegalConsultant,GeneralSupervisor")]

    public class MoamalatController : ControllerBase
    {
        private readonly IMoamalaService _moamalaService;
        private readonly IAuthorizationService _authorizationService;
        private AuthorizationResult hasConfidentialAccess;
        public MoamalatController(IMoamalaService moamalaService, IAuthorizationService authorizationService)
        {
            _moamalaService = moamalaService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(MoamalatQueryObject queryObject)
        {

            hasConfidentialAccess = await _authorizationService.AuthorizeAsync(HttpContext.User, "MoamlatConfedential");
            queryObject.HasConfidentialAccess = hasConfidentialAccess.Succeeded;
            var result = await _moamalaService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _moamalaService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = "Moamalat")]
        public async Task<IActionResult> Create([FromBody] MoamalaDto moamalaDto)
        {
            var result = await _moamalaService.AddAsync(moamalaDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        [Authorize(Policy = "Moamalat")]
        public async Task<IActionResult> Update([FromBody] MoamalaDto moamalaDto)
        {
            var result = await _moamalaService.EditAsync(moamalaDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _moamalaService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("confidential-degrees")]
        public IActionResult GetConfidentialDegrees()
        {
            return Ok(EnumExtensions.GetValues<ConfidentialDegrees>());
        }

        [HttpGet("pass-types")]
        public IActionResult GetPassTypes()
        {
            return Ok(EnumExtensions.GetValues<PassTypes>());
        }

        [HttpPut("change-status")]
        public async Task<IActionResult> ChangeStatusAsync([FromBody] MoamalaChangeStatusDto changeStatusDto)
        {
            var result = await _moamalaService.ChangeStatusAsync(changeStatusDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("return/{id}")]
        public async Task<IActionResult> ReturnAsync(int id, string note)
        {
            var result = await _moamalaService.ReturnAsync(id, note);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("update-work-item-type")]
        public async Task<IActionResult> UpdateWorkItemType([FromBody] MoamalaUpdateWorkItemType moamalaUpdateWorkItemType)
        {
            var result = await _moamalaService.UpdateWorkItemTypeAsync(moamalaUpdateWorkItemType);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("update-related-id")]
        public async Task<IActionResult> UpdateRelatedId([FromBody] MoamalaUpdateRelatedId moamalaUpdateRelatedId)
        {
            var result = await _moamalaService.UpdateRelatedIdAsync(moamalaUpdateRelatedId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("print-moamala-details")]
        public IActionResult PrintMoamala([FromBody] MoamalaDetailsDto moamalaDetails)
        {
            License.LicenseKey = "IRONPDF.SMARTFINGERSFORIT.IRO210218.6534.32144.812012-D39F0D0BC6-MOWOPFLN4ZL6A-EFYQABQP3ZNE-YCW5VYEFHU5Z-SK3736X45P77-ZKKDZSRW6NVP-RDW4EC-LSVWNSUZYLWEEA-PRO.1DEV.1YR-6DVKWT.RENEW.SUPPORT.18.FEB.2022";

            var Renderer = new HtmlToPdf();
            Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.MarginTop = 30;
            Renderer.PrintOptions.MarginBottom = 30;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.CustomCssUrl = "wwwroot/css/print-style.css";
            Renderer.PrintOptions.InputEncoding = System.Text.Encoding.UTF8;
            Renderer.PrintOptions.Title = moamalaDetails.ConfidentialDegree?.Name;
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

            string htmlContent =
                @"<div class='page-header'><h2>تفاصيل المعاملة  رقم " + moamalaDetails.Id + @"</h2></div>
                   <br>
                    <table class=""table table-striped"" >
                        <tbody>   
                            <tr>
                                <td><b>تاريخ انشاء المعاملة: </b>" + moamalaDetails.CreatedOn.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "  " + DateTimeHelper.GetHigriDate(moamalaDetails.CreatedOn) + @"</td>
                                <td><b>الرقم الموحد: </b> " + moamalaDetails.UnifiedNo + @"</td>
                            </tr>
                            <tr>
                                <td><b>وقت انشاء المعاملة: </b>" + moamalaDetails.CreatedOnTime + @"</td>
                                <td><b>رقم المعاملة: </b> " + moamalaDetails.MoamalaNumber + @"</td>
                            </tr>
                            <tr>
                                <td><b>تاريخ ورود المعاملة: </b>" + moamalaDetails.PassDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "  " + DateTimeHelper.GetHigriDate(moamalaDetails.PassDate) + @"</td>
                                <td><b>عنوان المعاملة: </b>" + moamalaDetails.Subject + @"</td>
                            </tr>
                            <tr>
                                <td><b>وقت ورود المعاملة: </b>" + moamalaDetails.PassTime + @"</td>
                                <td><b>درجة السرية: </b>" + moamalaDetails.ConfidentialDegree?.Name + @"</td>
                            </tr>
                            <tr>
                                <td><b>واردة من: </b>" + moamalaDetails.SenderDepartment?.Name + @"</td>
                                <td><b>حالة المعاملة: </b>" + moamalaDetails.Status?.Name + @"</td>
                            </tr>";
            if (moamalaDetails.WorkItemType != null)
                htmlContent += @"<tr>
                                <td><b>النوع الفرعي للمعاملة: </b>" + moamalaDetails.SubWorkItemType?.Name + @"</td>
                                <td><b>نوع المعاملة: </b>" + moamalaDetails.WorkItemType?.Name + @"</td>
                            </tr>";

            htmlContent += @"<tr>
                                <td colspan=""2""><b>تفاصيل المعاملة: </b>" + moamalaDetails.Description + @"</td>
                            </tr>
                      </tbody>
                    </table>";

            htmlContent += @"
                <h2 style=""float: right; font-family:'Traditional Arabic'"">المعاملات المرتبطة</h2>
                <table>
                 <thead>
                   <tr>                      
                      <th>واردة من</th>
                      <th>حالة المعاملة</th>
                      <th>درجة السرية</th>
                      <th>عنوان المعاملة</th>                      
                      <th>تاريخ ووقت ورود المعاملة</th>
                      <th>تاريخ ووقت إنشاء المعاملة</th>
                      <th>رقم المعاملة</th>
                      <th>الرقم الموحد</th>
                      <th>#</th>
                     </tr>
                 </thead>
               <tbody> ";

            int i = 1;
            foreach (var moamala in moamalaDetails.RelatedMoamalat)
            {
                htmlContent += @"<tr>
                      <td>" + moamala.SenderDepartment?.Name + @"</td>
                      <td>" + moamala.Status?.Name + @"</td>
                      <td>" + moamala.ConfidentialDegree?.Name + @"</td>
                      <td>" + moamala.Subject + @"</td>
                      <td>" + moamala.PassDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "  " + DateTimeHelper.GetHigriDate(moamala.PassDate) + "  " + moamala.PassTime + @"</td>
                      <td>" + moamala.CreatedOn.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "  " + DateTimeHelper.GetHigriDate(moamala.CreatedOn) + "  " + moamala.CreatedOnTime + @"</td>
                      <td>" + moamala.MoamalaNumber + @"</td>
                      <td>" + moamala.UnifiedNo + @"</td>
                      <td>" + i + @"</td>
                    </tr>";
                i++;
            }

            htmlContent += @"</tbody></table>";

            var PDF = Renderer.RenderHtmlAsPdf(htmlContent).BinaryData;

            return new FileContentResult(PDF, "application/pdf");
        }
    }
}
