using System.Reactive.Disposables;
using OutlookAdReport.WpfUi.ViewModels;
using ReactiveUI;
using Splat;

namespace OutlookAdReport.WpfUi.Views;

/// <summary> A search view.</summary>
public partial class SearchView
{
    /// <summary> Default constructor.</summary>
    public SearchView() : this(null)
    {
    }

    /// <summary> Default constructor.</summary>
    /// <param name="viewModel"> (Optional) The view model. </param>
    public SearchView(SearchViewModel? viewModel = null)
    {
        InitializeComponent();
        ViewModel = viewModel ?? Locator.Current.GetService<SearchViewModel>();

        // bindings
        this.WhenActivated(disposableRegistration =>
        {
            this.Bind(ViewModel,
                    vm => vm.From,
                    view => view.FromDatePicker.SelectedDate)
                .DisposeWith(disposableRegistration);

            this.Bind(ViewModel,
                    vm => vm.Till,
                    view => view.TillDatePicker.SelectedDate)
                .DisposeWith(disposableRegistration);

            this.WhenAnyValue(
                    x => x.ViewModel!.From,
                    x => x.ViewModel!.Till,
                    x => x.ViewModel!.LoginService.IsLoggedIn,
                    (from, till, loggedIn) => from <= till && loggedIn)
                .BindTo(this, x => x.QueryAppointmentsButton.IsEnabled);

            this.WhenAnyObservable(x => x.ViewModel!.QueryAppointmentsCommand.IsExecuting)
                .BindTo(this, x => x.BusyIndicator.IsBusy);

            this.BindCommand(ViewModel,
                    vm => vm.QueryAppointmentsCommand,
                    view => view.QueryAppointmentsButton)
                .DisposeWith(disposableRegistration);
        });
    }
}