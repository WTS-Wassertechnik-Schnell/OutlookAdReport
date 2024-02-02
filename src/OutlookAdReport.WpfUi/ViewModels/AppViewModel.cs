using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using OutlookAdReport.Data;
using ReactiveUI;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the application.</summary>
public class AppViewModel : ReactiveObject
{
    /// <summary> Default constructor.</summary>
    public AppViewModel(ILoginService loginService)
    {
        SearchViewModel = new SearchViewModel(this);
        LoginViewModel = new LoginViewModel(loginService, this);
        _events = new ObservableCollection<EventMessageViewModel>();

        _hasEvents = Events
            .ToObservableChangeSet(x => x)
            .ToCollection()
            .Select(items => items.Any())
            .ToProperty(this, x => x.HasEvents);
    }

    /// <summary> Gets or sets the search view model.</summary>
    /// <value> The search view model.</value>
    public SearchViewModel SearchViewModel { get; set; }

    /// <summary> Gets or sets the login view model.</summary>
    /// <value> The login view model.</value>
    public LoginViewModel LoginViewModel { get; set; }

    private readonly ObservableAsPropertyHelper<bool> _hasEvents;

    /// <summary> Gets a value indicating whether this object has events.</summary>
    /// <value> True if this object has events, false if not.</value>
    public bool HasEvents => _hasEvents.Value;

    /// <summary> The events.</summary>
    private ObservableCollection<EventMessageViewModel> _events;
    
    /// <summary> Gets or sets the events.</summary>
    /// <value> The events.</value>
    public ObservableCollection<EventMessageViewModel> Events
    {
        get => _events;
        set => this.RaiseAndSetIfChanged(ref _events, value);
    }
}