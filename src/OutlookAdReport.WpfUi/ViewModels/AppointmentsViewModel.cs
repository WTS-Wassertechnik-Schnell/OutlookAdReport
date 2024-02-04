using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using OutlookAdReport.Data.Services;
using ReactiveUI;
using Splat;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the appointments.</summary>
public class AppointmentsViewModel : ReactiveObject
{
    /// <summary> (Immutable) the appointments.</summary>
    private readonly ObservableAsPropertyHelper<ObservableCollection<AppointmentViewModel>> _appointments;

    /// <summary> Gets the query service.</summary>
    /// <value> The query service.</value>
    public IAppointmentQueryService QueryService { get; }

    /// <summary> Gets the appointments.</summary>
    /// <value> The appointments.</value>
    public ObservableCollection<AppointmentViewModel> Appointments => _appointments.Value;

    /// <summary> Constructor.</summary>
    /// <param name="queryService"> (Optional) The query service. </param>
    public AppointmentsViewModel(IAppointmentQueryService? queryService = null)
    {
        QueryService = queryService ?? Locator.Current.GetService<IAppointmentQueryService>()!;

        _appointments = QueryService.Appointments
            .ToObservableChangeSet()
            .ToCollection()
            .Select(x =>
                new ObservableCollection<AppointmentViewModel>(
                    x.Select(y => new AppointmentViewModel(y))))
            .ToProperty(this, x => x.Appointments);
    }
}