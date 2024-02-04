using Microsoft.Exchange.WebServices.Data;
using OutlookAdReport.Data.Models;

namespace OutlookAdReport.ExchangeServer.Models;

/// <summary> Encapsulates the result of an exchange login.</summary>
public class ExchangeLoginResult : ILoginResult
{
    /// <summary> Constructor.</summary>
    /// <param name="service"> The service. </param>
    public ExchangeLoginResult(ExchangeService? service)
    {
        Service = service!;
        IsAuthenticated = service != null;
    }

    /// <summary> Constructor.</summary>
    /// <param name="serviceRequestException"> The service request exception. </param>
    public ExchangeLoginResult(Exception serviceRequestException)
    {
        IsAuthenticated = false;
        Error = serviceRequestException.Message;
    }

    /// <summary> Gets the service.</summary>
    /// <value> The service.</value>
    public ExchangeService Service { get; } = null!;

    /// <summary> Gets a value indicating whether this object is authenticated.</summary>
    /// <value> True if this object is authenticated, false if not.</value>
    public bool IsAuthenticated { get; }

    /// <summary> Gets the error.</summary>
    /// <value> The error.</value>
    public string? Error { get; }
}