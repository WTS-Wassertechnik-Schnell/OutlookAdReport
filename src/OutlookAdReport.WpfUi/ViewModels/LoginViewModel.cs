using ReactiveUI;
using System.Reactive;
using OutlookAdReport.Data;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the login.</summary>
public class LoginViewModel : ReactiveObject
{
    /// <summary> Gets the application view model.</summary>
    /// <value> The application view model.</value>
    public AppViewModel AppViewModel { get; }

    /// <summary> (Immutable) the login service.</summary>
    private readonly ILoginService _loginService;

    /// <summary> Default constructor.</summary>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="appViewModel"> The application view model. </param>
    /// <param name="loginService"> (Immutable) the login service. </param>
    public LoginViewModel(AppViewModel appViewModel, ILoginService loginService)
    {
        _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        AppViewModel = appViewModel;
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

    private ILoginResult? _loginResult;

    /// <summary> Gets or sets the login result.</summary>
    /// <value> The login result.</value>
    public ILoginResult? LoginResult
    {
        get => _loginResult;
        private set => this.RaiseAndSetIfChanged(ref _loginResult, value);
    }

    /// <summary> Login asynchronous.</summary>
    /// <param name="ct"> A token that allows processing to be cancelled. </param>
    /// <returns> A Task.</returns>
    public async Task LoginAsync(CancellationToken ct)
    {
        LoggedIn = false;
        LoginResult = await _loginService.LoginAsync(Username, Password);
        AppViewModel.Events.Clear();
        if (!LoginResult.IsAuthenticated)
        {
            AppViewModel.Events.Add(new EventMessageViewModel
            {
                Message = LoginResult.Error!,
                MessageType = EventMessageType.Error
            });
        }
        else
        {
            AppViewModel.Events.Add(new EventMessageViewModel
            {
                Message = "User successfully logged in.",
                MessageType = EventMessageType.Success
            });
        }
        LoggedIn = LoginResult.IsAuthenticated;
    }
}