namespace OutlookAdReport.Analyzation.Options;

/// <summary> A pause option.</summary>
public class PauseOption
{
    /// <summary> Gets or sets the operator.</summary>
    /// <value> The operator.</value>
    public string Operator { get; set; } = string.Empty;

    /// <summary> Gets or sets the hours at work.</summary>
    /// <value> The hours at work.</value>
    public TimeSpan HoursAtWork { get; set; }

    /// <summary> Gets or sets the pause time.</summary>
    /// <value> The pause time.</value>
    public TimeSpan PauseTime { get; set; }
}