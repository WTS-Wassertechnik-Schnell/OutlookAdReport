using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OutlookAdReport.Data;
using OutlookAdReport.ExchangeServer;
using OutlookAdReport.WpfUi.Services;
using OutlookAdReport.WpfUi.Utils;
using OutlookAdReport.WpfUi.ViewModels;
using OutlookAdReport.WpfUi.Views;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace OutlookAdReport.WpfUi;

/// <summary> A bootstrapper.</summary>
public class Bootstrapper
{
    /// <summary> Initializes this object.</summary>
    /// <returns> An IServiceProvider.</returns>
    public IServiceProvider Init()
    {
        var sp = BuildHost().Services;

        sp.UseMicrosoftDependencyResolver();

        return sp;
    }

    /// <summary> Builds the host.</summary>
    /// <returns> An IHost.</returns>
    private static IHost BuildHost()
    {
        return Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(hostConfig =>
            {
                var localSettingsFile = new FileInfo(@"Y:\Exchange.Extensions\ExchangeSettings.json");
                if (localSettingsFile.FileExists(500)) hostConfig.AddJsonFile("appsettings.json", false, true);
            })
            .ConfigureServices((context, services) =>
            {
                services.UseMicrosoftDependencyResolver();
                var resolver = Locator.CurrentMutable;
                resolver.InitializeSplat();
                resolver.InitializeReactiveUI();

                ConfigureServices(context.Configuration, services);
            })
            .Build();
    }

    /// <summary> Configure services.</summary>
    /// <param name="configuration"> The configuration. </param>
    /// <param name="services">      The services. </param>
    private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // options
        services.Configure<ExchangeOptions>(configuration.GetSection("ExchangeServer"));
        services.Configure<AnalyzationOptions>(configuration.GetSection("AnalyzationOptions"));

        // views
        services.AddSingleton<AppWindow>();

        // view-models
        services.AddSingleton<AppViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<SearchViewModel>();

        // services
        services.AddSingleton<IEventService, AppViewModel>();
        services.AddTransient<ILoginService, ExchangeLoginService>();
        services.AddTransient<IAppointmentQueryService, ExchangeAppointmentQueryService>();
    }
}