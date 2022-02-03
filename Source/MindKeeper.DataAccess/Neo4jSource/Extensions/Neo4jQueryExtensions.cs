using System.Text;

namespace MindKeeper.DataAccess.Neo4jSource.Extensions
{
    public static class Neo4jQueryExtensions
    {
        public static string AsProperties<T>(this T value)
        {
            var type = value.GetType();
            var builder = new StringBuilder();

            var properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                var prop = properties[i];
                var property = $"{prop.Name}: ${prop.Name}";
                builder.Append(property);

                if (i < properties.Length - 1)
                    builder.Append(',');
            }

            return builder.ToString();
        }
    }
}
