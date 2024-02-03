namespace OutlookAdReport.Data;

/// <summary> Encapsulates the result of an empty login.</summary>
public class EmptyLoginResult : ILoginResult
{
    /// <summary> Gets a value indicating whether this object is authenticated.</summary>
    /// <value> True if this object is authenticated, false if not.</value>
    public bool IsAuthenticated => false;

    /// <summary> Gets the error.</summary>
    /// <value> The error.</value>
    public string? Error => null;
}