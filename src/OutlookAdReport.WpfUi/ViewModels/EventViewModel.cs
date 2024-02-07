using OutlookAdReport.Analyzation.Models;
using ReactiveUI;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the event.</summary>
public class EventViewModel : ReactiveObject
{
    /// <summary> Gets the business event.</summary>
    /// <value> The business event.</value>
    public IBusinessEvent BusinessEvent { get; }

    /// <summary> Gets the Date/Time of the date.</summary>
    /// <value> The date.</value>
    public DateTime Date => BusinessEvent.Start.Date;

    /// <summary> Gets or sets the Date/Time of the departure.</summary>
    /// <value> The departure.</value>
    public DateTime? Departure { get; set; }

    /// <summary> Gets or sets the Date/Time of the arrival.</summary>
    /// <value> The arrival.</value>
    public DateTime? Arrival { get; set; }

    /// <summary> Gets or sets the office.</summary>
    /// <value> The office.</value>
    public TimeSpan? Office { get; set; }

    /// <summary> Gets the office string.</summary>
    /// <value> The office string.</value>
    public string? OfficeString
    {
        get
        {
            if (Office == null) return null;
            return (Office < TimeSpan.Zero ? "-" : "") + Office.Value.ToString("hh\\:mm");
        }
    }

    /// <summary> Gets or sets the number of. </summary>
    /// <value> The total.</value>
    public TimeSpan? Total { get; set; }

    /// <summary> Gets or sets the working.</summary>
    /// <value> The working.</value>
    public TimeSpan? Working { get; set; }

    /// <summary> Constructor.</summary>
    /// <param name="businessEvent"> The business event. </param>
    public EventViewModel(IBusinessEvent businessEvent)
    {
        BusinessEvent = businessEvent;

        if (businessEvent.IsDeparture)
        {
            Departure = businessEvent.Start;
            Total = businessEvent.Day.TimeTotal;
            Working = businessEvent.Day.TimeWorking;
        }

        if (businessEvent.IsArrival)
            Arrival = businessEvent.Start;

        if (businessEvent.IsOffice)
            Office = businessEvent.Duration;

        if (businessEvent.IsPause)
            Office = businessEvent.Duration;
    }
}