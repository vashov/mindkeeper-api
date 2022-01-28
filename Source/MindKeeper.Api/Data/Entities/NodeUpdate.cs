using System;

namespace MindKeeper.Api.Data.Entities
{
    public class NodeUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NodeId { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
