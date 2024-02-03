using OutlookAdReport.Data;

namespace OutlookAdReport.WpfUi.Services;

/// <summary> Interface for login result service.</summary>
public interface ILoginResultService
{
    /// <summary> Gets login result.</summary>
    /// <returns> The login result.</returns>
    public ILoginResult GetLoginResult();

    /// <summary> Sets login result.</summary>
    /// <param name="loginResult"> The login result. </param>
    public void SetLoginResult(ILoginResult loginResult);
}