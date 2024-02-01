using System.Reflection;
using System.Windows;
using ReactiveUI;
using Splat;

namespace OutlookAdReport.WpfUi;

/// <summary> An application.</summary>
public partial class App : Application
{
    public App()
    {
        Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
    }
}