using Neo4j.Driver.Extensions;
using System;
using System.Text.Json.Serialization;

namespace MindKeeper.Domain.Entities
{
    public class BaseEntity
    {
        private Guid? _id;

        [Neo4jProperty(Ignore = true)]
        public Guid Id
        {
            get
            {
                if (!_id.HasValue)
                    _id = Guid.Parse(IdString);
                return _id.Value;
            }
        }

        [JsonIgnore]
        [Neo4jProperty(Name = "Id")]
        public string IdString { get; set; }
    }
}
