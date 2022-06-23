using IronPdf;
using IronXL;
using IronXL.Styles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/investigations")]
    [Authorize(Policy = "Investigators")]

    public class InvestigationsController : ControllerBase
    {
        private readonly IInvestigationService _investigationService;

        public InvestigationsController(IInvestigationService investigationService)
        {
            _investigationService = investigationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(InvestigationQueryObject queryObject)
        {
            var result = await _investigationService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _investigationService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InvestigationDto investigationDto)
        {
            var result = await _investigationService.AddAsync(investigationDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InvestigationDto InvestigationDto)
        {
            var result = await _investigationService.EditAsync(InvestigationDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _investigationService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("export-pdf")]
        public async Task<IActionResult> PrintCasesList(InvestigationQueryObject queryObject)
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
            Renderer.PrintOptions.Title = "قائمة التحقيقات";
            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {
                HtmlFragment = Common.Extensions.StringExtensions.SharedHeader()
            };
            Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            {
                RightText = "{date}",
                LeftText = "الصفحة رقم {page} من {total-pages}",
                DrawDividerLine = true,
                FontSize = 12,
                FontFamily = "Traditional Arabic"
            };

            // get all investigations
            ReturnResult<QueryResultDto<InvestigationListItemDto>> result = await _investigationService.GetAllAsync(queryObject);
            IEnumerable<InvestigationListItemDto> investigationsList = result.Data.Items;

            // Add data to the pdf
            string htmlContent =
                @"
                <table style=""direction: rtl!important"">
                  <thead>
                     <tr>
                      <th>#</th>
                      <th>رقم التحقيق</th>
                      <th>تاريخ ووقت بداية التحقيق</th>
                      <th>موضوع التحقيق</th>
                      <th>المحقق / رئيس مجموعة التحقيق</th>
                      <th>حالة التحقيق</th>
                      <th>له شق جنائي</th>
                    </tr>
                  </thead>
                  <tbody>";

            int i = 1;
            foreach (var investigation in investigationsList)
            {
                htmlContent += @"<tr>
                      <td>" + i + @"</td>
                      <td>" + investigation.InvestigationNumber + @"</td>
                      <td>" + investigation.StartDate + " - " + investigation.StartDateHigri + " - " + investigation.StartTime + @"</td>
                      <td>" + investigation.Subject + @"</td>
                      <td>" + investigation.InvestigatorFullName + @"</td>
                      <td>" + investigation.InvestigationStatus + @"</td>
                      <td>" + (investigation.IsHasCriminalSide ? "نعم" : "لا") + @"</td>
                    </tr>";
                i++;
            }
            htmlContent += @"</tbody>
                </table>";

            var PDF = Renderer.RenderHtmlAsPdf(htmlContent).BinaryData;

            return new FileContentResult(PDF, "application/pdf");
        }

        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportExcelCaseList(InvestigationQueryObject queryObject)
        {
            IronXL.License.LicenseKey = "IRONXL.SMARTFINGERSFORIT.IRO210218.7379.51122.812012-ACC9B4E2F4-EPJFWG6RI5IOE-Z5WAC6GYLGLA-DCCYQZBG7BJL-YGKMFP6PXLE3-7CTA56AEYFMM-JI6OML-LN5TEMPE5LSEEA-PRO.1DEV.1YR-AECULF.RENEW.SUPPORT.18.FEB.2022";
            bool isLicensed = IronXL.License.IsLicensed;

            WorkBook xlsxWorkbook = WorkBook.Create(ExcelFileFormat.XLSX);
            xlsxWorkbook.Metadata.Author = "برنامج إدارة الترافع";

            // get all investigations
            ReturnResult<QueryResultDto<InvestigationListItemDto>> result = await _investigationService.GetAllAsync(queryObject);
            IEnumerable<InvestigationListItemDto> investigationsList = result.Data.Items;

            WorkSheet xlsSheet = xlsxWorkbook.CreateWorkSheet("التحقيقات");

            // add title and curren date
            xlsSheet["E2"].Value = "قائمة التحقيقات";
            xlsSheet["C2"].Value = DateTime.Now.ToShortDateString();
            xlsSheet["C2:E2"].Style.BottomBorder.SetColor("#20C96F");
            xlsSheet["C2:E2"].Style.BottomBorder.Type = BorderType.Double;
            xlsSheet["C2:E2"].Style.Font.Bold = true;
            xlsSheet["C2:E2"].Style.VerticalAlignment = VerticalAlignment.Center;
            xlsSheet["C2:E2"].Style.HorizontalAlignment = HorizontalAlignment.Center;
            xlsSheet["C2:E2"].Style.WrapText = true;
            xlsSheet["A3:G3"].Style.FillPattern = FillPattern.Diamonds;

            // Add header of cells and styles
            xlsSheet["A4"].Value = "#";
            xlsSheet["B4"].Value = "رقم التحقيق";
            xlsSheet["C4"].Value = "تاريخ ووقت بداية التحقيق";
            xlsSheet["D4"].Value = "موضوع التحقيق";
            xlsSheet["E4"].Value = "المحقق / رئيس مجموعة التحقيق";
            xlsSheet["F4"].Value = "حالة التحقيق";
            xlsSheet["G4"].Value = "له شق جنائي";
            xlsSheet["A4:G4"].Style.BottomBorder.SetColor("#008000");
            xlsSheet["A4:G4"].Style.BottomBorder.Type = BorderType.Thick;
            xlsSheet["A4:G4"].Style.Font.Bold = true;
            xlsSheet["A4:G4"].Style.ShrinkToFit = false;

            //Add data to the worksheet
            int i = 5;
            foreach (var investigation in investigationsList)
            {
                xlsSheet["A" + i].Value = i - 4;
                xlsSheet["B" + i].Value = investigation.InvestigationNumber;
                xlsSheet["C" + i].Value = investigation.StartDate + " - " + investigation.StartDateHigri + " - " + investigation.StartTime;
                xlsSheet["D" + i].Value = investigation.Subject;
                xlsSheet["E" + i].Value = investigation.InvestigatorFullName;
                xlsSheet["F" + i].Value = investigation.InvestigationStatus;
                xlsSheet["G" + i].Value = investigation.IsHasCriminalSide ? "نعم" : "لا";
                i++;
            }
            xlsSheet[$"A{i}:G{i}"].Style.FillPattern = FillPattern.Diamonds;

            var EXCEL = xlsxWorkbook.ToBinary();
            return new FileContentResult(EXCEL, "application/ms-excel");
        }
    }
}
