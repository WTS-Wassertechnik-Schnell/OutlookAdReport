using System.Reactive.Disposables;
using OutlookAdReport.WpfUi.ViewModels;
using ReactiveUI;
using Splat;

namespace OutlookAdReport.WpfUi.Views;

/// <summary> The appointments view.</summary>
public partial class AppointmentsView
{
    /// <summary> Default constructor.</summary>
    public AppointmentsView() : this(null)
    {
    }

    /// <summary> Default constructor.</summary>
    /// <param name="viewModel"> (Optional) The view model. </param>
    public AppointmentsView(AppointmentsViewModel? viewModel = null)
    {
        InitializeComponent();
        ViewModel = viewModel ?? Locator.Current.GetService<AppointmentsViewModel>();

        // bindings
        this.WhenActivated(disposableRegistration =>
        {
            this.WhenAnyValue(
                    x => x.ViewModel!.QueryService.Appointments,
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