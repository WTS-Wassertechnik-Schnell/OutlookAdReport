using ReactiveUI;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the application.</summary>
public class AppViewModel : ReactiveObject
{
    /// <summary> Gets or sets the search view model.</summary>
    /// <value> The search view model.</value>
    public SearchViewModel SearchViewModel { get; set; } = new();
}