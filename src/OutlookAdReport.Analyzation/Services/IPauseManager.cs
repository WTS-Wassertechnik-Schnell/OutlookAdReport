using Microsoft.Extensions.Options;
using OutlookAdReport.Analyzation.Models;
using OutlookAdReport.Analyzation.Options;
using OutlookAdReport.Data.Models;

namespace OutlookAdReport.Analyzation.Services;

/// <summary> Interface for pause manager.</summary>
public interface IPauseManager
{
    /// <summary> Adds a required pause.</summary>
    /// <param name="day">    The day. </param>
    /// <param name="events"> The events. </param>
    public void AddRequiredPause(IBusinessDay day, IList<IBusinessEvent> events);
}

/// <summary> Manager for default pauses.</summary>
public class DefaultPauseManager : IPauseManager
{
    /// <summary> Gets options for controlling the pause.</summary>
    /// <value> Options that control the pause.</value>
    public IList<PauseOption> PauseOptions { get; }

    /// <summary> Gets or sets options for controlling the analyzation.</summary>
    /// <value> Options that control the analyzation.</value>
    public AnalyzationOptions AnalyzationOptions { get; set; }

    /// <summary> Constructor.</summary>
    /// <param name="analyzationOptions"> Options for controlling the analyzation. </param>
    public DefaultPauseManager(IOptions<AnalyzationOptions> analyzationOptions)
    {
        AnalyzationOptions = analyzationOptions.Value;
        PauseOptions = AnalyzationOptions.PauseOptions.OrderBy(po => po.HoursAtWork).ToList();
    }

    /// <summary> Adds a required pause.</summary>
    /// <param name="day">    The day. </param>
    /// <param name="events"> The events. </param>
    public void AddRequiredPause(IBusinessDay day, IList<IBusinessEvent> events)
    {
        var requiredPause = GetRequiredPause(day);
        var start = events.First().Start;
        var end = start.Add(requiredPause);
        if (requiredPause != TimeSpan.Zero)
        {
            var pause = new BusinessEvent(day,
                new DefaultAppointment(AnalyzationOptions.PauseKey, string.Empty, string.Empty, start, end));
            events.Add(pause);
        }
    }

    /// <summary> Gets required pause.</summary>
    /// <param name="day"> The day. </param>
    /// <returns> The required pause.</returns>
    private TimeSpan GetRequiredPause(IBusinessDay day)
    {
        foreach (var option in PauseOptions)
        {
            switch (option.Operator)
            {
                case "l":
                    if (day.TimeTotal < option.HoursAtWork)
                        return option.PauseTime;
                    break;
                case "g":
                    if (day.TimeTotal >= option.HoursAtWork)
                        return option.PauseTime;
                    break;
                default:
                    throw new NotImplementedException($"PauseOperator {option.Operator} not implemented.");
            }
        }

        return TimeSpan.Zero;
    }
}