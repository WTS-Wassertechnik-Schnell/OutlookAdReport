using System.ComponentModel;
using System.Runtime.CompilerServices;
using OutlookAdReport.Data.Models;

namespace OutlookAdReport.Data.Services;

/// <summary> A service for accessing logins information.</summary>
public abstract class LoginService : ILoginService
{
    /// <summary> The login result.</summary>
    private ILoginResult? _loginResult;

    /// <summary> Occurs when a property value changes.</summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary> Gets or sets the login result.</summary>
    /// <value> The login result.</value>
    public ILoginResult? LoginResult
    {
        get => _loginResult;
        set
        {
            SetField(ref _loginResult, value);
            OnPropertyChanged(nameof(IsLoggedIn));
        }
    }

    /// <summary> Gets a value indicating whether this object is logged in.</summary>
    /// <value> True if this object is logged in, false if not.</value>
    public bool IsLoggedIn => LoginResult is { IsAuthenticated: true };

    public abstract Task<ILoginResult> LoginAsync(string user, string password);

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