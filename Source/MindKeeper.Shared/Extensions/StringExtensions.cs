namespace MindKeeper.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string PrepareForSqlLike(this string value, bool putPercentToEnd = false)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            value = value.Replace("[", "[[]").Replace("%", "[%]");

            if (putPercentToEnd)
                value += "%";

            return value;
        }
    }
}
