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
    /// <param name="pauseManager">       Manager for pause. </param>
    /// <param name="appointments">       The appointments. </param>
    public DefaultBusinessDay(IBusinessDayAnalyzerService dayAnalyzerService, IEventService eventService, IPauseManager pauseManager, IEnumerable<IAppointment> appointments)
    {
        DayAnalyzerService = dayAnalyzerService;
        EventService = eventService;
        PauseManager = pauseManager;
        OfficeEvents = new IBusinessEvent[] { };
        Events = Analyze(appointments);
    }

    /// <summary> Analyzes the given appointments.</summary>
    /// <param name="appointments"> The appointments. </param>
    private IEnumerable<IBusinessEvent> Analyze(IEnumerable<IAppointment> appointments)
    {
        var events = new List<IBusinessEvent>();

        foreach (var appointment in appointments.OrderBy(a => a.Start))
        {
            events.Add(new BusinessEvent(this, appointment));
        }

        CheckFreeDays(events);
        AssignDeparture(events);
        AssignArrival(events);
        ProcessOfficeEvents(events);
        
        if (!IsFree && Arrival != null && Departure != null)
        {
            TimeTotal = Arrival.End - Departure.Start;
            TimeWorking = TimeTotal - Arrival.Duration - Departure.Duration;
        }

        if (!IsFree)
        {
            PauseManager.AddRequiredPause(this, events);
            ProcessPauseEvents(events);
            var pauseTime = new TimeSpan(events.Where(e => e.IsPause).Sum(e => e.Duration.Ticks));
            TimeWorking -= pauseTime;
        }

        return events;
    }
    
    /// <summary> Check free days.</summary>
    private void CheckFreeDays(List<IBusinessEvent> events)
    {
        var vacations = events
            .Where(e => DayAnalyzerService.IsVacation(e))
            .ToList();
        if (vacations.Any())
        {
            IsVacation = true;
            foreach (var vacation in vacations)
                vacation.IsVacation = true;
        }

        var sickEvents = events
            .Where(e => DayAnalyzerService.IsSick(e))
            .ToList();
        if (sickEvents.Any())
        {
            IsSick = true;
            foreach (var sd in sickEvents)
                sd.IsSick = true;
        }

        var celebEvents = events
            .Where(e => DayAnalyzerService.IsCelebration(e))
            .ToList();
        if (celebEvents.Any())
        {
            IsCelebration = true;
            foreach (var celeb in celebEvents)
                celeb.IsCelebration = true;
        }

        if (vacations.Any() || sickEvents.Any() || celebEvents.Any())
        {
            IsFree = true;
        }
    }

    /// <summary> Assign departure.</summary>
    /// <param name="events"> The events. </param>
    private void AssignDeparture(List<IBusinessEvent> events)
    {
        if (IsFree)
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
            Departure.IsDeparture = true;
        }
        else
        {
            Departure = departures.OrderBy(d => d.Start).Last();
            Departure.IsDeparture = true;
            EventService.AddEvent($"Mehrere Abfahrten für den '{events.First().Start.ToShortDateString()}' gefunden.", EventMessageType.Warning);
        }
    }

    /// <summary> Assign arrival.</summary>
    /// <param name="events"> The events. </param>
    private void AssignArrival(List<IBusinessEvent> events)
    {
        if (IsFree)
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
            Arrival.IsArrival = true;
        }
        else
        {
            Arrival = arrivals.OrderBy(d => d.Start).Last();
            Arrival.IsArrival = true;
            EventService.AddEvent($"Mehrere Rückkunften für den '{events.First().Start.ToShortDateString()}' gefunden.", EventMessageType.Warning);
        }
    }

    /// <summary> Process the office events described by events.</summary>
    /// <param name="events"> The events. </param>
    private void ProcessOfficeEvents(List<IBusinessEvent> events)
    {
        if (IsFree)
        {
            return;
        }

        var officeEvents = events
            .Where(e => DayAnalyzerService.IsOffice(e))
            .ToList();

        foreach (var oe in officeEvents)
        {
            oe.IsOffice = true;
        }

        OfficeEvents = officeEvents;
        TimeOffice = new TimeSpan(officeEvents.Sum(e => e.Duration.Ticks));
    }

    /// <summary> Process the pause events described by events.</summary>
    /// <param name="events"> The events. </param>
    private void ProcessPauseEvents(List<IBusinessEvent> events)
    {
        var pauseEvents = events
            .Where(e => DayAnalyzerService.IsPause(e))
            .ToList();

        foreach (var pe in pauseEvents)
        {
            pe.IsPause = true;
        }

        TimePause = new TimeSpan(pauseEvents.Sum(e => (e.End - e.Start).Ticks));
    }

    /// <summary> Gets the day analyzer service.</summary>
    /// <value> The day analyzer service.</value>
    public IBusinessDayAnalyzerService DayAnalyzerService { get; }

    /// <summary> Gets the event service.</summary>
    /// <value> The event service.</value>
    public IEventService EventService { get; }

    /// <summary> Gets the manager for pause.</summary>
    /// <value> The pause manager.</value>
    public IPauseManager PauseManager { get; }

    /// <summary> Gets or sets a value indicating whether this object is free.</summary>
    /// <value> True if this object is free, false if not.</value>
    public bool IsFree { get; set; }

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

    /// <summary>   Gets or sets a value indicating whether this object is sick. </summary>
    /// <value> True if this object is sick, false if not. </value>
    public bool IsSick { get; set; }

    /// <summary>   Gets or sets a value indicating whether this object is celebration. </summary>
    /// <value> True if this object is celebration, false if not. </value>
    public bool IsCelebration { get; set; }

    /// <summary>   Gets or sets a value indicating whether this object is vacation. </summary>
    /// <value> True if this object is vacation, false if not. </value>
    public bool IsVacation { get; set; }

    /// <summary> Gets or sets the time working.</summary>
    /// <value> The time working.</value>
    public TimeSpan TimeWorking { get; set; }

    /// <summary> Gets or sets the time total.</summary>
    /// <value> The time total.</value>
    public TimeSpan TimeTotal { get; set; }
}