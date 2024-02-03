using OutlookAdReport.WpfUi.ViewModels;

namespace OutlookAdReport.WpfUi.Services;

/// <summary> Interface for event service.</summary>
public interface IEventService
{
    /// <summary> Clears the events.</summary>
    public void ClearEvents();
    
    /// <summary> Adds an event.</summary>
    /// <param name="messageViewModel"> The message view model. </param>
    public void AddEvent(EventMessageViewModel messageViewModel);
}