using OutlookAdReport.Data;

namespace OutlookAdReport.WpfUi.Services;

/// <summary> A service for accessing default login results information.</summary>
public class DefaultLoginResultService : ILoginResultService
{
    private ILoginResult _loginResult = new EmptyLoginResult();

    /// <summary> Gets login result.</summary>
    /// <returns> The login result.</returns>
    public ILoginResult GetLoginResult()
    {
        return _loginResult;
    }

    /// <summary> Sets login result.</summary>
    /// <param name="loginResult"> The login result. </param>
    public void SetLoginResult(ILoginResult loginResult)
    {
        _loginResult = loginResult;
    }
}