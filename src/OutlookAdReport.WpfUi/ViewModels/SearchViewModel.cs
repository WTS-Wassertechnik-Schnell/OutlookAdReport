using System.Collections.ObjectModel;
using ReactiveUI;
using System.Reactive;
using OutlookAdReport.Data;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the search.</summary>
public class SearchViewModel : ReactiveObject
{
    /// <summary> Gets the application view model.</summary>
    /// <value> The application view model.</value>
    public AppViewModel AppViewModel { get; }

    /// <summary> Gets the query service.</summary>
    /// <value> The query service.</value>
    public IAppointmentQueryService QueryService { get; }

    /// <summary> Default constructor.</summary>
    public SearchViewModel(AppViewModel appViewModel, IAppointmentQueryService queryService)
    {
        AppViewModel = appViewModel;
        QueryService = queryService;

        var today = DateTime.Today;
        var month = new DateTime(today.Year, today.Month, 1);

        From = month.AddMonths(-1);
        Till = month.AddDays(-1);

        QueryAppointmentsCommand = ReactiveCommand.CreateFromTask(QueryAppointmentsAsync);
    }

    /// <summary> Gets the 'query appointments' command.</summary>
    /// <value> The 'query appointments' command.</value>
    public ReactiveCommand<Unit, Unit> QueryAppointmentsCommand { get; }

    private DateTime _from;

    /// <summary> Gets or sets the Date/Time of from.</summary>
    /// <value> from.</value>
    public DateTime From
    {
        get => _from;
        set => this.RaiseAndSetIfChanged(ref _from, value);
    }

    private DateTime _till;
    
    /// <summary> Gets or sets the Date/Time of the till.</summary>
    /// <value> The till.</value>
    public DateTime Till
    {
        get => _till;
        set => this.RaiseAndSetIfChanged(ref _till, value);
    }

    private ObservableCollection<AppointmentViewModel> _appointments = new();

    /// <summary> Gets or sets the appointments.</summary>
    /// <value> The appointments.</value>
    public ObservableCollection<AppointmentViewModel> Appointments
    {
        get => _appointments;
        private set => this.RaiseAndSetIfChanged(ref _appointments, value);
    }

    /// <summary> Queries appointments asynchronous.</summary>
    /// <param name="ct"> A token that allows processing to be cancelled. </param>
    /// <returns> A Task.</returns>
    public async Task QueryAppointmentsAsync(CancellationToken ct)
    {
        try
        {
            var appointments = await QueryService
                .QueryAppointments(AppViewModel.LoginViewModel.LoginResult!, From, Till);
            Appointments = new ObservableCollection<AppointmentViewModel>(appointments
                .Select(a => new AppointmentViewModel(a))
                .OrderBy(a => a.Appointment.Start));
        }
        catch (Exception e)
        {
            AppViewModel.Events.Add(new EventMessageViewModel
            {
                Message = e.Message,
                MessageType = EventMessageType.Error
            });
        }
    }
}