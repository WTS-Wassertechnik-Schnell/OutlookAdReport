using OutlookAdReport.Data.Models;
using ReactiveUI;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the appointment.</summary>
public class AppointmentViewModel : ReactiveObject
{
    /// <summary> Constructor.</summary>
    /// <param name="appointment"> The appointment. </param>
    public AppointmentViewModel(IAppointment appointment)
    {
        Appointment = appointment;

        Header = $"{appointment.Start.ToShortDateString()}";

        ShortDate = appointment.Start.ToShortDateString();
        BeginTime = appointment.Start.TimeOfDay.ToString(@"hh\:mm");
        EndTime = appointment.End.TimeOfDay.ToString(@"hh\:mm");
    }

    /// <summary> Gets the appointment.</summary>
    /// <value> The appointment.</value>
    public IAppointment Appointment { get; }

    /// <summary> Gets the header.</summary>
    /// <value> The header.</value>
    public string Header { get; }

    /// <summary> Gets the short date.</summary>
    /// <value> The short date.</value>
    public string ShortDate { get; }

    /// <summary> Gets the begin time.</summary>
    /// <value> The begin time.</value>
    public string BeginTime { get; }

    /// <summary> Gets the end time.</summary>
    /// <value> The end time.</value>
    public string EndTime { get; }
}