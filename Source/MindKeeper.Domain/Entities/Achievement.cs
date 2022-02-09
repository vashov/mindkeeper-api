namespace MindKeeper.Domain.Entities
{
    public class Achievement
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSecret { get; set; }
    }
}
