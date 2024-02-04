using ReactiveUI;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the event message.</summary>
public class EventMessageViewModel : ReactiveObject
{
    private string _message = string.Empty;
    private EventMessageType _messageType;

    /// <summary> Gets or sets the type of the message.</summary>
    /// <value> The type of the message.</value>
    public EventMessageType MessageType
    {
        get => _messageType;
        set => this.RaiseAndSetIfChanged(ref _messageType, value);
    }

    /// <summary> Gets or sets the message.</summary>
    /// <value> The message.</value>
    public string Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }
}

/// <summary> Values that represent event message types.</summary>
public enum EventMessageType
{
    /// <summary> An enum constant representing the success option.</summary>
    Success = 0,

    /// <summary> An enum constant representing the warning option.</summary>
    Warning = 1,

    /// <summary> An enum constant representing the error option.</summary>
    Error = 2
}