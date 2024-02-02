namespace OutlookAdReport.Data;

/// <summary> Interface for login service.</summary>
public interface ILoginService
{
    /// <summary> Login asynchronous.</summary>
    /// <param name="user">     The user. </param>
    /// <param name="password"> The password. </param>
    /// <returns> The login.</returns>
    public Task<ILoginResult> LoginAsync(string user, string password);
}