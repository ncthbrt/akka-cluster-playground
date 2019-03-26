namespace Playground.Shared.Domain
{
    public class Animal
    {
        public Animal(string name, string species)
        {
            Name = name;
            Species = species;     
        }

        public string Name { get; }
        public string Species { get; }
    }
}
