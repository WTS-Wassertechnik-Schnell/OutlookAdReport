namespace OutlookAdReport.Analyzation.Options;

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

    /// <summary> Gets or sets the pause key.</summary>
    /// <value> The pause key.</value>
    public string PauseKey { get; set; } = string.Empty;

    /// <summary> Gets or sets the vacation key.</summary>
    /// <value> The vacation key.</value>
    public string VacationKey { get; set; } = string.Empty;

    /// <summary> Gets or sets the sick key.</summary>
    /// <value> The sick key.</value>
    public string SickKey { get; set; } = string.Empty;

    /// <summary> Gets or sets the celebration key.</summary>
    /// <value> The celebration key.</value>
    public string CelebrationKey { get; set; } = string.Empty;

    /// <summary> Gets or sets options for controlling the pause.</summary>
    /// <value> Options that control the pause.</value>
    public IEnumerable<PauseOption> PauseOptions { get; set; } = new List<PauseOption>();
}