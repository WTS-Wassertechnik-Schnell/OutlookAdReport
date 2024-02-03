using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutlookAdReport.Data;
using OutlookAdReport.ExchangeServer;
using OutlookAdReport.WpfUi.ViewModels;
using OutlookAdReport.WpfUi.Views;
using ReactiveUI;
using Splat;

namespace OutlookAdReport.WpfUi;

/// <summary> An application.</summary>
public partial class App : Application
{
    /// <summary> Gets or sets the service provider.</summary>
    /// <value> The service provider.</value>
    public IServiceProvider ServiceProvider { get; private set; }

    /// <summary> Gets or sets the configuration.</summary>
    /// <value> The configuration.</value>
    public IConfiguration Configuration { get; private set; }

    /// <summary> Default constructor.</summary>
#pragma warning disable CS8618
    public App()
#pragma warning restore CS8618
    {
        Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
    }

    /// <summary> Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
    /// <param name="e">    A <see cref="T:System.Windows.StartupEventArgs" /> that contains the
    ///                     event data. </param>
    protected override void OnStartup(StartupEventArgs e)
    {
        var localSettingsFile = new FileInfo(@"Y:\Exchange.Extensions\ExchangeSettings.json");

        var builder = FileExists(localSettingsFile, 500)
            ? new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(localSettingsFile.FullName, optional: true, reloadOnChange: true)
            : new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        
        Configuration = builder.Build();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = ServiceProvider.GetRequiredService<AppWindow>();
        mainWindow.Show();
    }

    public static bool FileExists(FileInfo fileInfo, int millisecondsTimeout)
    {
        var task = new Task<bool>(() => fileInfo.Exists);
        task.Start();
        return task.Wait(millisecondsTimeout) && task.Result;
    }

    /// <summary> Configure services.</summary>
    /// <param name="services"> The services. </param>
    private void ConfigureServices(IServiceCollection services)
    {
        // options
        services.Configure<ExchangeOptions>(Configuration.GetSection("ExchangeServer"));

        // views
        services.AddSingleton<AppWindow>();

        // view-models
        services.AddTransient<AppViewModel>();
        
        // services
        services.AddTransient<ILoginService, ExchangeLoginService>();
        services.AddTransient<IAppointmentQueryService, ExchangeAppointmentQueryService>();
    }
}