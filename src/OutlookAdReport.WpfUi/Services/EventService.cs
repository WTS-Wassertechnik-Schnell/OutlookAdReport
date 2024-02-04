using System.Collections.ObjectModel;
using OutlookAdReport.WpfUi.ViewModels;

namespace OutlookAdReport.WpfUi.Services;

/// <summary> A service for accessing events information.</summary>
public class EventService : IEventService
{
    /// <summary> Gets the events.</summary>
    /// <value> The events.</value>
    public ObservableCollection<EventMessageViewModel> Events { get; } = new();

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

    /// <summary> Adds an event.</summary>
    /// <param name="message">     The message. </param>
    /// <param name="messageType"> Type of the message. </param>
    public void AddEvent(string message, EventMessageType messageType)
    {
        AddEvent(new EventMessageViewModel
        {
            Message = message,
            MessageType = messageType
        });
    }
}