using System.Collections.ObjectModel;
using OutlookAdReport.Data.Models;

namespace OutlookAdReport.Data.Services;

/// <summary> Interface for appointment query service.</summary>
public interface IAppointmentQueryService
{
    /// <summary> Gets the appointments.</summary>
    /// <value> The appointments.</value>
    public ObservableCollection<IAppointment> Appointments { get; }

    /// <summary> Queries the appointments.</summary>
    /// <param name="loginResult"> The login result. </param>
    /// <param name="start">       The start Date/Time. </param>
    /// <param name="end">         The end Date/Time. </param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process query appointments in this
    ///     collection.
    /// </returns>
    public Task<IEnumerable<IAppointment>> QueryAppointments(ILoginResult loginResult, DateTime start, DateTime end);
}