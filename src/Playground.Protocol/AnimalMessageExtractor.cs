using Akka.Cluster.Sharding;
using static Playground.Protocol.AnimalProtocol;

namespace Playground.Protocol
{
    public class AnimalMessageExtractor : HashCodeMessageExtractor
    {
        public AnimalMessageExtractor() : base(10)
        {
        }

        public override string EntityId(object message)
        {
            if (message is AnimalMessage a)
            {
                return a.AnimalName;
            }
            return null;
        }
    }
}