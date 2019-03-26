namespace Playground.Shared.Domain
{
    public class Enclosure
    {
        public int EnclosureId { get; }
        public string Name { get; }
        public int Capacity { get; }

        public Enclosure(int enclosureId, string name, int capacity)
        {
            EnclosureId = enclosureId;
            Name = name;
            Capacity = capacity;
        }
    }
}
