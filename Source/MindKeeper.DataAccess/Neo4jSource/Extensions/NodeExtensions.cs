using Neo4j.Driver;
using System;

namespace MindKeeper.DataAccess.Neo4jSource.Extensions
{
    public static class NodeExtensions
    {
        public static T AsEntity<T>(this INode node)
        {
            var type = typeof(T);
            var entity = Activator.CreateInstance(type);

            foreach (var property in type.GetProperties())
            {
                if (property.Name == "Id")
                {
                    var id = node.Id;
                    property.SetValue(entity, id);
                    continue;
                }

                if (!node.Properties.ContainsKey(property.Name))
                    continue;

                var value = node.Properties[property.Name];
                if (value is ZonedDateTime zoned)
                {
                    value = zoned.ToDateTimeOffset();
                }

                property.SetValue(entity, value);
            }

            return (T)entity;
        }
    }
}
