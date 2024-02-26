using System;
using System.Diagnostics.Tracing;
using ClosedXML.Excel;
using Microsoft.Extensions.Options;
using OutlookAdReport.Analyzation.Models;
using OutlookAdReport.Export.ExcelExport.Options;

namespace OutlookAdReport.Export.ExcelExport;

/// <summary>   An excel file export. </summary>
public class ExcelFileExport : FileExport
{
    /// <summary>   Constructor. </summary>
    /// <param name="options">  Options for controlling the operation. </param>
    public ExcelFileExport(IOptions<ExportOptions> options) : base(options)
    {
    }

    /// <summary>   Gets default folder. </summary>
    /// <returns>   The default folder. </returns>
    protected override string GetDefaultFolder() => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    /// <summary>   Gets default file name. </summary>
    /// <returns>   The default file name. </returns>
    protected override string GetDefaultFileName() => $"Zeiten {Environment.UserName}.xlsx";

    /// <summary>   Exports. </summary>
    /// <param name="events">   The events. </param>
    /// <param name="fileName"> Filename of the file. </param>
    public override void Export(IEnumerable<IBusinessEvent> events, string fileName)
    {
        if (File.Exists(fileName))
            File.Delete(fileName);
        File.Copy("BaseSheet.xlsx", fileName);

        var workbook = new XLWorkbook(fileName);
        ExportWorkReportData(workbook.Worksheet("Arbeitsbericht"), events);
        
        workbook.Save();
        //throw new NotImplementedException();
    }

    /// <summary>   Export work report data. </summary>
    /// <param name="sheet">    The sheet. </param>
    /// <param name="events">   The events. </param>
    protected virtual void ExportWorkReportData(IXLWorksheet? sheet, IEnumerable<IBusinessEvent> events)
    {
        if (sheet == null) return;

        var ev = events.ToList();

        sheet.Row(1).InsertRowsBelow(ev.Count);
        var rng = sheet.Range(sheet.Cell(1, 1), sheet.Cell(ev.Count + 1, 14));
        rng.Clear(XLClearOptions.AllFormats);

        var rowNum = 2;

        foreach (var entry in ev)
        {
            sheet.Cell(rowNum, 1).Value = entry.Start;
            sheet.Cell(rowNum, 1).Style.DateFormat.Format = "d.M";

            sheet.Cell(rowNum, 2).Value = entry.Customer;

            sheet.Cell(rowNum, 3).Value = entry.Contact;

            sheet.Cell(rowNum, 4).Value = entry.ZipCode;

            sheet.Cell(rowNum, 5).Value = entry.City;

            sheet.Cell(rowNum, 6).Value = entry.Remarks;

            if (entry.IsDeparture)
            {
                sheet.Cell(rowNum, 7).Value = entry.Start;
                sheet.Cell(rowNum, 7).Style.DateFormat.Format = "hh:mm";

                sheet.Cell(rowNum, 13).Value = entry.Day.TimeTotal;
                sheet.Cell(rowNum, 13).Style.DateFormat.Format = "hh:mm";

                sheet.Cell(rowNum, 14).Value = entry.Day.TimeWorking;
                sheet.Cell(rowNum, 14).Style.DateFormat.Format = "hh:mm";
            }

            if (entry.IsOffice)
            {
                sheet.Cell(rowNum, 8).Value = entry.Duration;
                sheet.Cell(rowNum, 8).Style.DateFormat.Format = "hh:mm";
            }

            if (entry.IsPause)
            {
                sheet.Cell(rowNum, 8).Value = entry.Duration;
                sheet.Cell(rowNum, 8).Style.DateFormat.Format = "hh:mm";
            }

            if (entry.IsDeparture)
            {
                sheet.Cell(rowNum, 12).Value = entry.End;
                sheet.Cell(rowNum, 12).Style.DateFormat.Format = "hh:mm";
            }

            if (entry.IsVacation)
                sheet.Cell(rowNum, 9).Value = 1;

            if (entry.IsSick)
                sheet.Cell(rowNum, 10).Value = 1;

            if (entry.IsCelebration)
                sheet.Cell(rowNum, 11).Value = 1;
            
            rowNum++;
        }

        var table = rng.CreateTable();
        table.ShowTotalsRow = true;
        table.Field("Url.").TotalsRowFunction = XLTotalsRowFunction.Sum;
        table.Field("Kr.").TotalsRowFunction = XLTotalsRowFunction.Sum;
        table.Field("Ft.").TotalsRowFunction = XLTotalsRowFunction.Sum;
        table.Field("Spesen").TotalsRowFunction = XLTotalsRowFunction.Sum;
        table.Field("Spesen").TotalsCell.Style.DateFormat.Format = "[h]:mm;@";
        table.Field("Arb.zeit").TotalsRowFunction = XLTotalsRowFunction.Sum;
        table.Field("Arb.zeit").TotalsCell.Style.DateFormat.Format = "[h]:mm;@";
    }
}