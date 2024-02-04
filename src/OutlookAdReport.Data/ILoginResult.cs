namespace OutlookAdReport.Data;

/// <summary> Interface for login result.</summary>
public interface ILoginResult
{
    /// <summary> Gets a value indicating whether this object is authenticated.</summary>
    /// <value> True if this object is authenticated, false if not.</value>
    public bool IsAuthenticated { get; }

    /// <summary> Gets the error.</summary>
    /// <value> The error.</value>
    public string? Error { get; }
}