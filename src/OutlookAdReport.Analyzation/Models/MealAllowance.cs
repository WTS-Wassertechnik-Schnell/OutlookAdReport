namespace OutlookAdReport.Analyzation.Models;

/// <summary>   A meal allowance. </summary>
public class MealAllowance
{
    /// <summary>   Gets or sets the Date/Time of the date. </summary>
    /// <value> The date. </value>
    public DateTime Date { get; set; }

    /// <summary>   Gets or sets the Date/Time of the start. </summary>
    /// <value> The start. </value>
    public DateTime Start { get; set; }

    /// <summary>   Gets or sets the Date/Time of the end. </summary>
    /// <value> The end. </value>
    public DateTime End { get; set; }

    /// <summary>   Gets or sets the locations. </summary>
    /// <value> The locations. </value>
    public IEnumerable<string> Locations { get; set; }

    /// <summary>   Gets or sets the time out of office. </summary>
    /// <value> The time out of office. </value>
    public TimeSpan TimeOutOfOffice { get; set; }

    /// <summary>   Gets or sets the allowance. </summary>
    /// <value> The allowance. </value>
    public decimal Allowance { get; set; }
}