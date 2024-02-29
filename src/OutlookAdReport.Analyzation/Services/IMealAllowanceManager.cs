using OutlookAdReport.Analyzation.Models;

namespace OutlookAdReport.Analyzation.Services;

/// <summary>   Interface for meal allowance manager. </summary>
public interface IMealAllowanceManager
{
    /// <summary>   Enumerates compute allowances in this collection. </summary>
    /// <param name="events">   The events. </param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process compute allowances in this
    ///     collection.
    /// </returns>
    IEnumerable<MealAllowance> ComputeAllowances(IEnumerable<IBusinessEvent> events);
}