using System.Collections.ObjectModel;
using System.ComponentModel;
using OutlookAdReport.Analyzation.Models;
using OutlookAdReport.Data.Models;

namespace OutlookAdReport.Analyzation.Services;

/// <summary> Interface for business day analyzer service.</summary>
public interface IBusinessDayAnalyzerService : INotifyPropertyChanged
{
    /// <summary> Gets the business days.</summary>
    /// <value> The business days.</value>
    public ObservableCollection<IBusinessDay> BusinessDays { get; }

    /// <summary> Gets the business events.</summary>
    /// <value> The business events.</value>
    public ObservableCollection<IBusinessEvent> BusinessEvents { get; }

    /// <summary> Enumerates analyze in this collection.</summary>
    /// <param name="appointments"> The appointments. </param>
    /// <returns>An enumerator that allows foreach to be used to process analyze in this collection.</returns>
    public IEnumerable<IBusinessDay> Analyze(IEnumerable<IAppointment> appointments);
    
    /// <summary> Query if 'businessEvent' is departure.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if departure, false if not.</returns>
    public bool IsDeparture(IBusinessEvent businessEvent);

    /// <summary> Query if 'businessEvent' is arrival.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if arrival, false if not.</returns>
    public bool IsArrival(IBusinessEvent businessEvent);

    /// <summary> Query if 'businessEvent' is office.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if office, false if not.</returns>
    public bool IsOffice(IBusinessEvent businessEvent);

    /// <summary> Query if 'businessEvent' is vacation.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if vacation, false if not.</returns>
    public bool IsVacation(IBusinessEvent businessEvent);

    /// <summary> Query if 'businessEvent' is sick.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if sick, false if not.</returns>
    public bool IsSick(IBusinessEvent businessEvent);

    /// <summary> Query if 'businessEvent' is celebration.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if celebration, false if not.</returns>
    public bool IsCelebration(IBusinessEvent businessEvent);

    /// <summary> Query if 'businessEvent' is pause.</summary>
    /// <param name="businessEvent"> The business event. </param>
    /// <returns> True if pause, false if not.</returns>
    public bool IsPause(IBusinessEvent businessEvent);
}