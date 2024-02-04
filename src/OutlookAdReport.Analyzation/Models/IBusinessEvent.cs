using System.ComponentModel;

namespace OutlookAdReport.Analyzation.Models;

/// <summary> Interface for business event.</summary>
public interface IBusinessEvent : INotifyPropertyChanged
{
    /// <summary> Gets the day.</summary>
    /// <value> The day.</value>
    public IBusinessDay Day { get; }

    /// <summary> Gets or sets the Date/Time of the start.</summary>
    /// <value> The start.</value>
    public DateTime Start { get; set; }

    /// <summary> Gets or sets the Date/Time of the end.</summary>
    /// <value> The end.</value>
    public DateTime End { get; set; }

    /// <summary> Gets the duration.</summary>
    /// <value> The duration.</value>
    public TimeSpan Duration { get; }

    /// <summary> Gets or sets the customer.</summary>
    /// <value> The customer.</value>
    public string Customer { get; set; }

    /// <summary> Gets or sets the contact.</summary>
    /// <value> The contact.</value>
    public string Contact { get; set; }

    /// <summary> Gets or sets the street.</summary>
    /// <value> The street.</value>
    public string Street { get; set; }

    /// <summary> Gets or sets the zip code.</summary>
    /// <value> The zip code.</value>
    public string ZipCode { get; set; }

    /// <summary> Gets or sets the city.</summary>
    /// <value> The city.</value>
    public string City { get; set; }

    /// <summary> Gets or sets the remarks.</summary>
    /// <value> The remarks.</value>
    public string Remarks { get; set; }
}