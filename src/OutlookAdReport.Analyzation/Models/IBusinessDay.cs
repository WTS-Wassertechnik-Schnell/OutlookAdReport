using OutlookAdReport.Data.Models;

namespace OutlookAdReport.Analyzation.Models;

/// <summary> Interface for business day.</summary>
public interface IBusinessDay
{
    /// <summary> Gets or sets the appointments.</summary>
    /// <value> The appointments.</value>
    public IEnumerable<IAppointment> Appointments { get; set; }

    /// <summary> Gets or sets the departure.</summary>
    /// <value> The departure.</value>
    public IAppointment? Departure { get; set; }

    /// <summary> Gets or sets the arrival.</summary>
    /// <value> The arrival.</value>
    public IAppointment? Arrival { get; set; }

    /// <summary> Gets or sets the time office.</summary>
    /// <value> The time office.</value>
    public TimeSpan TimeOffice { get; set; }

    /// <summary> Gets or sets the time working.</summary>
    /// <value> The time working.</value>
    public TimeSpan TimeWorking { get; set; }

    /// <summary> Gets or sets the time total.</summary>
    /// <value> The time total.</value>
    public TimeSpan TimeTotal { get; set; }
}