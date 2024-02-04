using System.Reactive.Disposables;
using OutlookAdReport.WpfUi.ViewModels;
using ReactiveUI;
using Splat;

namespace OutlookAdReport.WpfUi.Views;

/// <summary> Form for viewing the application.</summary>
public partial class AppWindow
{
    /// <summary> Constructor.</summary>
    /// <param name="viewModel"> (Optional) The view model. </param>
    public AppWindow(AppViewModel? viewModel = null)
    {
        InitializeComponent();
        ViewModel = viewModel ?? Locator.Current.GetService<AppViewModel>();

        // bindings
        this.WhenActivated(disposableRegistration =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.HasEvents,
                    view => view.OutputGroupBox.Visibility)
                .DisposeWith(disposableRegistration);

            this.Bind(ViewModel,
                    vm => vm.ShowSuccess,
                    view => view.SuccessCheckbox.IsChecked)
                .DisposeWith(disposableRegistration);

            this.Bind(ViewModel,
                    vm => vm.ShowWarning,
                    view => view.WarningCheckbox.IsChecked)
                .DisposeWith(disposableRegistration);

            this.Bind(ViewModel,
                    vm => vm.ShowError,
                    view => view.ErrorCheckbox.IsChecked)
                .DisposeWith(disposableRegistration);

            this.OneWayBind(ViewModel,
                    vm => vm.VisibleEvents,
                    view => view.EventListBox.ItemsSource)
                .DisposeWith(disposableRegistration);
        });
    }
}