using OutlookAdReport.WpfUi.ViewModels;
using ReactiveUI;

namespace OutlookAdReport.WpfUi.Views;

/// <summary> Form for viewing the application.</summary>
public partial class AppWindow
{
    /// <summary> Default constructor.</summary>
    public AppWindow()
    {
        InitializeComponent();
        ViewModel = new AppViewModel();
        SearchView.ViewModel = ViewModel.SearchViewModel;

        // bindings
        this.WhenActivated(disposableRegistration =>
        {

        });
    }
}