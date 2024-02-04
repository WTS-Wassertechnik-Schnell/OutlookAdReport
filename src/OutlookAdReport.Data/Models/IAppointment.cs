namespace OutlookAdReport.Data.Models;

/// <summary> Interface for appointment.</summary>
public interface IAppointment
{
    /// <summary> Gets or sets the subject.</summary>
    /// <value> The subject.</value>
    public string Subject { get; set; }

    /// <summary> Gets or sets the location.</summary>
    /// <value> The location.</value>
    public string Location { get; set; }

    /// <summary> Gets or sets the street.</summary>
    /// <value> The street.</value>
    public string? Street { get; set; }

    /// <summary> Gets or sets the zip code.</summary>
    /// <value> The zip code.</value>
    public string? ZipCode { get; set; }

    /// <summary> Gets or sets the city.</summary>
    /// <value> The city.</value>
    public string? City { get; set; }

    /// <summary> Gets or sets the description.</summary>
    /// <value> The description.</value>
    public string Description { get; set; }

    /// <summary> Gets or sets the Date/Time of the start.</summary>
    /// <value> The start.</value>
    public DateTime Start { get; set; }

    /// <summary> Gets or sets the Date/Time of the end.</summary>
    /// <value> The end.</value>
    public DateTime End { get; set; }
}