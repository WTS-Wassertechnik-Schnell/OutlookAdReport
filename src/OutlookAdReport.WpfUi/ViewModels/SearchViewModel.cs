using ReactiveUI;
using System.Reactive;

namespace OutlookAdReport.WpfUi.ViewModels;

/// <summary> A ViewModel for the search.</summary>
public class SearchViewModel : ReactiveObject
{
    /// <summary> Default constructor.</summary>
    public SearchViewModel()
    {
        var today = DateTime.Today;
        var month = new DateTime(today.Year, today.Month, 1);

        From = month.AddMonths(-1);
        Till = month.AddDays(-1);

        QueryAppointmentsCommand = ReactiveCommand.CreateFromTask(QueryAppointmentsAsync);
    }

    /// <summary> Gets the 'query appointments' command.</summary>
    /// <value> The 'query appointments' command.</value>
    public ReactiveCommand<Unit, Unit> QueryAppointmentsCommand { get; }

    private DateTime _from;

    /// <summary> Gets or sets the Date/Time of from.</summary>
    /// <value> from.</value>
    public DateTime From
    {
        get => _from;
        set => this.RaiseAndSetIfChanged(ref _from, value);
    }

    private DateTime _till;

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
        await Task.Delay(TimeSpan.FromSeconds(3), ct);
    }
}