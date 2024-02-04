using System.Reactive;
using OutlookAdReport.Data;
using OutlookAdReport.WpfUi.Services;
using ReactiveUI;
using Splat;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the login.</summary>
public class LoginViewModel : ReactiveObject
{
    private string _password = string.Empty;

    private string _username = Environment.UserName;

    /// <summary> Default constructor.</summary>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when one or more required arguments are
    ///     null.
    /// </exception>
    /// <param name="loginService"> (Optional) (Immutable) the login service. </param>
    /// <param name="eventService"> (Optional) The event service. </param>
    public LoginViewModel(ILoginService? loginService = null, IEventService? eventService = null)
    {
        LoginService = loginService ?? Locator.Current.GetService<ILoginService>()!;
        EventService = eventService ?? Locator.Current.GetService<IEventService>()!;
        LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
    }

    /// <summary> Gets the login service.</summary>
    /// <value> The login service.</value>
    public ILoginService LoginService { get; }

    /// <summary> Gets the event service.</summary>
    /// <value> The event service.</value>
    public IEventService EventService { get; }

    /// <summary> Gets the 'login' command.</summary>
    /// <value> The 'login' command.</value>
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }

    /// <summary> Gets or sets the username.</summary>
    /// <value> The username.</value>
    public string Username
    {
        get => _username;
        set => this.RaiseAndSetIfChanged(ref _username, value);
    }

    /// <summary> Gets or sets the password.</summary>
    /// <value> The password.</value>
    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    /// <summary> Login asynchronous.</summary>
    /// <param name="ct"> A token that allows processing to be cancelled. </param>
    /// <returns> A Task.</returns>
    public async Task LoginAsync(CancellationToken ct)
    {
        var result = await LoginService.LoginAsync(Username, Password);
        EventService.ClearEvents();
        if (!result.IsAuthenticated)
            EventService.AddEvent(new EventMessageViewModel
            {
                Message = result.Error!,
                MessageType = EventMessageType.Error
            });
        else
            EventService.AddEvent(new EventMessageViewModel
            {
                Message = "User successfully logged in.",
                MessageType = EventMessageType.Success
            });
    }
}