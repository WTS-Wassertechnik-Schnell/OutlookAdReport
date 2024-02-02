using OutlookAdReport.WpfUi.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace OutlookAdReport.WpfUi.Views;

/// <summary> Form for viewing the application.</summary>
public partial class AppWindow
{
    /// <summary> Default constructor.</summary>
    public AppWindow() : this(new AppViewModel(null!))
    {
        
    }

    /// <summary> Constructor.</summary>
    /// <param name="viewModel"> The view model. </param>
    public AppWindow(AppViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        SearchView.ViewModel = ViewModel.SearchViewModel;
        LoginView.ViewModel = ViewModel.LoginViewModel;

        // bindings
        this.WhenActivated(disposableRegistration =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.HasEvents,
                    view => view.OutputGroupBox.Visibility)
                .DisposeWith(disposableRegistration);

            this.OneWayBind(ViewModel, 
                vm => vm.Events,
                view => view.EventListBox.ItemsSource)
                .DisposeWith(disposableRegistration);
        });
    }
}