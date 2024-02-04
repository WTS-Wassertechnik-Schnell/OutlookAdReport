using System.ComponentModel;
using OutlookAdReport.Data.Models;

namespace OutlookAdReport.Data.Services;

/// <summary> Interface for login service.</summary>
public interface ILoginService : INotifyPropertyChanged
{
    /// <summary> Gets or sets the login result.</summary>
    /// <value> The login result.</value>
    public ILoginResult? LoginResult { get; set; }

    /// <summary> Gets a value indicating whether this object is logged in.</summary>
    /// <value> True if this object is logged in, false if not.</value>
    public bool IsLoggedIn { get; }

    /// <summary> Login asynchronous.</summary>
    /// <param name="user">     The user. </param>
    /// <param name="password"> The password. </param>
    /// <returns> The login.</returns>
    public Task<ILoginResult> LoginAsync(string user, string password);
}