using System.Text.RegularExpressions;

namespace ExpenseTracker.Api.Common.Validations
{
    public static class ColorValidator
    {
        private static readonly Regex HexColorRegex =
            new(@"^#([0-9a-fA-F]{3}|[0-9a-fA-F]{6})$",
                RegexOptions.Compiled);

        public static bool IsValidHex(string? color)
        {
            if (string.IsNullOrWhiteSpace(color))
                return false;

            return HexColorRegex.IsMatch(color);
        }
    }
}