using ReactiveUI;
using System.Reactive;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the login.</summary>
public class LoginViewModel : ReactiveObject
{
    /// <summary> Default constructor.</summary>
    public LoginViewModel()
    {
        LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
    }

    /// <summary> Gets the 'login' command.</summary>
    /// <value> The 'login' command.</value>
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }

    private string _username = Environment.UserName;

    /// <summary> Gets or sets the username.</summary>
    /// <value> The username.</value>
    public string Username
    {
        get => _username;
        set => this.RaiseAndSetIfChanged(ref _username, value);
    }

    private string _password = string.Empty;

    /// <summary> Gets or sets the password.</summary>
    /// <value> The password.</value>
    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    private bool _loggedIn;

    /// <summary> Gets or sets a value indicating whether the logged in.</summary>
    /// <value> True if logged in, false if not.</value>
    public bool LoggedIn
    {
        get => _loggedIn;
        set => this.RaiseAndSetIfChanged(ref _loggedIn, value);
    }

    /// <summary> Login asynchronous.</summary>
    /// <param name="ct"> A token that allows processing to be cancelled. </param>
    /// <returns> A Task.</returns>
    public async Task LoginAsync(CancellationToken ct)
    {
        await Task.Delay(TimeSpan.FromSeconds(3), ct);
        LoggedIn = true;
    }
}