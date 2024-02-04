using OutlookAdReport.Analyzation.Services;
using OutlookAdReport.Data.Models;
using OutlookAdReport.Data.Services;

namespace OutlookAdReport.Analyzation.Models;

/// <summary> A default business day.</summary>
public class DefaultBusinessDay : IBusinessDay
{
    /// <summary> Constructor.</summary>
    /// <param name="dayAnalyzerService"> The day analyzer service. </param>
    /// <param name="eventService">       The event service. </param>
    /// <param name="appointments">       The appointments. </param>
    public DefaultBusinessDay(IBusinessDayAnalyzerService dayAnalyzerService, IEventService eventService, IEnumerable<IAppointment> appointments)
    {
        DayAnalyzerService = dayAnalyzerService;
        EventService = eventService;
        Events = Analyze(appointments);
        OfficeEvents = new IBusinessEvent[]{};
    }

    /// <summary> Analyzes the given appointments.</summary>
    /// <param name="appointments"> The appointments. </param>
    private IEnumerable<BusinessEvent> Analyze(IEnumerable<IAppointment> appointments)
    {
        var events = new List<BusinessEvent>();

        foreach (var appointment in appointments.OrderBy(a => a.Start))
        {
            events.Add(new BusinessEvent(this, appointment));
        }

        CheckFreeDays(events);
        AssignDeparture(events);
        AssignArrival(events);
        ProcessOfficeEvents(events);

        if (!IsFreeDay && Arrival != null && Departure != null)
        {
            TimeTotal = Arrival.End - Departure.Start;
            TimeWorking = TimeTotal - Arrival.Duration - Departure.Duration;
        }

        return events;
    }

    /// <summary> Check free days.</summary>
    private void CheckFreeDays(List<BusinessEvent> events)
    {
        var vacations = events
            .Where(e => DayAnalyzerService.IsVacation(e))
            .ToList();

        var sickEvents = events
            .Where(e => DayAnalyzerService.IsSick(e))
            .ToList();

        var celebEvents = events
            .Where(e => DayAnalyzerService.IsCelebration(e))
            .ToList();

        if (vacations.Any() || sickEvents.Any() || celebEvents.Any())
        {
            IsFreeDay = true;
        }
    }

    /// <summary> Assign departure.</summary>
    /// <param name="events"> The events. </param>
    private void AssignDeparture(List<BusinessEvent> events)
    {
        if (IsFreeDay)
        {
            events.RemoveAll(e => DayAnalyzerService.IsDeparture(e));
            return;
        }

        var departures = events
            .Where(e => DayAnalyzerService.IsDeparture(e))
            .ToList();

        if (!departures.Any())
        {
            EventService.AddEvent($"Keine Abfahrt für den '{events.First().Start.ToShortDateString()}' gefunden.", EventMessageType.Warning);
        }
        else if (departures.Count == 1)
        {
            Departure = departures.Single();
        }
        else
        {
            Departure = departures.OrderBy(d => d.Start).Last();
            EventService.AddEvent($"Mehrere Abfahrten für den '{events.First().Start.ToShortDateString()}' gefunden.", EventMessageType.Warning);
        }
    }

    /// <summary> Assign arrival.</summary>
    /// <param name="events"> The events. </param>
    private void AssignArrival(List<BusinessEvent> events)
    {
        if (IsFreeDay)
        {
            events.RemoveAll(e => DayAnalyzerService.IsArrival(e));
            return;
        }

        var arrivals = events
            .Where(e => DayAnalyzerService.IsArrival(e))
            .ToList();

        if (!arrivals.Any())
        {
            EventService.AddEvent($"Keine Rückkunft für den '{events.First().Start.ToShortDateString()}' gefunden.", EventMessageType.Warning);
        }
        else if (arrivals.Count == 1)
        {
            Arrival = arrivals.Single();
        }
        else
        {
            Arrival = arrivals.OrderBy(d => d.Start).Last();
            EventService.AddEvent($"Mehrere Rückkunften für den '{events.First().Start.ToShortDateString()}' gefunden.", EventMessageType.Warning);
        }
    }

    /// <summary> Process the office events described by events.</summary>
    /// <param name="events"> The events. </param>
    private void ProcessOfficeEvents(List<BusinessEvent> events)
    {
        if (IsFreeDay)
        {
            return;
        }

        var officeEvents = events
            .Where(e => DayAnalyzerService.IsOffice(e))
            .ToList();

        OfficeEvents = officeEvents;
        TimeOffice = new TimeSpan(officeEvents.Sum(e => (e.End - e.Start).Ticks));
    }

    /// <summary> Gets the day analyzer service.</summary>
    /// <value> The day analyzer service.</value>
    public IBusinessDayAnalyzerService DayAnalyzerService { get; }

    /// <summary> Gets the event service.</summary>
    /// <value> The event service.</value>
    public IEventService EventService { get; }

    /// <summary> Gets or sets a value indicating whether this object is free day.</summary>
    /// <value> True if this object is free day, false if not.</value>
    public bool IsFreeDay { get; set; }

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

    /// <summary> Gets or sets the time working.</summary>
    /// <value> The time working.</value>
    public TimeSpan TimeWorking { get; set; }

    /// <summary> Gets or sets the time total.</summary>
    /// <value> The time total.</value>
    public TimeSpan TimeTotal { get; set; }
}