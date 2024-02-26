using OutlookAdReport.Analyzation.Models;

namespace OutlookAdReport.Export.ExcelExport;

/// <summary>   Interface for file export. </summary>
public interface IFileExport : IExport
{
    /// <summary>   Exports. </summary>
    /// <param name="events">   The events. </param>
    /// <param name="fileName"> Filename of the file. </param>
    void Export(IEnumerable<IBusinessEvent> events, string fileName);
}