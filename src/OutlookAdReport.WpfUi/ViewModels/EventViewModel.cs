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

    /// <summary> Constructor.</summary>
    /// <param name="businessEvent"> The business event. </param>
    public EventViewModel(IBusinessEvent businessEvent)
    {
        BusinessEvent = businessEvent;

        if (businessEvent == businessEvent.Day.Departure)
            Departure = businessEvent.Start;

        if (businessEvent == businessEvent.Day.Arrival)
            Arrival = businessEvent.Start;

        if (businessEvent.Day.OfficeEvents.Any())
        {
            if (businessEvent.Day.OfficeEvents.Contains(businessEvent))
            {
                Office = businessEvent.End - businessEvent.Start;
            }
        }
    }
}