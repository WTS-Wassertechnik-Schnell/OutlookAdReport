using System.Reactive.Disposables;
using ReactiveUI;

namespace OutlookAdReport.WpfUi.Views;

/// <summary> A search view.</summary>
public partial class SearchView
{
    /// <summary> Default constructor.</summary>
    public SearchView()
    {
        InitializeComponent();

        // bindings
        this.WhenActivated(disposableRegistration =>
        {
            this.Bind(ViewModel,
                    viewModel => viewModel.From,
                    view => view.FromDatePicker.SelectedDate)
                .DisposeWith(disposableRegistration);

            this.Bind(ViewModel,
                    viewModel => viewModel.Till,
                    view => view.TillDatePicker.SelectedDate)
                .DisposeWith(disposableRegistration);

            this.WhenAnyValue(x => x.ViewModel!.From, x => x.ViewModel!.Till,
                    (from, till) => from <= till)
                .BindTo(this, x => x.QueryAppointmentsButton.IsEnabled);

            this.WhenAnyObservable(x => x.ViewModel!.QueryAppointmentsCommand.IsExecuting)
                .BindTo(this, x => x.QueryingProgressBar.Visibility);

            this.BindCommand(ViewModel,
                viewModel => viewModel.QueryAppointmentsCommand,
                view => view.QueryAppointmentsButton)
                .DisposeWith(disposableRegistration);
        });
    }
}