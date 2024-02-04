using System.Reactive.Disposables;
using OutlookAdReport.WpfUi.ViewModels;
using ReactiveUI;
using Splat;

namespace OutlookAdReport.WpfUi.Views;

/// <summary> A login view.</summary>
public partial class LoginView
{
    /// <summary> Default constructor.</summary>
    public LoginView() : this(null)
    {
    }

    /// <summary> Default constructor.</summary>
    /// <param name="viewModel"> (Optional) The view model. </param>
    public LoginView(LoginViewModel? viewModel = null)
    {
        InitializeComponent();
        ViewModel = viewModel ?? Locator.Current.GetService<LoginViewModel>();

        // bindings
        this.WhenActivated(disposableRegistration =>
        {
            // bind context to viewmodel to enable password binding
            this.WhenAnyValue(x => x.ViewModel)
                .BindTo(this, x => x.DataContext)
                .DisposeWith(disposableRegistration);

            this.Bind(ViewModel,
                    vm => vm.Username,
                    view => view.UsernameTextBox.Text)
                .DisposeWith(disposableRegistration);

            this.Bind(ViewModel,
                    vm => vm.Password,
                    view => view.PasswordTextBox.Password)
                .DisposeWith(disposableRegistration);

            this.WhenAnyValue(
                    x => x.ViewModel!.Username,
                    x => x.ViewModel!.Password,
                    (user, pw) => !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pw))
                .BindTo(this, x => x.LoginButton.IsEnabled);

            this.WhenAnyObservable(x => x.ViewModel!.LoginCommand.IsExecuting)
                .BindTo(this, x => x.BusyIndicator.IsBusy);

            this.BindCommand(ViewModel,
                    vm => vm.LoginCommand,
                    view => view.LoginButton)
                .DisposeWith(disposableRegistration);
        });
    }
}