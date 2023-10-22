using System;
using System.Windows.Data;

namespace SudokuWpf.Converters;

public class ZeroValueConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        string? stringValue = value.ToString();
        if (string.IsNullOrWhiteSpace(stringValue) || stringValue.Equals("0"))
        {
            return string.Empty;
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        string? stringValue = value.ToString();
        return string.IsNullOrWhiteSpace(stringValue) ? 0 : value;
    }
}