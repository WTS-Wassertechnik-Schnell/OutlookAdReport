using Microsoft.Exchange.WebServices.Data;

namespace OutlookAdReport.ExchangeServer;

/// <summary> An exchange options.</summary>
public class ExchangeOptions
{
    /// <summary> Gets or sets URL of the service.</summary>
    /// <value> The service URL.</value>
    public string ServiceUrl { get; set; }

    /// <summary> Gets or sets the service version.</summary>
    /// <value> The service version.</value>
    public ExchangeVersion ServiceVersion { get; set; } = ExchangeVersion.Exchange2016;

    /// <summary> Gets or sets a value indicating whether the respect privacy.</summary>
    /// <value> True if respect privacy, false if not.</value>
    /// <remarks>
    /// Doesn't fetch any appointments with a privacy marker.
    /// </remarks>
    public bool RespectPrivacy { get; set; } = true;
}