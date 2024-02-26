using System.Windows.Data;
// ReSharper disable NullnessAnnotationConflictWithJetBrainsAnnotations

namespace OutlookAdReport.WpfUi.Utils.Converters;

public class BoolToValueConverter<T> : IValueConverter
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public T FalseValue { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public T TrueValue { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (value == null)
#pragma warning disable CS8603 // Possible null reference return.
            return FalseValue;
#pragma warning restore CS8603 // Possible null reference return.
        else
#pragma warning disable CS8603 // Possible null reference return.
            return (bool)value ? TrueValue : FalseValue;
#pragma warning restore CS8603 // Possible null reference return.
    }

    // ReSharper disable NullnessAnnotationConflictWithJetBrainsAnnotations
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        return value != null && value.Equals(TrueValue);
    }
}

/// <summary>   to string converter. </summary>
public class BoolToStringConverter : BoolToValueConverter<string> { }