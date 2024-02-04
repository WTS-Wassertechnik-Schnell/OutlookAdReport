using System.Collections.ObjectModel;
using System.Reactive;
using OutlookAdReport.Data;
using OutlookAdReport.WpfUi.Services;
using ReactiveUI;
using Splat;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the search.</summary>
public class SearchViewModel : ReactiveObject
{
    private ObservableCollection<AppointmentViewModel> _appointments = new();

    private DateTime _from;

    private DateTime _till;

    /// <summary> Default constructor.</summary>
    /// <param name="queryService"> (Optional) The query service. </param>
    /// <param name="eventService"> (Optional) The event service. </param>
    /// <param name="loginService"> (Optional) The login service. </param>
    public SearchViewModel(IAppointmentQueryService? queryService = null, IEventService? eventService = null,
        ILoginService? loginService = null)
    {
        QueryService = queryService ?? Locator.Current.GetService<IAppointmentQueryService>()!;
        EventService = eventService ?? Locator.Current.GetService<IEventService>()!;
        LoginService = loginService ?? Locator.Current.GetService<ILoginService>()!;

        var today = DateTime.Today;
        var month = new DateTime(today.Year, today.Month, 1);

        From = month.AddMonths(-1);
        Till = month.AddDays(-1);

        QueryAppointmentsCommand = ReactiveCommand.CreateFromTask(QueryAppointmentsAsync);
    }

    /// <summary> Gets the query service.</summary>
    /// <value> The query service.</value>
    public IAppointmentQueryService QueryService { get; }

    /// <summary> Gets the event service.</summary>
    /// <value> The event service.</value>
    public IEventService EventService { get; }

    /// <summary> Gets the login service.</summary>
    /// <value> The login service.</value>
    public ILoginService LoginService { get; }

    /// <summary> Gets the 'query appointments' command.</summary>
    /// <value> The 'query appointments' command.</value>
    public ReactiveCommand<Unit, Unit> QueryAppointmentsCommand { get; }

    /// <summary> Gets or sets the Date/Time of from.</summary>
    /// <value> from.</value>
    public DateTime From
    {
        get => _from;
        set => this.RaiseAndSetIfChanged(ref _from, value);
    }

    /// <summary> Gets or sets the Date/Time of the till.</summary>
    /// <value> The till.</value>
    public DateTime Till
    {
        get => _till;
        set => this.RaiseAndSetIfChanged(ref _till, value);
    }

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
                .QueryAppointments(LoginService.LoginResult!, From, Till);
            Appointments = new ObservableCollection<AppointmentViewModel>(appointments
                .Select(a => new AppointmentViewModel(a))
                .OrderBy(a => a.Appointment.Start));
            EventService.AddEvent(new EventMessageViewModel
            {
                Message = $"Fetched {Appointments.Count} appointments.",
                MessageType = EventMessageType.Success
            });
        }
        catch (Exception e)
        {
            EventService.AddEvent(new EventMessageViewModel
            {
                Message = e.Message,
                MessageType = EventMessageType.Error
            });
        }
    }
}