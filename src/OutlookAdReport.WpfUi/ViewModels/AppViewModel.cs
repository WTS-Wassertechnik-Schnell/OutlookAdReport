using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using OutlookAdReport.WpfUi.Services;
using ReactiveUI;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the application.</summary>
public class AppViewModel : ReactiveObject, IEventService
{
    private readonly ObservableAsPropertyHelper<bool> _hasEvents;

    private readonly ObservableAsPropertyHelper<IReadOnlyCollection<EventMessageViewModel>> _visibleEvents;

    /// <summary> The events.</summary>
    private ObservableCollection<EventMessageViewModel> _events;

    private bool _showError;

    private bool _showSuccess;

    private bool _showWarning;

    /// <summary> Default constructor.</summary>
    public AppViewModel()
    {
        ShowSuccess = true;
        ShowWarning = true;
        ShowError = true;

        _events = new ObservableCollection<EventMessageViewModel>();

        var observableFilter = this.WhenAnyValue(
                vm => vm.ShowSuccess,
                vm => vm.ShowWarning,
                vm => vm.ShowError)
            .Select(x => MakeFilter(x.Item1, x.Item2, x.Item3));

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

    /// <summary> Gets a value indicating whether this object has events.</summary>
    /// <value> True if this object has events, false if not.</value>
    public bool HasEvents => _hasEvents.Value;

    /// <summary> Gets or sets a value indicating whether the success is shown.</summary>
    /// <value> True if show success, false if not.</value>
    public bool ShowSuccess
    {
        get => _showSuccess;
        set => this.RaiseAndSetIfChanged(ref _showSuccess, value);
    }

    /// <summary> Gets or sets a value indicating whether the warning is shown.</summary>
    /// <value> True if show warning, false if not.</value>
    public bool ShowWarning
    {
        get => _showWarning;
        set => this.RaiseAndSetIfChanged(ref _showWarning, value);
    }

    /// <summary> Gets or sets a value indicating whether the error is shown.</summary>
    /// <value> True if show error, false if not.</value>
    public bool ShowError
    {
        get => _showError;
        set => this.RaiseAndSetIfChanged(ref _showError, value);
    }

    /// <summary> Gets or sets the events.</summary>
    /// <value> The events.</value>
    public ObservableCollection<EventMessageViewModel> Events
    {
        get => _events;
        set => this.RaiseAndSetIfChanged(ref _events, value);
    }

    /// <summary> Gets the visible events.</summary>
    /// <value> The visible events.</value>
    public IReadOnlyCollection<EventMessageViewModel> VisibleEvents => _visibleEvents.Value;

    /// <summary> Clears the events.</summary>
    public void ClearEvents()
    {
        Events.Clear();
    }

    /// <summary> Adds an event.</summary>
    /// <param name="messageViewModel"> The message view model. </param>
    public void AddEvent(EventMessageViewModel messageViewModel)
    {
        Events.Add(messageViewModel);
    }

    /// <summary> Makes a filter.</summary>
    /// <param name="success"> True if the operation was a success, false if it failed. </param>
    /// <param name="warning"> True to warning. </param>
    /// <param name="error">   True to error. </param>
    /// <returns> A function delegate that yields a bool.</returns>
    private static Func<EventMessageViewModel, bool> MakeFilter(bool success, bool warning, bool error)
    {
        return e =>
            (e.MessageType == EventMessageType.Success && success) ||
            (e.MessageType == EventMessageType.Warning && warning) ||
            (e.MessageType == EventMessageType.Error && error);
    }
}