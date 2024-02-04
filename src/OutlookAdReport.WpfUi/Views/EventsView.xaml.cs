using System.Reactive.Disposables;
using OutlookAdReport.WpfUi.ViewModels;
using ReactiveUI;
using Splat;

namespace OutlookAdReport.WpfUi.Views;

/// <summary> The events view.</summary>
public partial class EventsView
{
    /// <summary> Default constructor.</summary>
    public EventsView() : this(null)
    {
    }

    /// <summary> Default constructor.</summary>
    /// <param name="viewModel"> (Optional) The view model. </param>
    public EventsView(EventsViewModel? viewModel = null)
    {
        InitializeComponent();
        ViewModel = viewModel ?? Locator.Current.GetService<EventsViewModel>()!;

        // bindings
        this.WhenActivated(disposableRegistration =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.Events,
                    view => view.EventsGrid.ItemsSource)
                .DisposeWith(disposableRegistration);
        });
    }
}