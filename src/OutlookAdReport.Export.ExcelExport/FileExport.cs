using Microsoft.Extensions.Options;
using OutlookAdReport.Analyzation.Models;
using OutlookAdReport.Export.ExcelExport.Options;

namespace OutlookAdReport.Export.ExcelExport;

/// <summary>   A file export. </summary>
public abstract class FileExport : Export, IFileExport
{
    /// <summary>   Specialized constructor for use only by derived class. </summary>
    /// <param name="options">  Options for controlling the operation. </param>
    protected FileExport(IOptions<ExportOptions> options) : base(options)
    {
    }

    /// <summary>   Gets default folder. </summary>
    /// <returns>   The default folder. </returns>
    protected abstract string GetDefaultFolder();

    /// <summary>   Gets default file name. </summary>
    /// <returns>   The default file name. </returns>
    protected abstract string GetDefaultFileName();

    /// <summary>   Exports the given events. </summary>
    /// <param name="events">   The events. </param>
    public virtual void Export(IEnumerable<IBusinessEvent> events)
    {
        var folder = GetDefaultFolder();
        var file = GetDefaultFileName();
        var fileName = Path.Combine(folder, file);

        Export(events, fileName);
    }

    /// <summary>   Exports. </summary>
    /// <param name="events">   The events. </param>
    /// <param name="fileName"> Filename of the file. </param>
    public abstract void Export(IEnumerable<IBusinessEvent> events, string fileName);
}