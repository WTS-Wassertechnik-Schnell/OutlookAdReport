using DynamicData.Binding;
using OutlookAdReport.Analyzation.Services;
using DynamicData;
using OutlookAdReport.Data.Services;
using ReactiveUI;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using Splat;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the events.</summary>
public class EventsViewModel : ReactiveObject
{
    private readonly ObservableAsPropertyHelper<ObservableCollection<EventViewModel>> _events;

    /// <summary> Gets the day analyzer service.</summary>
    /// <value> The day analyzer service.</value>
    public IBusinessDayAnalyzerService DayAnalyzerService { get; }

    /// <summary> Gets the query service.</summary>
    /// <value> The query service.</value>
    public IAppointmentQueryService QueryService { get; }
    
    /// <summary> Gets the events.</summary>
    /// <value> The events.</value>
    public ObservableCollection<EventViewModel> Events => _events.Value;

    /// <summary> Constructor.</summary>
    /// <param name="dayAnalyzerService"> (Optional) The day analyzer service. </param>
    /// <param name="queryService">       (Optional) The query service. </param>
    public EventsViewModel(IBusinessDayAnalyzerService? dayAnalyzerService = null, IAppointmentQueryService? queryService = null)
    {
        DayAnalyzerService = dayAnalyzerService ?? Locator.Current.GetService<IBusinessDayAnalyzerService>()!;
        QueryService = queryService ?? Locator.Current.GetService<IAppointmentQueryService>()!;

        _events = DayAnalyzerService.BusinessEvents
            .ToObservableChangeSet()
            .ToCollection()
            .Select(x =>
                new ObservableCollection<EventViewModel>(
                    x.Select(y => new EventViewModel(y))))
            .ToProperty(this, x => x.Events);
    }
}