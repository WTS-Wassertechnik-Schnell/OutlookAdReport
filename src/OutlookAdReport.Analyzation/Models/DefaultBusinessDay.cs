using OutlookAdReport.Analyzation.Services;
using OutlookAdReport.Data.Models;

namespace OutlookAdReport.Analyzation.Models;

/// <summary> A default business day.</summary>
public class DefaultBusinessDay : IBusinessDay
{
    /// <summary> Constructor.</summary>
    /// <param name="dayAnalyzerService"> The day analyzer service. </param>
    /// <param name="appointments">       The appointments. </param>
    public DefaultBusinessDay(IBusinessDayAnalyzerService dayAnalyzerService, IEnumerable<IAppointment> appointments)
    {
        DayAnalyzerService = dayAnalyzerService;
        Appointments = appointments;
    }

    /// <summary> Gets the day analyzer service.</summary>
    /// <value> The day analyzer service.</value>
    public IBusinessDayAnalyzerService DayAnalyzerService { get; }

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