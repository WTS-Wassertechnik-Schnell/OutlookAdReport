using OutlookAdReport.Analyzation.Services;
using ReactiveUI;
using Splat;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using OutlookAdReport.Analyzation.Models;
using System.Reactive;
using OutlookAdReport.Export.ExcelExport;
using OutlookAdReport.Data.Models;
using OutlookAdReport.Data.Services;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary>   A ViewModel for the export. </summary>
public class ExportViewModel : ReactiveObject
{
    /// <summary>   Gets the event service. </summary>
    /// <value> The event service. </value>
    public IEventService EventService { get; }

    /// <summary>   (Immutable) the export service. </summary>
    private readonly IExport _exportService;

    /// <summary>   (Immutable) the events. </summary>
    private readonly ObservableAsPropertyHelper<ObservableCollection<IBusinessEvent>> _events;

    /// <summary> Gets the day analyzer service.</summary>
    /// <value> The day analyzer service.</value>
    public IBusinessDayAnalyzerService DayAnalyzerService { get; }

    /// <summary> Gets the events.</summary>
    /// <value> The events.</value>
    public ObservableCollection<IBusinessEvent> Events => _events.Value;

    /// <summary>   Gets the 'export' command. </summary>
    /// <value> The 'export' command. </value>
    public ReactiveCommand<Unit, Unit> ExportCommand { get; }

    /// <summary>   Constructor. </summary>
    /// <param name="exportService">        The export service. </param>
    /// <param name="dayAnalyzerService">   (Optional) The day analyzer service. </param>
    /// <param name="eventService">         (Optional) The event service. </param>
    public ExportViewModel(IExport exportService, IBusinessDayAnalyzerService? dayAnalyzerService = null, IEventService? eventService = null)
    {
        EventService = eventService ?? Locator.Current.GetService<IEventService>()!;
        _exportService = exportService;
        DayAnalyzerService = dayAnalyzerService ?? Locator.Current.GetService<IBusinessDayAnalyzerService>()!;

        _events = DayAnalyzerService.BusinessEvents
            .ToObservableChangeSet()
            .ToCollection()
            .Select(x =>
                new ObservableCollection<IBusinessEvent>(
                    x.Select(y => y)))
            .ToProperty(this, x => x.Events);

        ExportCommand = ReactiveCommand.CreateFromTask(ExportAsync);
    }

    /// <summary>   Export asynchronous. </summary>
    /// <param name="ct">   A token that allows processing to be cancelled. </param>
    /// <returns>   A Task. </returns>
    public async Task ExportAsync(CancellationToken ct)
    {
        try
        {
            await Task.Factory.StartNew(() =>
            {
                _exportService.Export(Events);
            }, ct);

            EventService.AddEvent(new EventMessageModel
            {
                Message =
                    $"Exportet {DayAnalyzerService.BusinessDays.Count} days with {DayAnalyzerService.BusinessEvents.Count} events."
            });
        }
        catch (Exception e)
        {
            EventService.AddEvent(e.Message, EventMessageType.Error);
        }
    }
}