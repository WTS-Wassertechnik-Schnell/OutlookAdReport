﻿using System.Reactive;
using OutlookAdReport.Analyzation.Services;
using OutlookAdReport.Data.Models;
using OutlookAdReport.Data.Services;
using ReactiveUI;
using Splat;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the search.</summary>
public class SearchViewModel : ReactiveObject
{
    private DateTime _from;

    private DateTime _till;

    /// <summary> Default constructor.</summary>
    /// <param name="queryService">       (Optional) The query service. </param>
    /// <param name="eventService">       (Optional) The event service. </param>
    /// <param name="loginService">       (Optional) The login service. </param>
    /// <param name="dayAnalyzerService"> (Optional) The day analyzer service. </param>
    public SearchViewModel(IAppointmentQueryService? queryService = null, IEventService? eventService = null,
        ILoginService? loginService = null, IBusinessDayAnalyzerService? dayAnalyzerService = null)
    {
        QueryService = queryService ?? Locator.Current.GetService<IAppointmentQueryService>()!;
        EventService = eventService ?? Locator.Current.GetService<IEventService>()!;
        LoginService = loginService ?? Locator.Current.GetService<ILoginService>()!;
        DayAnalyzerService = dayAnalyzerService ?? Locator.Current.GetService<IBusinessDayAnalyzerService>()!;

        var today = DateTime.Today;
        var month = new DateTime(today.Year, today.Month, 1);

        From = month.AddMonths(-1);
        Till = month.AddDays(-1);

        QueryAppointmentsCommand = ReactiveCommand.CreateFromTask(QueryAppointmentsAsync);
    }

    /// <summary> Gets the query service.</summary>
    /// <value> The query service.</value>
    public IAppointmentQueryService QueryService { get; }

    /// <summary> Gets the event service.</summary>
    /// <value> The event service.</value>
    public IEventService EventService { get; }

    /// <summary> Gets the login service.</summary>
    /// <value> The login service.</value>
    public ILoginService LoginService { get; }

    /// <summary> Gets the day analyzer service.</summary>
    /// <value> The day analyzer service.</value>
    public IBusinessDayAnalyzerService DayAnalyzerService { get; }

    /// <summary> Gets the 'query appointments' command.</summary>
    /// <value> The 'query appointments' command.</value>
    public ReactiveCommand<Unit, Unit> QueryAppointmentsCommand { get; }

    /// <summary> Gets or sets the Date/Time of from.</summary>
    /// <value> from.</value>
    public DateTime From
    {
        get => _from;
        set => this.RaiseAndSetIfChanged(ref _from, value);
    }

    /// <summary> Gets or sets the Date/Time of the till.</summary>
    /// <value> The till.</value>
    public DateTime Till
    {
        get => _till;
        set => this.RaiseAndSetIfChanged(ref _till, value);
    }

    /// <summary> Queries appointments asynchronous.</summary>
    /// <param name="ct"> A token that allows processing to be cancelled. </param>
    /// <returns> A Task.</returns>
    public async Task QueryAppointmentsAsync(CancellationToken ct)
    {
        await QueryAppointmentsAsyncInternal();
        await AnalyzeAppointmentsInternal();
    }

    private async Task QueryAppointmentsAsyncInternal()
    {
        try
        {
            await QueryService.QueryAppointments(LoginService.LoginResult!, From, Till);
            EventService.AddEvent(new EventMessageModel
            {
                Message = $"Fetched {QueryService.Appointments.Count} appointments.",
                MessageType = EventMessageType.Success
            });
        }
        catch (Exception e)
        {
            EventService.AddEvent(e.Message, EventMessageType.Error);
        }
    }

    /// <summary> Analyze appointments internal.</summary>
    /// <returns> A Task.</returns>
    private Task AnalyzeAppointmentsInternal()
    {
        try
        {
            DayAnalyzerService.Analyze(QueryService.Appointments);
            EventService.AddEvent(new EventMessageModel
            {
                Message = $"Analyzed {DayAnalyzerService.BusinessDays.Count} days with {DayAnalyzerService.BusinessEvents.Count} events."
            });
        }
        catch (Exception e)
        {
            EventService.AddEvent(e.Message, EventMessageType.Error);
        }

        return Task.CompletedTask;
    }
}