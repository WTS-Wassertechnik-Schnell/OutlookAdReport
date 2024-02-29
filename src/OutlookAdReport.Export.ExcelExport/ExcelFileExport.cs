using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2019.Excel.RichData2;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.Extensions.Options;
using OutlookAdReport.Analyzation.Models;
using OutlookAdReport.Analyzation.Services;
using OutlookAdReport.Export.ExcelExport.Options;

namespace OutlookAdReport.Export.ExcelExport;

/// <summary>   An excel file export. </summary>
public class ExcelFileExport : FileExport
{
    /// <summary>   Gets the manager for meal allowance. </summary>
    /// <value> The meal allowance manager. </value>
    public IMealAllowanceManager MealAllowanceManager { get; }

    /// <summary>   Constructor. </summary>
    /// <param name="options">              Options for controlling the operation. </param>
    /// <param name="mealAllowanceManager"> Manager for meal allowance. </param>
    public ExcelFileExport(IOptions<ExportOptions> options, IMealAllowanceManager mealAllowanceManager) : base(options)
    {
        MealAllowanceManager = mealAllowanceManager;
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

        var ev = events.ToList();
        var workbook = new XLWorkbook(fileName);
        ExportWorkReportData(workbook.Worksheet("Arbeitsbericht"), ev);
        ExportMealAllowanceData(workbook.Worksheet("Spesen"), ev);
        
        workbook.Save();
        //throw new NotImplementedException();
    }

    /// <summary>   Export meal allowance data. </summary>
    /// <param name="sheet">    The sheet. </param>
    /// <param name="events">   The events. </param>
    protected virtual void ExportMealAllowanceData(IXLWorksheet? sheet, List<IBusinessEvent> events)
    {
        if (sheet == null) return;

        var allowances = MealAllowanceManager.ComputeAllowances(events).ToList();
        sheet.Row(1).InsertRowsBelow(allowances.Count);
        var rng = sheet.Range(sheet.Cell(1, 1), sheet.Cell(allowances.Count + 1, 6));
        rng.Clear(XLClearOptions.AllFormats);

        var rowNum = 2;

        foreach (var entry in allowances)
        {
            sheet.Cell(rowNum, 1).Value = entry.Date;
            sheet.Cell(rowNum, 1).Style.DateFormat.Format = "d.M";

            sheet.Cell(rowNum, 2).Value = entry.Start;
            sheet.Cell(rowNum, 2).Style.DateFormat.Format = "hh:mm";

            sheet.Cell(rowNum, 3).Value = entry.End;
            sheet.Cell(rowNum, 3).Style.DateFormat.Format = "hh:mm";

            sheet.Cell(rowNum, 4).Value = string.Join(", ", entry.Locations);

            sheet.Cell(rowNum, 5).Value = entry.TimeOutOfOffice;
            sheet.Cell(rowNum, 5).Style.DateFormat.Format = "hh:mm";

            sheet.Cell(rowNum, 6).Value = entry.Allowance;
            sheet.Cell(rowNum, 6).Style.NumberFormat.Format = "#.00 \u20ac;-#.00 \u20ac";
            
            rowNum++;
        }

        var table = rng.CreateTable();
        table.ShowTotalsRow = true;
        table.Field("Ver.-Pausch.").TotalsRowFunction = XLTotalsRowFunction.Sum;
        table.Field("Ver.-Pausch.").TotalsCell.Style.DateFormat.Format = "#.00 \u20ac;-#.00 \u20ac";
    }

    /// <summary>   Export work report data. </summary>
    /// <param name="sheet">    The sheet. </param>
    /// <param name="events">   The events. </param>
    protected virtual void ExportWorkReportData(IXLWorksheet? sheet, List<IBusinessEvent> events)
    {
        if (sheet == null) return;

        sheet.Row(1).InsertRowsBelow(events.Count);
        var rng = sheet.Range(sheet.Cell(1, 1), sheet.Cell(events.Count + 1, 14));
        rng.Clear(XLClearOptions.AllFormats);

        var rowNum = 2;

        foreach (var entry in events)
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