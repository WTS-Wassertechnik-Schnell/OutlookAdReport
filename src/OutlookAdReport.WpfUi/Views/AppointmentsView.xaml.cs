using System.Reactive.Disposables;
using ReactiveUI;

namespace OutlookAdReport.WpfUi.Views;

/// <summary> The appointments view.</summary>
public partial class AppointmentsView
{
    /// <summary> Default constructor.</summary>
    public AppointmentsView()
    {
        InitializeComponent();
        
        // bindings
        this.WhenActivated(disposableRegistration =>
        {
            this.WhenAnyValue(
                    x => x.ViewModel!.Appointments,
                    appointments => !appointments.Any() ? "(Keine Termine)" : $" ({appointments.Count})")
                .BindTo(this, x => x.CountAppointmentsTextBlock.Text)
                .DisposeWith(disposableRegistration);

            this.OneWayBind(ViewModel,
                vm => vm.Appointments,
                view => view.AppointmentsGrid.ItemsSource)
                .DisposeWith(disposableRegistration);
        });
    }
}