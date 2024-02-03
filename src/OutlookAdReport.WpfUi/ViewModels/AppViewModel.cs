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

        ShowSuccess = true;
        ShowWarning = true;
        ShowError = true;
        
        _events = new ObservableCollection<EventMessageViewModel>();
        
        var observableFilter = this.WhenAnyValue(
            vm => vm.ShowSuccess,
            vm => vm.ShowWarning,
            vm => vm.ShowError)
            .Select(x => MakeFilter(x.Item1,x.Item2,x.Item3));

        _visibleEvents = Events
            .ToObservableChangeSet(x => x)
            .Filter(observableFilter)
            .ToCollection()
            .ToProperty(this, x => x.VisibleEvents);

        _hasEvents = Events
            .ToObservableChangeSet(x => x)
            .ToCollection()
            .Select(items => items.Any())
            .ToProperty(this, x => x.HasEvents);
    }

    /// <summary> Makes a filter.</summary>
    /// <param name="success"> True if the operation was a success, false if it failed. </param>
    /// <param name="warning"> True to warning. </param>
    /// <param name="error">   True to error. </param>
    /// <returns> A function delegate that yields a bool.</returns>
    private static Func<EventMessageViewModel, bool> MakeFilter(bool success, bool warning, bool error)
    {
        return e =>
            e.MessageType == EventMessageType.Success && success ||
            e.MessageType == EventMessageType.Warning && warning ||
            e.MessageType == EventMessageType.Error && error;
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

    private bool _showSuccess;

    /// <summary> Gets or sets a value indicating whether the success is shown.</summary>
    /// <value> True if show success, false if not.</value>
    public bool ShowSuccess
    {
        get => _showSuccess;
        set => this.RaiseAndSetIfChanged(ref _showSuccess, value);
    }

    private bool _showWarning;

    /// <summary> Gets or sets a value indicating whether the warning is shown.</summary>
    /// <value> True if show warning, false if not.</value>
    public bool ShowWarning
    {
        get => _showWarning;
        set => this.RaiseAndSetIfChanged(ref _showWarning, value);
    }

    private bool _showError;

    /// <summary> Gets or sets a value indicating whether the error is shown.</summary>
    /// <value> True if show error, false if not.</value>
    public bool ShowError
    {
        get => _showError;
        set => this.RaiseAndSetIfChanged(ref _showError, value);
    }

    /// <summary> The events.</summary>
    private ObservableCollection<EventMessageViewModel> _events;
    
    /// <summary> Gets or sets the events.</summary>
    /// <value> The events.</value>
    public ObservableCollection<EventMessageViewModel> Events
    {
        get => _events;
        set => this.RaiseAndSetIfChanged(ref _events, value);
    }

    private readonly ObservableAsPropertyHelper<IReadOnlyCollection<EventMessageViewModel>> _visibleEvents;

    /// <summary> Gets the visible events.</summary>
    /// <value> The visible events.</value>
    public IReadOnlyCollection<EventMessageViewModel> VisibleEvents => _visibleEvents.Value;
}