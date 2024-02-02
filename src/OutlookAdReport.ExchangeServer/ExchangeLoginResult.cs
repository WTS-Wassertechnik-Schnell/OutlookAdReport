using Microsoft.Exchange.WebServices.Data;
using OutlookAdReport.Data;

namespace OutlookAdReport.ExchangeServer;

/// <summary> Encapsulates the result of an exchange login.</summary>
public class ExchangeLoginResult : ILoginResult
{
    /// <summary> Gets the service.</summary>
    /// <value> The service.</value>
    public ExchangeService Service { get; } = null!;

    /// <summary> Constructor.</summary>
    /// <param name="service"> The service. </param>
    public ExchangeLoginResult(ExchangeService? service)
    {
        Service = service!;
        IsAuthenticated = service != null;
        Errors = new List<string>().AsReadOnly();
    }

    public ExchangeLoginResult(ServiceRequestException serviceRequestException)
    {
        IsAuthenticated = false;
        Errors = new []{ serviceRequestException.Message }.AsReadOnly();
    }

    /// <summary> Gets a value indicating whether this object is authenticated.</summary>
    /// <value> True if this object is authenticated, false if not.</value>
    public bool IsAuthenticated { get; }

    /// <summary> Gets the errors.</summary>
    /// <value> The errors.</value>
    public IReadOnlyCollection<string> Errors { get; }
}