using System.Windows;
using OutlookAdReport.WpfUi.Views;
using Splat;

namespace OutlookAdReport.WpfUi;

/// <summary> An application.</summary>
public sealed partial class App
{
    /// <summary> Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
    /// <param name="e">
    ///     A <see cref="T:System.Windows.StartupEventArgs" /> that contains the
    ///     event data.
    /// </param>
    protected override void OnStartup(StartupEventArgs e)
    {
        new Bootstrapper().Init();
        Locator.Current.GetService<AppWindow>()!.Show();
    }
}