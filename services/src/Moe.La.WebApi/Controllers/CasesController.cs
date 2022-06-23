using IronPdf;
using IronXL;
using IronXL.Styles;
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
    [Route("api/cases")]
    [AllowAnonymous]
    public class CasesController : ControllerBase
    {
        private readonly ICaseService _caseService;

        public CasesController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CaseQueryObject queryObject)
        {
            var result = await _caseService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("full-data")]
        public IActionResult GetAll()
        {
            var result = _caseService.GetAllAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        //[Authorize(Policy = "LitigationManagerOrRegionsSupervisor")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _caseService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        //[Authorize(Policy = "LitigationManager")]
        public async Task<IActionResult> Create([FromBody] BasicCaseDto caseDto)
        {
            var result = await _caseService.AddAsync(caseDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("next")]
        //[Authorize(Policy = "LitigationManager")]
        public async Task<IActionResult> CreateNextCase([FromBody] NextCaseDto caseDto)
        {
            var result = await _caseService.CreateNextCase(caseDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("parties/{id}")]
        public async Task<IActionResult> GetCaseParties(int Id)
        {
            var result = await _caseService.GetCasePartiesAsync(Id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("add-case-party")]
        public async Task<IActionResult> AddCasePartyAsync([FromBody] CasePartyDto caseParty)
        {
            var result = await _caseService.AddCasePartyAsync(caseParty);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("update-case-party")]
        public async Task<IActionResult> UpdateCasePartyAsync([FromBody] CasePartyDto caseParty)
        {
            var result = await _caseService.UpdateCasePartyAsync(caseParty);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("delete-case-party/{id}")]
        public async Task<IActionResult> DeleteCasePartyAsync(int id)
        {
            var result = await _caseService.DeleteCasePartyAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }



        [HttpPut("attachments")]
        public async Task<IActionResult> UpdateAttachmentsAsync([FromBody] CaseAttachmentsListDto data)
        {
            var result = await _caseService.UpdateAttachmentsAsync(data);
            return StatusCode((int)result.StatusCode, result);
        }

        #region case-grounds

        [HttpGet("grounds/{id}")]
        public async Task<IActionResult> GetAllGroundsAsync(int id)
        {
            var result = await _caseService.GetAllGroundsAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("ground")]
        public async Task<IActionResult> CreateGround([FromBody] CaseGroundsDto caseGroundsDto)
        {
            var result = await _caseService.AddGroundAsync(caseGroundsDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("ground")]
        public async Task<IActionResult> EditGround([FromBody] CaseGroundsDto caseGroundsDto)
        {
            var result = await _caseService.EditGroundAsync(caseGroundsDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("ground/{id}")]
        public async Task<IActionResult> DeleteGround(int id)
        {
            var result = await _caseService.RemoveGroundAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("grounds")]
        public async Task<IActionResult> UpdateAllGroundsAsync([FromBody] CaseGroundsListDto caseGrounds)
        {
            var result = await _caseService.UpdateAllGroundsAsync(caseGrounds);
            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region Case Moamalat

        [HttpGet("moamalat/{id}")]
        public async Task<IActionResult> GetCaseMoamalat(int id)
        {
            var result = await _caseService.GetCaseMoamalatAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("moamalat")]
        public async Task<IActionResult> CreateCaseMoamalat([FromBody] CaseMoamalatDto caseMoamalatDto)
        {
            var result = await _caseService.AddCaseMoamalatAsync(caseMoamalatDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{caseId}/moamalat/{moamalaId}")]
        public async Task<IActionResult> DeleteCaseMoamalat(int caseId, int moamalaId)
        {
            var result = await _caseService.RemoveCaseMoamalatAsync(caseId, moamalaId);
            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BasicCaseDto caseDto)
        {
            var result = await _caseService.EditAsync(caseDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _caseService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("case-sources")]
        public IActionResult GetCaseSources()
        {
            return Ok(EnumExtensions.GetValues<CaseSources>());
        }

        [HttpGet("case-status")]
        public IActionResult GetCaseStatus()
        {
            return Ok(EnumExtensions.GetValues<CaseStatuses>());
        }

        [HttpGet("litigation-types")]
        public IActionResult GetLitigationTypes()
        {
            return Ok(EnumExtensions.GetValues<LitigationTypes>());
        }

        [HttpGet("parties-status")]
        public IActionResult GetPartiesStatus()
        {
            return Ok(EnumExtensions.GetValues<PartyStatus>());
        }

        [HttpGet("ministry-legal-status")]
        public IActionResult GetMinistryLegalStatus()
        {
            return Ok(EnumExtensions.GetValues<MinistryLegalStatuses>());
        }

        [HttpGet("judgement-results")]
        public IActionResult GetJudgementResults()
        {
            return Ok(EnumExtensions.GetValues<JudgementResults>());
        }

        [HttpGet("party-types")]
        public IActionResult GetPartyTypes()
        {
            return Ok(EnumExtensions.GetValues<PartyTypes>());
        }


        [HttpPut("change-status")]
        public async Task<IActionResult> ChangeStatus([FromBody] CaseChangeStatusDto caseChangeStatusDto)
        {
            var result = await _caseService.ChangeStatusAsync(caseChangeStatusDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("send-to-general-manager")]
        public async Task<IActionResult> SendToBranchManager([FromBody] CaseSendToBranchManagerDto caseSendToBranchManagerDto)
        {
            var result = await _caseService.SendToBranchManagerAsync(caseSendToBranchManagerDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("assign-researcher")]
        public async Task<IActionResult> AssignResearcher([FromBody] CaseResearchersDto caseResearchersDto)
        {
            var result = await _caseService.AssignResearcherAsync(caseResearchersDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("parent-case-id/{id}")]
        public async Task<IActionResult> GetParentCase(int id)
        {
            var result = await _caseService.GetParentCaseAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("add-rule")]
        public async Task<IActionResult> CreateRule([FromBody] CaseRuleDto ruleDto)
        {
            var result = await _caseService.AddRuleAsync(ruleDto);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost("receive-judgment-instrument")]
        public async Task<IActionResult> ReceiveJudgmentInstrument([FromBody] ReceiveJdmentInstrumentDto receiveJdmentInstrumentDto)
        {
            var result = await _caseService.ReceiveJudmentInstrumentAsync(receiveJdmentInstrumentDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("receive-judgment-instrument")]
        public async Task<IActionResult> GetReceiveJudgmentInstrument(int Id)
        {
            var result = await _caseService.GetReceiveJudmentInstrumentAsync(Id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("receive-judgment-instrument")]
        public async Task<IActionResult> EditReceiveJudgmentInstrument([FromBody] ReceiveJdmentInstrumentDto receiveJdmentInstrumentDto)
        {
            var result = await _caseService.EditReceiveJudmentInstrumentAsync(receiveJdmentInstrumentDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("Create-Case")]
        [Authorize(Policy = "LitigationManager")]
        public async Task<IActionResult> CreateCase([FromBody] InitialCaseDto initialCase)
        {
            var result = await _caseService.CreateCaseAsync(initialCase);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("Create-objection-Case")]
        public async Task<IActionResult> CreateObjectionCase([FromBody] ObjectionCaseDto model)
        {
            var result = await _caseService.CreateObjectionCaseAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("print-case-details")]
        public IActionResult PrintCase([FromBody] CaseDetailsDto caseDetails)
        {
            IronPdf.License.LicenseKey = "IRONPDF.SMARTFINGERSFORIT.IRO210218.6534.32144.812012-D39F0D0BC6-MOWOPFLN4ZL6A-EFYQABQP3ZNE-YCW5VYEFHU5Z-SK3736X45P77-ZKKDZSRW6NVP-RDW4EC-LSVWNSUZYLWEEA-PRO.1DEV.1YR-6DVKWT.RENEW.SUPPORT.18.FEB.2022";
            bool isLicensed = IronPdf.License.IsLicensed;

            var Renderer = new HtmlToPdf();
            Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.MarginTop = 30;
            Renderer.PrintOptions.MarginBottom = 30;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.CustomCssUrl = "wwwroot/css/print-style.css";
            Renderer.PrintOptions.InputEncoding = System.Text.Encoding.UTF8;
            Renderer.PrintOptions.Title = caseDetails.CaseSource.Name;
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
                @"  <div class='page-header'><h2>تفاصيل القضية  رقم " + caseDetails.Id + @"</h2></div>
                    <br>
                    <table class=""table table-striped"" >
                        <tbody>   
                            <tr>
                                <td><b>المصدر: </b> " + caseDetails.CaseSource.Name + @"</td>
                                <td><b>صفة الوزارة القانونية: </b>" + caseDetails.LegalStatus?.Name + @"</td>
                            </tr>
                            <tr>
                                <td><b>رقم المعاملة: </b>" + (caseDetails.CaseSource.Id == (int)CaseSources.Moeen ? caseDetails.MoeenRef : caseDetails.CaseSource.Id == (int)CaseSources.Najiz ? caseDetails.NajizRef : caseDetails.RaselRef) + @"</td>
                                <td><b>المحكمة: </b>" + caseDetails.Court?.Name + @"</td>
                            </tr>
                            <tr>
                                <td><b>حالة القضية: </b>" + caseDetails.Status.Name + @"</td>
                                <td><b>الدائرة: </b>" + caseDetails.CircleNumber + @"</td>
                            </tr>
                            <tr>
                                <td><b>";

            htmlContent += "رقم القضية فى المحكمة:";
            htmlContent += caseDetails.CaseNumberInSource + @" </b></td>
                                <td><b> درجة الترافع: </b>" + caseDetails.LitigationType?.Name + @"</td>
                            </tr>
                            <tr>
                                <td><b>تاريخ بداية القضية: </b>" + (caseDetails.StartDate.HasValue ? caseDetails.StartDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "  " + DateTimeHelper.GetHigriDate(caseDetails.StartDate.Value) : "") + @"</td>
                                <td><b>الباحث المكلف: </b>";
            foreach (var researcher in caseDetails.Researchers)
                htmlContent += researcher.Name;
            htmlContent += @"</td>
                            </tr>
                            <tr>
                                <td colspan=""2""><b>عنوان الدعوى: </b>" + caseDetails.Subject + @"</td>
                            </tr>
                        </tbody>
                    </table>";

            htmlContent += @"
                <h2 style=""float: right; font-family:'Traditional Arabic'"">الأطراف</h2>
                <table>
                 <thead>
                    <tr>   
                    <th>نوع الطرف</th>
                    <th>اسم الطرف</th>
                    <th style=""width:80px;"">#</th>
                    </tr>
                 </thead>
               <tbody> ";
            int i = 1;
            foreach (var caseParty in caseDetails.CaseParties)
            {
                htmlContent += @"<tr>
                    <td>" + caseParty.Party.PartyTypeName + "</td> " +
                    "<td>" + caseParty.Party.Name + "</td>" +
                    "<td>" + i + "</td></tr>";
                i++;
            }

            htmlContent += @"</tbody></table>";

            htmlContent += @"
                <h2  style=""float: right; font-family:'Traditional Arabic'"">الجلسات</h2>
                <table >
                  <thead >
                    <tr>         
                    <th>نوع الجلسة</th>             
                    <th>وقت الجلسة</th>
                    <th>تاريخ الجلسة</th>
                    <th style=""width:80px;"">#</th>
                    </tr>
                  </thead>
                  <tbody>";
            i = 1;
            foreach (var hearing in caseDetails.Hearings)
            {
                htmlContent += @"<tr>
                      <td>" + hearing.Type.Name + @"</td>
                      <td>" + hearing.HearingTime + @"</td>
                      <td><div>" + hearing.HearingDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "</div><div>" + DateTimeHelper.GetHigriDate(hearing.HearingDate) + @"</div></td>
                      <td>" + i + @"</td>
                    </tr>";
                i++;
            }
            htmlContent += @"</tbody>
                </table>
                </div>";

            var PDF = Renderer.RenderHtmlAsPdf(htmlContent).BinaryData;

            return new FileContentResult(PDF, "application/pdf");
        }

        [HttpPost("print-case-list")]
        public async Task<IActionResult> PrintCasesList([FromBody] CaseQueryObject queryObject)
        {
            IronPdf.License.LicenseKey = "IRONPDF.SMARTFINGERSFORIT.IRO210218.6534.32144.812012-D39F0D0BC6-MOWOPFLN4ZL6A-EFYQABQP3ZNE-YCW5VYEFHU5Z-SK3736X45P77-ZKKDZSRW6NVP-RDW4EC-LSVWNSUZYLWEEA-PRO.1DEV.1YR-6DVKWT.RENEW.SUPPORT.18.FEB.2022";
            bool isLicensed = IronPdf.License.IsLicensed;

            var Renderer = new HtmlToPdf();
            Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.MarginTop = 30;
            Renderer.PrintOptions.MarginBottom = 30;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.CustomCssUrl = "wwwroot/css/print-style.css";
            Renderer.PrintOptions.InputEncoding = System.Text.Encoding.UTF8;
            Renderer.PrintOptions.Title = "قائمة القضايا";
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

            //// get all cases
            //queryObject.Page = 1;
            //queryObject.PageSize = 9999;

            ReturnResult<QueryResultDto<CaseListItemDto>> result = await _caseService.GetAllAsync(queryObject);
            IEnumerable<CaseListItemDto> casesList = result.Data.Items;

            // Add data to the pdf
            string htmlContent =
                @"
                   <div class='page-header'><h2>قائمة القضايا </h2></div>
                <table style=""direction: rtl!important"">
                  <thead>
                     <tr>
                      <th>#</th>
                      <th>رقم القضية في المحكمة</th>
                      <th>تاريخ بداية القضية</th>
                      <th>مصدر القضية</th>
                      <th>تاريخ الإنشاء</th>
                      <th>المحكمة</th>
                      <th>الدائرة</th>
                      <th>درجة الترافع</th>
                      <th>صفة الوزارة</th>
                      <th>الحالة</th>
                    </tr>
                  </thead>
                  <tbody>";

            int i = 1;
            foreach (var _case in casesList)
            {
                htmlContent += @"<tr>
                      <td>" + i + @"</td>
                      <td>" + _case.CaseNumberInSource + @"</td>
                      <td><div>" + (_case.StartDate.HasValue ? _case.StartDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "") + "</div><div>" + (_case.StartDate.HasValue ? DateTimeHelper.GetHigriDate(_case.StartDate.Value) : "") + @"</div></td>
                      <td>" + _case.CaseSource.Name + @"</td>
                      <td><div>" + _case.CreatedOn.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "</div><div>" + _case.CreatedOnHigri + @"</div></td>
                      <td>" + _case.Court + @"</td>
                      <td>" + _case.CircleNumber + @"</td>
                      <td>" + _case.LitigationType.Name + @"</td>
                      <td>" + _case.LegalStatus + @"</td>
                      <td>" + _case.Status.Name + @"</td>
                    </tr>";
                i++;
            }
            htmlContent += @"</tbody>
                </table>";

            var PDF = Renderer.RenderHtmlAsPdf(htmlContent).BinaryData;

            return new FileContentResult(PDF, "application/pdf");
        }

        [HttpPost("excel-case-list")]
        public async Task<IActionResult> ExportExcelCaseList([FromBody] CaseQueryObject queryObject)
        {

            IronXL.License.LicenseKey = "IRONXL.SMARTFINGERSFORIT.IRO210218.7379.51122.812012-ACC9B4E2F4-EPJFWG6RI5IOE-Z5WAC6GYLGLA-DCCYQZBG7BJL-YGKMFP6PXLE3-7CTA56AEYFMM-JI6OML-LN5TEMPE5LSEEA-PRO.1DEV.1YR-AECULF.RENEW.SUPPORT.18.FEB.2022";
            bool isLicensed = IronXL.License.IsLicensed;

            WorkBook xlsxWorkbook = WorkBook.Create(ExcelFileFormat.XLSX);
            xlsxWorkbook.Metadata.Author = "برنامج إدارة الترافع";

            //// get all cases
            //queryObject.Page = 1;
            //queryObject.PageSize = 9999;

            ReturnResult<QueryResultDto<CaseListItemDto>> result = await _caseService.GetAllAsync(queryObject);
            IEnumerable<CaseListItemDto> casesList = result.Data.Items;

            WorkSheet xlsSheet = xlsxWorkbook.CreateWorkSheet("قائمة القضايا الواردة للترافع");

            // add title and curren date
            xlsSheet["J2"].Value = "القضايا الواردة للترافع";
            xlsSheet["H2"].Value = DateTime.Now.ToShortDateString();
            xlsSheet["H2:J2"].Style.BottomBorder.SetColor("#20C96F");
            xlsSheet["H2:J2"].Style.BottomBorder.Type = BorderType.Double;
            xlsSheet["H2:J2"].Style.Font.Bold = true;
            xlsSheet["H2:J2"].Style.VerticalAlignment = VerticalAlignment.Center;
            xlsSheet["H2:J2"].Style.HorizontalAlignment = HorizontalAlignment.Center;
            xlsSheet["H2:J2"].Style.WrapText = true;
            xlsSheet["A3:Y3"].Style.FillPattern = FillPattern.Diamonds;

            // Add header of cells and styles
            xlsSheet["A4"].Value = "#";
            xlsSheet["B4"].Value = "رقم القضية في المحكمة";
            xlsSheet["C4"].Value = "تاريخ الإنشاء";
            xlsSheet["D4"].Value = "المصدر";
            xlsSheet["E4"].Value = "المحكمة";
            xlsSheet["F4"].Value = "عدد الجلسات";
            xlsSheet["G4"].Value = "الدائرة";
            xlsSheet["H4"].Value = "الحالة";
            xlsSheet["I4"].Value = "رقم المعاملة في ناجز";
            xlsSheet["J4"].Value = "رقم القضية في ناجز";
            xlsSheet["K4"].Value = "رقم الطلب في ناجز";
            xlsSheet["L4"].Value = "رقم المعاملة في معين";
            xlsSheet["M4"].Value = "رقم الدعوى في معين";
            xlsSheet["N4"].Value = "رقم القيد في راسل";
            xlsSheet["O4"].Value = "الرقم الموحد في راسل";
            xlsSheet["P4"].Value = "رقم القضية في اللجنة";
            xlsSheet["Q4"].Value = "تاريخ بداية القضية";
            xlsSheet["R4"].Value = "الإدارة المحال إليها";
            xlsSheet["S4"].Value = "صفة الوزارة القانونية";
            xlsSheet["T4"].Value = "درجة الترافع";
            xlsSheet["U4"].Value = "تاريخ النطق بالحكم";
            xlsSheet["V4"].Value = "موعد استلام الحكم";
            xlsSheet["W4"].Value = "طريقة الإدخال";
            xlsSheet["X4"].Value = "عنوان الدعوى";
            xlsSheet["Y4"].Value = "موضوع الدعوى";
            xlsSheet["A4:Y4"].Style.BottomBorder.SetColor("#008000");
            xlsSheet["A4:Y4"].Style.BottomBorder.Type = BorderType.Thick;
            xlsSheet["A4:Y4"].Style.Font.Bold = true;
            xlsSheet["A4:Y4"].Style.WrapText = true;

            //Add data to the worksheet
            int i = 5;
            foreach (var _case in casesList)
            {
                xlsSheet["A" + i].Value = i - 4;
                xlsSheet["B" + i].Value = _case.Id;
                xlsSheet["C" + i].Value = _case.CreatedOn.ToShortDateString();
                xlsSheet["D" + i].Value = _case.CaseSource.Name;
                xlsSheet["E" + i].Value = _case.Court;
                xlsSheet["F" + i].Value = _case.HearingsCount;
                xlsSheet["G" + i].Value = _case.CircleNumber;
                xlsSheet["H" + i].Value = _case.Status.Name;
                xlsSheet["I" + i].Value = _case.NajizRef;
                xlsSheet["J" + i].Value = _case.CaseSource.Id == (int)CaseSources.Najiz ? _case.CaseNumberInSource : "";
                xlsSheet["K" + i].Value = _case.NajizId;
                xlsSheet["L" + i].Value = _case.MoeenRef;
                xlsSheet["M" + i].Value = _case.CaseSource.Id == (int)CaseSources.Moeen ? _case.CaseNumberInSource : "";
                xlsSheet["N" + i].Value = _case.RaselRef;
                xlsSheet["O" + i].Value = _case.RaselUnifiedNo;
                xlsSheet["P" + i].Value = _case.CaseSource.Id == (int)CaseSources.QuasiJudicialCommittee ? _case.CaseNumberInSource : "";
                xlsSheet["Q" + i].Value = _case.StartDate.HasValue ? _case.StartDate.Value.ToShortDateString() : "";
                xlsSheet["R" + i].Value = _case.Branch;
                xlsSheet["S" + i].Value = _case.LegalStatus;
                xlsSheet["T" + i].Value = _case.LitigationType;
                xlsSheet["U" + i].Value = _case.PronouncingJudgmentDate.HasValue ? _case.PronouncingJudgmentDate.Value.ToShortDateString() : "";
                xlsSheet["V" + i].Value = _case.ReceivingJudgmentDate.HasValue ? _case.ReceivingJudgmentDate.Value.ToShortDateString() : "";
                xlsSheet["W" + i].Value = _case.IsManual == true ? "يدويا" : "تكامل";
                xlsSheet["X" + i].Value = _case.Subject;
                xlsSheet["Y" + i].Value = _case.CaseDescription;
                i++;
            }
            xlsSheet[$"A{i}:Y{i}"].Style.FillPattern = FillPattern.Diamonds;

            var EXCEL = xlsxWorkbook.ToBinary();
            return new FileContentResult(EXCEL, "application/ms-excel");
        }
    }
}
