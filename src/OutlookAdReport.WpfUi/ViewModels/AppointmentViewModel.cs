using OutlookAdReport.Data;
using ReactiveUI;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the appointment.</summary>
public class AppointmentViewModel : ReactiveObject
{
    /// <summary> Gets the appointment.</summary>
    /// <value> The appointment.</value>
    public IAppointment Appointment { get; }

    /// <summary> Constructor.</summary>
    /// <param name="appointment"> The appointment. </param>
    public AppointmentViewModel(IAppointment appointment)
    {
        Appointment = appointment;
        Header = $"{appointment.Start.ToShortDateString()}";
    }

    /// <summary> Gets the header.</summary>
    /// <value> The header.</value>
    public string Header { get; }
}