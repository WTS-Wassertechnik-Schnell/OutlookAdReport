using OutlookAdReport.Analyzation.Models;

namespace OutlookAdReport.Export.ExcelExport;

/// <summary>   Interface for export. </summary>
public interface IExport
{
    /// <summary>   Exports the given events. </summary>
    /// <param name="events">   The events. </param>
    void Export(IEnumerable<IBusinessEvent> events);
}