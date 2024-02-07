using System.Collections.ObjectModel;

namespace OutlookAdReport.Analyzation.Models;

/// <summary> Interface for business day.</summary>
public interface IBusinessDay
{
    /// <summary> Gets or sets the events.</summary>
    /// <value> The events.</value>
    public IEnumerable<IBusinessEvent> Events { get; set; }

    /// <summary> Gets or sets the office events.</summary>
    /// <value> The office events.</value>
    public IEnumerable<IBusinessEvent> OfficeEvents { get; set; }

    /// <summary> Gets or sets the departure.</summary>
    /// <value> The departure.</value>
    public IBusinessEvent? Departure { get; set; }

    /// <summary> Gets or sets the arrival.</summary>
    /// <value> The arrival.</value>
    public IBusinessEvent? Arrival { get; set; }

    /// <summary> Gets or sets the time office.</summary>
    /// <value> The time office.</value>
    public TimeSpan TimeOffice { get; set; }

    /// <summary> Gets or sets the time pause.</summary>
    /// <value> The time pause.</value>
    public TimeSpan TimePause { get; set; }

    /// <summary> Gets or sets the time working.</summary>
    /// <value> The time working.</value>
    public TimeSpan TimeWorking { get; set; }

    /// <summary> Gets or sets the time total.</summary>
    /// <value> The time total.</value>
    public TimeSpan TimeTotal { get; set; }
}