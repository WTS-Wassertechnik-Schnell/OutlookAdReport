using System.Reactive.Disposables;
using ReactiveUI;

namespace OutlookAdReport.WpfUi.Views;

/// <summary> A login view.</summary>
public partial class LoginView
{
    /// <summary> Default constructor.</summary>
    public LoginView()
    {
        InitializeComponent();

        // bindings
        this.WhenActivated(disposableRegistration =>
        {
            // bind context to viewmodel to enable password binding
            this.WhenAnyValue(x => x.ViewModel)
                .BindTo(this, x => x.DataContext)
                .DisposeWith(disposableRegistration);

            this.Bind(ViewModel,
                    viewModel => viewModel.Username,
                    view => view.UsernameTextBox.Text)
                .DisposeWith(disposableRegistration);

            this.Bind(ViewModel,
                    viewModel => viewModel.Password,
                    view => view.PasswordTextBox.Password)
                .DisposeWith(disposableRegistration);

            this.WhenAnyValue(
                    x => x.ViewModel!.Username,
                    x => x.ViewModel!.Password,
                    (user, pw) => !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pw))
                .BindTo(this, x => x.LoginButton.IsEnabled);

            this.WhenAnyObservable(x => x.ViewModel!.LoginCommand.IsExecuting)
                .BindTo(this, x => x.LoginProgressBar.Visibility);

            this.BindCommand(ViewModel,
                    viewModel => viewModel.LoginCommand,
                    view => view.LoginButton)
                .DisposeWith(disposableRegistration);
        });
    }
}