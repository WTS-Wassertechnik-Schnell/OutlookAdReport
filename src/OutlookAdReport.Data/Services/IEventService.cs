using System.Collections.ObjectModel;
using OutlookAdReport.Data.Models;

namespace OutlookAdReport.Data.Services;

/// <summary> Interface for event service.</summary>
public interface IEventService
{
    /// <summary> Gets the events.</summary>
    /// <value> The events.</value>
    public ObservableCollection<EventMessageModel> Events { get; }

    /// <summary> Clears the events.</summary>
    public void ClearEvents();

    /// <summary> Adds an event.</summary>
    /// <param name="messageViewModel"> The message view model. </param>
    public void AddEvent(EventMessageModel messageViewModel);

    /// <summary> Adds an event.</summary>
    /// <param name="message">     The message. </param>
    /// <param name="messageType"> Type of the message. </param>
    public void AddEvent(string message, EventMessageType messageType);
}