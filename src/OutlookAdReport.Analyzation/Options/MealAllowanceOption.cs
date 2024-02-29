namespace OutlookAdReport.Analyzation.Options;

/// <summary>   A meal allowance option. </summary>
public class MealAllowanceOption
{
    /// <summary> Gets or sets the operator.</summary>
    /// <value> The operator.</value>
    public string Operator { get; set; } = string.Empty;

    /// <summary>   Gets or sets the hours total. </summary>
    /// <value> The hours total. </value>
    public TimeSpan HoursTotal{ get; set; }

    /// <summary>   Gets or sets the allowance. </summary>
    /// <value> The allowance. </value>
    public decimal Allowance { get; set; }
}