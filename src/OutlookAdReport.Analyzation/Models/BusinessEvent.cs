using System.ComponentModel;
using System.Runtime.CompilerServices;
using OutlookAdReport.Data.Models;

namespace OutlookAdReport.Analyzation.Models;

/// <summary> The business event.</summary>
public class BusinessEvent : IBusinessEvent
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private DateTime _start;
    private DateTime _end;
    private string _customer = string.Empty;
    private string _contact = string.Empty;
    private string _street = string.Empty;
    private string _zipCode = string.Empty;
    private string _city = string.Empty;
    private string _remarks = string.Empty;

    /// <summary> Gets the day.</summary>
    /// <value> The day.</value>
    public IBusinessDay Day { get; }

    /// <summary> Gets or sets the Date/Time of the start.</summary>
    /// <value> The start.</value>
    public DateTime Start
    {
        get => _start;
        set
        {
            SetField(ref _start, value);
            OnPropertyChanged(nameof(Duration));
        }
    }

    /// <summary> Gets or sets the Date/Time of the end.</summary>
    /// <value> The end.</value>
    public DateTime End
    {
        get => _end;
        set
        {
            SetField(ref _end, value);
            OnPropertyChanged(nameof(Duration));
        }
    }

    /// <summary> Gets the duration.</summary>
    /// <value> The duration.</value>
    public TimeSpan Duration => End - Start;

    /// <summary> Gets or sets the customer.</summary>
    /// <value> The customer.</value>
    public string Customer
    {
        get => _customer;
        set => SetField(ref _customer, value);
    }

    /// <summary> Gets or sets the contact.</summary>
    /// <value> The contact.</value>
    public string Contact
    {
        get => _contact;
        set => SetField(ref _contact, value);
    }

    /// <summary> Gets or sets the street.</summary>
    /// <value> The street.</value>
    public string Street
    {
        get => _street;
        set => SetField(ref _street, value);
    }

    /// <summary> Gets or sets the zip code.</summary>
    /// <value> The zip code.</value>
    public string ZipCode
    {
        get => _zipCode;
        set => SetField(ref _zipCode, value);
    }

    /// <summary> Gets or sets the city.</summary>
    /// <value> The city.</value>
    public string City
    {
        get => _city;
        set => SetField(ref _city, value);
    }

    /// <summary> Gets or sets the remarks.</summary>
    /// <value> The remarks.</value>
    public string Remarks
    {
        get => _remarks;
        set => SetField(ref _remarks, value);
    }

    /// <summary> Gets or sets a value indicating whether this object is departure.</summary>
    /// <value> True if this object is departure, false if not.</value>
    public bool IsDeparture { get; set; }

    /// <summary> Gets or sets a value indicating whether this object is arrival.</summary>
    /// <value> True if this object is arrival, false if not.</value>
    public bool IsArrival { get; set; }

    /// <summary> Gets or sets a value indicating whether this object is office.</summary>
    /// <value> True if this object is office, false if not.</value>
    public bool IsOffice { get; set; }

    /// <summary> Gets or sets a value indicating whether this object is pause.</summary>
    /// <value> True if this object is pause, false if not.</value>
    public bool IsPause { get; set; }

    /// <summary> Gets or sets a value indicating whether this object is vacation.</summary>
    /// <value> True if this object is vacation, false if not.</value>
    public bool IsVacation { get; set; }

    /// <summary> Gets or sets a value indicating whether this object is sick.</summary>
    /// <value> True if this object is sick, false if not.</value>
    public bool IsSick { get; set; }

    /// <summary> Gets or sets a value indicating whether this object is celebration.</summary>
    /// <value> True if this object is celebration, false if not.</value>
    public bool IsCelebration { get; set; }

    /// <summary> Constructor.</summary>
    /// <param name="day">         The day. </param>
    /// <param name="appointment"> The appointment. </param>
    public BusinessEvent(IBusinessDay day,IAppointment appointment)
    {
        Day = day;

        Start = appointment.Start;
        End = appointment.End;

        Customer = AnalyzeCustomer(appointment.Subject, out var contact);
        Contact = contact;

        Street = appointment.Street ?? string.Empty;
        ZipCode = appointment.ZipCode ?? string.Empty;
        City = appointment.City ?? string.Empty;

        Remarks = appointment.Description;
    }

    /// <summary> Analyze customer.</summary>
    /// <param name="subject"> The subject. </param>
    /// <param name="contact"> [out] The contact. </param>
    /// <returns> A string.</returns>
    private static string AnalyzeCustomer(string subject, out string contact)
    {
        if (string.IsNullOrWhiteSpace(subject))
        {
            contact = string.Empty;
            return string.Empty;
        }

        if (!subject.Contains(',') || subject.Length < subject.IndexOf(',') + 2)
        {
            contact = string.Empty;
            return subject.Trim();
        }

        var substr = subject[(subject.IndexOf(',')+1)..];
        
        contact = substr.Trim();
        return subject[..subject.IndexOf(',')].Trim();
    }

    /// <summary> Executes the 'property changed' action.</summary>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary> Sets a field.</summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    /// <param name="field">        [in,out] The field. </param>
    /// <param name="value">        The value. </param>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    /// <returns> True if it succeeds, false if it fails.</returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}