using Microsoft.Extensions.Options;
using OutlookAdReport.Export.ExcelExport.Options;

namespace OutlookAdReport.Export.ExcelExport;

/// <summary>   An export. </summary>
public abstract class Export
{
    /// <summary>   Gets or sets options for controlling the operation. </summary>
    /// <value> The options. </value>
    public ExportOptions Options { get; set; }

    /// <summary>   Specialized constructor for use only by derived class. </summary>
    /// <param name="options">  The options. </param>
    protected Export(IOptions<ExportOptions> options)
    {
        Options = options.Value;
    }
}