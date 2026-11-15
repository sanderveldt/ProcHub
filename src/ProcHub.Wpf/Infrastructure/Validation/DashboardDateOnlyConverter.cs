using System.Globalization;
using System.Windows.Data;

namespace ProcHub.Wpf.Infrastructure.Validation;

public sealed class DashboardDateOnlyConverter : IValueConverter
{
    private const string DateFormat = "dd-MM-yyyy";

    public object Convert(
        object? value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        return value is DateOnly date
            ? date.ToString(DateFormat, CultureInfo.InvariantCulture)
            : string.Empty;
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        var text = (value as string)?.Trim();

        return string.IsNullOrEmpty(text)
            ? null
            : DateOnly.ParseExact(
                text,
                DateFormat,
                CultureInfo.InvariantCulture);
    }
}
