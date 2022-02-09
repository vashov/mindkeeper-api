using System;

namespace MindKeeper.Domain.Entities
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
