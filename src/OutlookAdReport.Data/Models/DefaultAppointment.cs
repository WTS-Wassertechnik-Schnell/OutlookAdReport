using System.Text.RegularExpressions;

namespace OutlookAdReport.Data.Models;

/// <summary> A default appointment.</summary>
public class DefaultAppointment : IAppointment
{
    /// <summary> Constructor.</summary>
    /// <param name="subject">     The subject. </param>
    /// <param name="location">    The location. </param>
    /// <param name="description"> The description. </param>
    /// <param name="start">       The start Date/Time. </param>
    /// <param name="end">         The end Date/Time. </param>
    public DefaultAppointment(string subject, string location, string description, DateTime start, DateTime end)
    {
        Subject = subject;
        Location = location;
        Description = description;
        Start = start.ToLocalTime();
        End = end.ToLocalTime();

        try
        {
            if (!string.IsNullOrWhiteSpace(Location) && Location.Contains(',') &&
                Location.Length > Location.IndexOf(',') + 1)
            {
                Street = Location.Substring(0, Location.IndexOf(',')).Trim();
                Location = Location.Substring(Location.IndexOf(',') + 1).Trim();
            }

            if (!string.IsNullOrWhiteSpace(Location) && Location.Length > 7)
            {
                var match = Regex.Match(Location, @"\b\d{5}", RegexOptions.Compiled);
                if (match.Success)
                {
                    ZipCode = match.Value;
                    Location = Location.Replace(ZipCode, "").Trim();
                }
            }
        }
        catch (Exception)
        {
            // ignore
        }

        City = Location;
    }

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