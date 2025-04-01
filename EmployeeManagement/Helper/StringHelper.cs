using System.Globalization;

namespace EmployeeManagement.Helper
{
    public static class StringHelper
    {
        public static string FormatNumber(this decimal number)
        {
            return string.Format(new CultureInfo("vi-VN"), "{0:N0}", number);
        }
        public static string FormatNumberDecimal(this decimal number)
        {
            return number.ToString("#,##0.##").Replace(',', '.');
        }
        public static decimal ParseDecimal(this string? value)
        {
            // Replace dots (.) with empty string to remove thousand separators
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            string cleanValue = value.Replace(".", "");

            // Replace comma (,) with dot (.) to convert to decimal
            string formattedValue = cleanValue.Replace(",", ".");

            // Parse the formatted string to decimal
            if (decimal.TryParse(formattedValue, out decimal result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
    }
}
