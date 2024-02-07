using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using OutlookAdReport.Analyzation.Models;
using OutlookAdReport.Analyzation.Options;
using OutlookAdReport.Data.Models;
using OutlookAdReport.Data.Services;

namespace OutlookAdReport.Analyzation.Services;

/// <summary> A service for accessing default business day analyzers information.</summary>
public class DefaultBusinessDayAnalyzerService : IBusinessDayAnalyzerService
{
    /// <summary> Gets the event service.</summary>
    /// <value> The event service.</value>
    public IEventService EventService { get; }

    /// <summary> Gets the manager for pause.</summary>
    /// <value> The pause manager.</value>
    public IPauseManager PauseManager { get; }

    /// <summary> Gets options for controlling the operation.</summary>
    /// <value> The options.</value>
    public AnalyzationOptions Options { get; }

    /// <summary> Occurs when a property value changes.</summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary> Gets the business days.</summary>
    /// <value> The business days.</value>
    public ObservableCollection<IBusinessDay> BusinessDays { get; } = new ();

    /// <summary> Gets the business events.</summary>
    /// <value> The business events.</value>
    public ObservableCollection<IBusinessEvent> BusinessEvents { get; } = new ();

    /// <summary> Constructor.</summary>
    /// <param name="options">      Options for controlling the operation. </param>
    /// <param name="eventService"> The event service. </param>
    /// <param name="pauseManager"> Manager for pause. </param>
    public DefaultBusinessDayAnalyzerService(IOptions<AnalyzationOptions> options, IEventService eventService, IPauseManager pauseManager)
    {
        EventService = eventService;
        PauseManager = pauseManager;
        Options = options.Value;
    }

    /// <summary> Enumerates analyze in this collection.</summary>
    /// <param name="appointments"> The appointments. </param>
    /// <returns>An enumerator that allows foreach to be used to process analyze in this collection.</returns>
    public IEnumerable<IBusinessDay> Analyze(IEnumerable<IAppointment> appointments)
    {
        BusinessDays.Clear();
        BusinessEvents.Clear();

        var days = appointments
            .GroupBy(a => a.Start.Date)
            .OrderBy(g => g.Key)
            .Select(g => new DefaultBusinessDay(this, EventService, PauseManager, g))
            .ToList();
        
        foreach (var day in days)
        {
            BusinessDays.Add(day);
            foreach (var bEvent in day.Events)
            {
                BusinessEvents.Add(bEvent);
            }
        }

        return days;
    }

    /// <summary> Query if 'businessEvent' is departure.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if departure, false if not.</returns>
    public bool IsDeparture(IBusinessEvent businessEvent)
    {
        return 
            !string.IsNullOrWhiteSpace(businessEvent.Customer) && 
            businessEvent.Customer.StartsWith(Options.DepartureKey);
    }

    /// <summary> Query if 'businessEvent' is arrival.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if arrival, false if not.</returns>
    public bool IsArrival(IBusinessEvent businessEvent)
    {
        return
            !string.IsNullOrWhiteSpace(businessEvent.Customer) &&
            businessEvent.Customer.StartsWith(Options.HomeKey);
    }

    /// <summary> Query if 'businessEvent' is office.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if office, false if not.</returns>
    public bool IsOffice(IBusinessEvent businessEvent)
    {
        return
            !string.IsNullOrWhiteSpace(businessEvent.Customer) &&
            businessEvent.Customer.StartsWith(Options.OfficeKey);
    }

    /// <summary> Query if 'businessEvent' is vacation.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if vacation, false if not.</returns>
    public bool IsVacation(IBusinessEvent businessEvent)
    {
        return
            !string.IsNullOrWhiteSpace(businessEvent.Customer) &&
            businessEvent.Customer.StartsWith(Options.VacationKey);
    }

    /// <summary> Query if 'businessEvent' is sick.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if sick, false if not.</returns>
    public bool IsSick(IBusinessEvent businessEvent)
    {
        return
            !string.IsNullOrWhiteSpace(businessEvent.Customer) &&
            businessEvent.Customer.StartsWith(Options.SickKey);
    }

    /// <summary> Query if 'businessEvent' is celebration.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if celebration, false if not.</returns>
    public bool IsCelebration(IBusinessEvent businessEvent)
    {
        return
            !string.IsNullOrWhiteSpace(businessEvent.Customer) &&
            businessEvent.Customer.StartsWith(Options.CelebrationKey);
    }

    /// <summary> Query if 'businessEvent' is pause.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if pause, false if not.</returns>
    public bool IsPause(IBusinessEvent businessEvent)
    {
        return
            !string.IsNullOrWhiteSpace(businessEvent.Customer) &&
            businessEvent.Customer.Equals(Options.PauseKey);
    }

    /// <summary> Executes the 'property changed' action.</summary>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary> Sets a field.</summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    /// <param name="field">        [in,out] The field. </param>
    /// <param name="value">        The value. </param>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    /// <returns> True if it succeeds, false if it fails.</returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}