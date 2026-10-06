using System.Globalization;
using System.Windows.Controls;

namespace ProcHub.Wpf.Infrastructure.Validation;

public sealed class DateOnlyValidationRule : ValidationRule
{
    public string DateFormat { get; set; } = "dd-MM-yyyy";

    public override ValidationResult Validate(
        object value, 
        CultureInfo cultureInfo)
    {
        var text = value?
            .ToString()?
            .Trim();

        if (string.IsNullOrWhiteSpace(text)) 
        {
            return ValidationResult.ValidResult;
        }

        var isValid = DateOnly.TryParseExact(
            text,
            DateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _);

        return isValid
            ? ValidationResult.ValidResult
            : new ValidationResult(
                false,
                $"Use date format {DateFormat}.");
    }
}