using System.Reactive.Disposables;
using OutlookAdReport.WpfUi.ViewModels;
using ReactiveUI;
using Splat;

namespace OutlookAdReport.WpfUi.Views;

/// <summary> A login view.</summary>
public partial class ExportView
{
    /// <summary> Default constructor.</summary>
    public ExportView() : this(null)
    {
    }

    /// <summary> Default constructor.</summary>
    /// <param name="viewModel"> (Optional) The view model. </param>
    public ExportView(ExportViewModel? viewModel = null)
    {
        InitializeComponent();
        ViewModel = viewModel ?? Locator.Current.GetService<ExportViewModel>();

        // bindings
        this.WhenActivated(disposableRegistration =>
        {
            this.WhenAnyValue(
                    x => x.ViewModel!.Events,
                    events => events != null && events.Any() && events.Count > 0)
                .BindTo(this, x => x.ExportButton.IsEnabled);

            this.WhenAnyObservable(x => x.ViewModel!.ExportCommand.IsExecuting)
                .BindTo(this, x => x.BusyIndicator.IsBusy);

            this.BindCommand(ViewModel,
                    vm => vm.ExportCommand,
                    view => view.ExportButton)
                .DisposeWith(disposableRegistration);
        });
    }
}