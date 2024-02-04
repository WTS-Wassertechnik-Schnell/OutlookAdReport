using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using OutlookAdReport.Data.Models;
using OutlookAdReport.Data.Services;
using ReactiveUI;
using Splat;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the application.</summary>
public class AppViewModel : ReactiveObject
{
    private readonly ObservableAsPropertyHelper<bool> _hasEvents;

    private readonly ObservableAsPropertyHelper<IReadOnlyCollection<EventMessageModel>> _visibleEvents;

    private bool _showError;

    private bool _showSuccess;

    private bool _showWarning;

    /// <summary> Default constructor.</summary>
    public AppViewModel(IEventService? eventService = null)
    {
        EventService = eventService ?? Locator.Current.GetService<IEventService>()!;
        ShowSuccess = true;
        ShowWarning = true;
        ShowError = true;

        var observableFilter = this.WhenAnyValue(
                vm => vm.ShowSuccess,
                vm => vm.ShowWarning,
                vm => vm.ShowError)
            .Select(x => MakeFilter(x.Item1, x.Item2, x.Item3));

        _visibleEvents = EventService.Events
            .ToObservableChangeSet(x => x)
            .Filter(observableFilter)
            .ToCollection()
            .ToProperty(this, x => x.VisibleEvents);

        _hasEvents = EventService.Events
            .ToObservableChangeSet(x => x)
            .ToCollection()
            .Select(items => items.Any())
            .ToProperty(this, x => x.HasEvents);
    }

    /// <summary> Gets the event service.</summary>
    /// <value> The event service.</value>
    public IEventService EventService { get; }

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

    /// <summary> Gets the visible events.</summary>
    /// <value> The visible events.</value>
    public IReadOnlyCollection<EventMessageModel> VisibleEvents => _visibleEvents.Value;

    /// <summary> Makes a filter.</summary>
    /// <param name="success"> True if the operation was a success, false if it failed. </param>
    /// <param name="warning"> True to warning. </param>
    /// <param name="error">   True to error. </param>
    /// <returns> A function delegate that yields a bool.</returns>
    private static Func<EventMessageModel, bool> MakeFilter(bool success, bool warning, bool error)
    {
        return e =>
            (e.MessageType == EventMessageType.Success && success) ||
            (e.MessageType == EventMessageType.Warning && warning) ||
            (e.MessageType == EventMessageType.Error && error);
    }
}