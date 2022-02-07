using Neo4j.Driver.Extensions;
using System;
using System.Text.Json.Serialization;

namespace MindKeeper.Domain.Entities
{
    public class BaseEntity
    {
        [Neo4jProperty(Ignore = true)]
        public Guid Id { get; }

        [JsonIgnore]
        [Neo4jProperty(Name = "Id")]
        public string IdString { get; set; }
    }
}
