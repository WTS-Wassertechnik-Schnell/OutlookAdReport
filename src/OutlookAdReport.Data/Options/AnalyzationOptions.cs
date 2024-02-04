namespace OutlookAdReport.Data.Options;

/// <summary> An analyzation options.</summary>
public class AnalyzationOptions
{
    /// <summary> Gets or sets the departure key.</summary>
    /// <value> The departure key.</value>
    public string DepartureKey { get; set; } = string.Empty;

    /// <summary> Gets or sets the home key.</summary>
    /// <value> The home key.</value>
    public string HomeKey { get; set; } = string.Empty;

    /// <summary> Gets or sets the office key.</summary>
    /// <value> The office key.</value>
    public string OfficeKey { get; set; } = string.Empty;
}