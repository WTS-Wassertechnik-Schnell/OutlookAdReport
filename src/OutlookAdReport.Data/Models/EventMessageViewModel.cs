using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OutlookAdReport.Data.Models;

/// <summary> A ViewModel for the event message.</summary>
public class EventMessageModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _message = string.Empty;

    private EventMessageType _messageType;

    /// <summary> Gets or sets the type of the message.</summary>
    /// <value> The type of the message.</value>
    public EventMessageType MessageType
    {
        get => _messageType;
        set =>SetField(ref _messageType, value);
    }

    /// <summary> Gets or sets the message.</summary>
    /// <value> The message.</value>
    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }

    /// <summary> Executes the 'property changed' action.</summary>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary> Sets a field.</summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    /// <param name="field">        [in,out] The field. </param>
    /// <param name="value">        The value. </param>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    /// <returns> True if it succeeds, false if it fails.</returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
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