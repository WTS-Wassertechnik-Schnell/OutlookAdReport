using System.Collections.ObjectModel;
using OutlookAdReport.WpfUi.ViewModels;

namespace OutlookAdReport.WpfUi.Services;

/// <summary> Interface for event service.</summary>
public interface IEventService
{
    /// <summary> Gets the events.</summary>
    /// <value> The events.</value>
    public ObservableCollection<EventMessageViewModel> Events { get; }

    /// <summary> Clears the events.</summary>
    public void ClearEvents();

    /// <summary> Adds an event.</summary>
    /// <param name="messageViewModel"> The message view model. </param>
    public void AddEvent(EventMessageViewModel messageViewModel);

    /// <summary> Adds an event.</summary>
    /// <param name="message">     The message. </param>
    /// <param name="messageType"> Type of the message. </param>
    public void AddEvent(string message, EventMessageType messageType);
}