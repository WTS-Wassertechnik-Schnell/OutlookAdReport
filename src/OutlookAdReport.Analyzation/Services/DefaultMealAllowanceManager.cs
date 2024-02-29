using Microsoft.Extensions.Options;
using OutlookAdReport.Analyzation.Models;
using OutlookAdReport.Analyzation.Options;

namespace OutlookAdReport.Analyzation.Services;

/// <summary>   Manager for default meal allowances. </summary>
public class DefaultMealAllowanceManager : IMealAllowanceManager
{
    /// <summary> Gets or sets options for controlling the analyzation.</summary>
    /// <value> Options that control the analyzation.</value>
    public AnalyzationOptions AnalyzationOptions { get; set; }

    /// <summary>   Gets the meal allowances. </summary>
    /// <value> The meal allowances. </value>
    public IList<MealAllowanceOption> MealAllowances { get; }

    /// <summary>   Constructor. </summary>
    /// <param name="analyzationOptions">   Options that control the analyzation. </param>
    public DefaultMealAllowanceManager(IOptions<AnalyzationOptions> analyzationOptions)
    {
        AnalyzationOptions = analyzationOptions.Value;
        MealAllowances = AnalyzationOptions.MealAllowanceOptions.OrderBy(po => po.HoursTotal).ToList();
    }

    /// <summary>   Enumerates compute allowances in this collection. </summary>
    /// <param name="events">   The events. </param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process compute allowances in this
    ///     collection.
    /// </returns>
    public IEnumerable<MealAllowance> ComputeAllowances(IEnumerable<IBusinessEvent> events)
    {
        var days = events.Select(e => e.Day).Distinct().ToList();
        foreach (var day in days)
        {
            if (day.TimeTotal == TimeSpan.Zero && AnalyzationOptions.DisplayMealAllowanceOnEmtpty)
            {
                yield return new MealAllowance
                {
                    Allowance = 0,
                    Date = day.Events.First().Start.Date,
                    Start = day.Events.OrderBy(e => e.Start).First().Start,
                    End = day.Events.OrderByDescending(e => e.End).First().End,
                    TimeOutOfOffice = TimeSpan.Zero,
                    Locations = new List<string>()
                };
            }

            var allowance = GetAllowance(day);
            if (allowance is { Allowance: > 0 } || allowance is { Allowance: < 1 } && AnalyzationOptions.DisplayMealAllowanceOnInsufficient)
                yield return allowance;
        }
    }

    /// <summary>   Gets an allowance. </summary>
    /// <param name="day">  The day. </param>
    /// <returns>   The allowance. </returns>
    private MealAllowance? GetAllowance(IBusinessDay day)
    {
        foreach (var option in MealAllowances)
        {
            switch (option.Operator)
            {
                case "l":
                    if (day.TimeTotal < option.HoursTotal)
                        return new MealAllowance
                        {
                            Allowance = option.Allowance, 
                            Date = day.Events.First().Start.Date,
                            Start = day.Events.OrderBy(e => e.Start).First().Start,
                            End = day.Events.OrderByDescending(e => e.End).First().End,
                            TimeOutOfOffice = day.TimeOffice >= AnalyzationOptions.FullOfficeDay 
                                ? day.TimeTotal.Subtract(day.TimeOffice)
                                : day.TimeTotal,
                            Locations = day.Events
                                .Where(e => !string.IsNullOrWhiteSpace(e.City))
                                .Select(e => e.City.Replace(",", "")
                                    .Trim()).Where(c => !string.IsNullOrWhiteSpace(c))
                        };
                    break;
                case "g":
                    if (day.TimeTotal >= option.HoursTotal)
                        return new MealAllowance
                        {
                            Allowance = option.Allowance,
                            Date = day.Events.First().Start.Date,
                            Start = day.Events.OrderBy(e => e.Start).First().Start,
                            End = day.Events.OrderByDescending(e => e.End).First().End,
                            TimeOutOfOffice = day.TimeOffice >= AnalyzationOptions.FullOfficeDay
                                ? day.TimeTotal.Subtract(day.TimeOffice)
                                : day.TimeTotal,
                            Locations = day.Events
                                .Where(e => !string.IsNullOrWhiteSpace(e.City))
                                .Select(e => e.City.Replace(",", "")
                                .Trim()).Where(c => !string.IsNullOrWhiteSpace(c))
                        };
                    break;
                default:
                    throw new NotImplementedException($"MealAllowanceOperator {option.Operator} not implemented.");
            }
        }

        return null;
    }
}