import { Injectable } from '@angular/core';
import { Workbook } from 'exceljs';
import * as Excel from "exceljs/dist/exceljs.min.js"; //npm install exceljs
import * as ExcelProper from "exceljs";
import * as FileSaver from 'file-saver'; //npm install file-saver
import { NONE_TYPE } from '@angular/compiler/src/output/output_ast';

@Injectable()

export class ExportExcelService {

  blobType: string = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
  constructor() {
  }

  generateExcel(mainTitle: string, subTitle: string, cols: any[], objArray) {

    const Alphabet:string[]=['A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z']
    const title = mainTitle;
    const data = this.convertObjectsToArray(objArray)

    //Create workbook and worksheet
    let workbook = new Excel.Workbook();
    workbook.creator = "";
    workbook.lastModifiedBy ="";
    workbook.created = "";
    workbook.modified = "";

    workbook.rtl = true;

    let worksheet = workbook.addWorksheet(mainTitle);

    worksheet.views = [
      { rightToLeft: true, state: 'frozen', ySplit: 7 }
    ];
    //worksheet.views = [
    //  { rightToLeft: true }
    //];

    //Blank Row
    let blank = worksheet.addRow([]);
    blank.font = { name: 'Tahoma', family: 4, size: 8, bold: false }
    worksheet.mergeCells('A1:' + Alphabet[cols.length - 1] + '1');

    //Add Row and formatting
    let titleRow = worksheet.addRow([title]);
    titleRow.font = { name: 'Traditional Arabic', family: 4, size: 18, underline: 'double', bold:true }
    titleRow.alignment = { horizontal: 'center' };
    worksheet.mergeCells('A2:' + Alphabet[cols.length - 1] + '3');

    //Blank Row
    worksheet.addRow([]);
    worksheet.mergeCells('A4:' + Alphabet[cols.length - 1] + '4');

    let subTitleRow = worksheet.addRow([subTitle]);
    subTitleRow.font = { name: 'Traditional Arabic', size: 12, bold: true }
    subTitleRow.alignment = { horizontal: 'right' };
    worksheet.mergeCells('A5:' + Alphabet[cols.length - 1] + '5');

    //Add Image
    //let logo = workbook.addImage({
    //  base64: logoFile.logoBase64,
    //  extension: 'png',
    //});
    //worksheet.addImage(logo, 'E1:F3');

    //Blank Row
    worksheet.addRow([]);
    worksheet.mergeCells('A6:' + Alphabet[cols.length - 1] + '6');

    //Add Header Row
    let headerRow = worksheet.addRow(cols.map(item => { return item.title })); // add the header row
    headerRow.font = { name: 'Tahoma', size: 10, underline: 'none', bold: true }

    //Add Data and Conditional Formatting
    cols.forEach(function (item, index) { // loop throw the columns

      if (item.width)
        worksheet.getColumn(index + 1).width = item.width; //  set column width

      let cell = headerRow.getCell(index + 1); // get the cell

      //////
      if (!item.align) {
        if ((item.minVal) || (item.boolean))
          cell.alignment = { horizontal: 'center' };
      }
      else {
        cell.alignment = { horizontal: item.align };
      }
      /////

      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFFFFF00' },
        bgColor: { argb: 'FF0000FF' }
      }
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }

    });

    data.forEach(dataRow => {

      let row = worksheet.addRow(dataRow); // add the row with all the values
      row.font = { name: 'Tahoma', size: 10, underline: 'none', bold: false }

      cols.forEach(function (item, index) { // loop throw the columns and get the properties

        let cell = row.getCell(index + 1); // get the cell

        if ((item.minVal) || (item.boolean)) {

          ////
          if (!item.align)
          cell.alignment = { horizontal: 'center' };
          ///
          let color = 'FF99FF99';

          if (item.boolean) { // boolean

            if ((+cell.value === 0) || (+cell.value === 1)) { // true false
              if (+cell.value === 0) {
                color = 'FF9999';
                cell.value = 'لا';
              }
              else {
                cell.value = 'نعم';
              }
            }
            else if (item.boolean !== cell.value) { // item.boolean is a value like name
              color = 'FF9999';
            }

            cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          }// end boolean
          else if (item.minVal) { // minVal
            if (+cell.value <= item.minVal)
              color = 'FF9999';
          } // end minVal

          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: color }
          }
        } // end if
        else {
          if (item.align)
          cell.alignment = { horizontal: item.align };
        }
      });
    }); // end forEach

    worksheet.addRow([]);
    //Footer Row
    let footerRow = worksheet.addRow(['تم اصدار هذا الملف من خلال النظام.']);
    footerRow.font = { name: 'Tahoma', size: 10, underline: 'none', bold: true }

    footerRow.getCell(1).fill = {
      type: 'pattern',
      pattern: 'solid',
      fgColor: { argb: 'FFCCFFE5' }
    };
    footerRow.getCell(1).border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
    //Merge Cells
    worksheet.mergeCells(`A${footerRow.number}:` + Alphabet[cols.length-1] + `${footerRow.number}`);
    //Generate Excel File with given name

    workbook.xlsx.writeBuffer().then(data => {
      const blob = new Blob([data], { type: this.blobType });
      FileSaver.saveAs(blob, mainTitle+'.xlsx');
    });

  }


  convertObjectsToArray(objArray) {

    var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;

    let mainArray: any[] = [];

    for (var i = 0; i < array.length; i++) {
      var subArray: any[] = [];
      for (var j in array[i])
        subArray.push(array[i][j]);
      mainArray.push(subArray);
    }
    return mainArray;
  }
}



