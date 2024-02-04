using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using OutlookAdReport.Data.Models;
using OutlookAdReport.WpfUi.ViewModels;

namespace OutlookAdReport.WpfUi.Utils.Converters;

/// <summary> An event message type to color converter.</summary>
public class EventMessageTypeToColorConverter : IValueConverter
{
    /// <summary> Converts a value.</summary>
    /// <param name="value">      The value produced by the binding source. </param>
    /// <param name="targetType"> The type of the binding target property. </param>
    /// <param name="parameter">  The converter parameter to use. </param>
    /// <param name="culture">    The culture to use in the converter. </param>
    /// <returns>
    ///     A converted value. If the method returns <see langword="null" />, the valid null
    ///     value is used.
    /// </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not EventMessageType status) return Binding.DoNothing;

        var color = status switch
        {
            EventMessageType.Success => new SolidColorBrush(Colors.Green),
            EventMessageType.Warning => new SolidColorBrush(Colors.Yellow),
            EventMessageType.Error => new SolidColorBrush(Colors.Red),
            _ => new SolidColorBrush(Colors.White)
        };

        return color;
    }

    /// <summary> Converts a value.</summary>
    /// <exception cref="NotSupportedException">
    ///     Thrown when the requested operation is not
    ///     supported.
    /// </exception>
    /// <param name="value">      The value that is produced by the binding target. </param>
    /// <param name="targetType"> The type to convert to. </param>
    /// <param name="parameter">  The converter parameter to use. </param>
    /// <param name="culture">    The culture to use in the converter. </param>
    /// <returns>
    ///     A converted value. If the method returns <see langword="null" />, the valid null
    ///     value is used.
    /// </returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}