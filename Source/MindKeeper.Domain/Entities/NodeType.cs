using System;

namespace MindKeeper.Domain.Entities
{
    public class NodeType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEditable { get; set; }
    }
}
