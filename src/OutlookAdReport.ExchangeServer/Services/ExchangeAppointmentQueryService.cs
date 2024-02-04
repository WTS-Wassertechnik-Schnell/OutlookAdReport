using System.Collections.ObjectModel;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Options;
using OutlookAdReport.Data.Models;
using OutlookAdReport.Data.Services;
using OutlookAdReport.ExchangeServer.Models;
using OutlookAdReport.ExchangeServer.Options;

namespace OutlookAdReport.ExchangeServer.Services;

/// <summary> A service for accessing exchange appointment queries information.</summary>
public class ExchangeAppointmentQueryService : IAppointmentQueryService
{
    /// <summary> Constructor.</summary>
    /// <param name="options"> The options. </param>
    public ExchangeAppointmentQueryService(IOptions<ExchangeOptions> options)
    {
        Options = options;
    }

    /// <summary> Gets options for controlling the operation.</summary>
    /// <value> The options.</value>
    public IOptions<ExchangeOptions> Options { get; }

    /// <summary> Gets the appointments.</summary>
    /// <value> The appointments.</value>
    public ObservableCollection<IAppointment> Appointments { get; } = new();

    /// <summary> Queries the appointments.</summary>
    /// <param name="loginResult"> The login result. </param>
    /// <param name="start">       The start Date/Time. </param>
    /// <param name="end">         The end Date/Time. </param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process query appointments in this
    ///     collection.
    /// </returns>
    public async Task<IEnumerable<IAppointment>> QueryAppointments(ILoginResult loginResult, DateTime start,
        DateTime end)
    {
        Appointments.Clear();
        if (loginResult is not ExchangeLoginResult exchange) throw new ArgumentException("Invalod LoginResult.");

        return await QueryAppointmentsInternal(exchange.Service, start, end);
    }

    /// <summary> Queries appointments internal.</summary>
    /// <param name="service"> The service. </param>
    /// <param name="start">   The start Date/Time. </param>
    /// <param name="end">     The end Date/Time. </param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process query appointments internal
    ///     in this collection.
    /// </returns>
    private async Task<IEnumerable<IAppointment>> QueryAppointmentsInternal(ExchangeService service, DateTime start,
        DateTime end)
    {
        var calendarFolder = await CalendarFolder.Bind(service, WellKnownFolderName.Calendar);
        var view = new CalendarView(start, end.AddDays(1).AddTicks(-1));
        var exchangeAppointments = await service.FindAppointments(calendarFolder.Id, view);

        var set = new PropertySet
        {
            AppointmentSchema.Start,
            AppointmentSchema.TimeZone,
            AppointmentSchema.StartTimeZone,
            AppointmentSchema.End,
            ItemSchema.Body,
            ItemSchema.TextBody,
            ItemSchema.Subject,
            AppointmentSchema.Location,
            ItemSchema.Categories,
            AppointmentSchema.IsRecurring,
            ItemSchema.Sensitivity
        };

        await service.LoadPropertiesForItems(exchangeAppointments, set);

        var appointments = exchangeAppointments
            .Where(a => Options.Value.RespectPrivacy == false || a.Sensitivity != Sensitivity.Private)
            .Select(a =>
                new DefaultAppointment(a.Subject, a.Location, a.TextBody, a.Start, a.End))
            .OrderBy(a => a.Start)
            .ToList();

        foreach (var a in appointments) Appointments.Add(a);

        return appointments;
    }
}