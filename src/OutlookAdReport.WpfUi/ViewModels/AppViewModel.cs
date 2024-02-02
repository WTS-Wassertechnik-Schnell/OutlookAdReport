using ReactiveUI;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the application.</summary>
public class AppViewModel : ReactiveObject
{
    /// <summary> Default constructor.</summary>
    public AppViewModel()
    {
        SearchViewModel = new SearchViewModel(this);
        LoginViewModel = new LoginViewModel();
    }

    /// <summary> Gets or sets the search view model.</summary>
    /// <value> The search view model.</value>
    public SearchViewModel SearchViewModel { get; set; }

    /// <summary> Gets or sets the login view model.</summary>
    /// <value> The login view model.</value>
    public LoginViewModel LoginViewModel { get; set; }
}