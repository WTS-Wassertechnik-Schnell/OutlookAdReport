using OutlookAdReport.Analyzation.Models;

namespace OutlookAdReport.Analyzation.Services;

/// <summary> Interface for pause manager.</summary>
public interface IPauseManager
{
    /// <summary> Adds a required pause.</summary>
    /// <param name="day">    The day. </param>
    /// <param name="events"> The events. </param>
    public void AddRequiredPause(IBusinessDay day, IList<IBusinessEvent> events);
}