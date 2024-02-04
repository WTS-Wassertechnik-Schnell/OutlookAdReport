using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Options;
using OutlookAdReport.Data;

namespace OutlookAdReport.ExchangeServer;

/// <summary> A service for accessing exchange logins information.</summary>
public class ExchangeLoginService : LoginService
{
    /// <summary> Constructor.</summary>
    /// <param name="exchangeOptions"> Options for controlling the exchange. </param>
    public ExchangeLoginService(IOptions<ExchangeOptions> exchangeOptions)
    {
        ExchangeOptions = exchangeOptions;
    }

    /// <summary> Gets options for controlling the exchange.</summary>
    /// <value> Options that control the exchange.</value>
    public IOptions<ExchangeOptions> ExchangeOptions { get; }

    /// <summary> Login asynchronous.</summary>
    /// <param name="user">     The user. </param>
    /// <param name="password"> The password. </param>
    /// <returns> The login.</returns>
    public override async Task<ILoginResult> LoginAsync(string user, string password)
    {
        var service = new ExchangeService(ExchangeOptions.Value.ServiceVersion)
        {
            Url = new Uri(ExchangeOptions.Value.ServiceUrl),
            WebProxy = null, // disable all proxies
            Credentials = new WebCredentials(user, password)
        };

#pragma warning disable CS8622
        ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
#pragma warning restore CS8622

        try
        {
            await service.FindFolders(WellKnownFolderName.Inbox, new FolderView(1));
            LoginResult = new ExchangeLoginResult(service);
        }
        catch (ServiceRequestException sre)
        {
            LoginResult = new ExchangeLoginResult(sre);
        }

        return LoginResult;
    }

    /// <summary> Back, called when the certificate validation.</summary>
    /// <param name="sender">          Source of the event. </param>
    /// <param name="certificate">     The certificate. </param>
    /// <param name="chain">           The chain. </param>
    /// <param name="sslpolicyerrors"> The ssl-policy-errors. </param>
    /// <returns> True if it succeeds, false if it fails.</returns>
    private static bool CertificateValidationCallBack(object sender, X509Certificate certificate, X509Chain chain,
        SslPolicyErrors sslpolicyerrors)
    {
        return true;
    }
}