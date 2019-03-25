using LanguageExt;

namespace Playground.Shared
{
    public class DistributedActorMetaData
    {
        public string Name { get; }
        public Option<string> Role { get; }

        public DistributedActorMetaData(string name, Option<string> role)
        {
            Name = name;
            Role = role;
        }
        
        
    }
}
