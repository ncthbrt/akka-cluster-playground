using Akka.Cluster.Sharding;
using static Playground.Protocol.VisitorProtocol;

namespace Playground.Protocol
{
    public class VisitorMessageExtractor : HashCodeMessageExtractor
    {
        public VisitorMessageExtractor() : base(10)
        {
        }

        public override string EntityId(object message)
        {
            if (message is VisitorMessage v)
            {
                return v.Name;
            }
            return null;
        }
    }
}