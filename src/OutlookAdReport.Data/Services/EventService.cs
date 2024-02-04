using System.Collections.ObjectModel;
using OutlookAdReport.Data.Models;

namespace OutlookAdReport.Data.Services;

/// <summary> A service for accessing events information.</summary>
public class EventService : IEventService
{
    /// <summary> Gets the events.</summary>
    /// <value> The events.</value>
    public ObservableCollection<EventMessageModel> Events { get; } = new();

    /// <summary> Clears the events.</summary>
    public void ClearEvents()
    {
        Events.Clear();
    }

    /// <summary> Adds an event.</summary>
    /// <param name="messageViewModel"> The message view model. </param>
    public void AddEvent(EventMessageModel messageViewModel)
    {
        Events.Add(messageViewModel);
    }

    /// <summary> Adds an event.</summary>
    /// <param name="message">     The message. </param>
    /// <param name="messageType"> Type of the message. </param>
    public void AddEvent(string message, EventMessageType messageType)
    {
        AddEvent(new EventMessageModel
        {
            Message = message,
            MessageType = messageType
        });
    }
}