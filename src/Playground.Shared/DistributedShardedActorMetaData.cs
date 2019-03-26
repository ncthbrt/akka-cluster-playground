using Akka.Actor;
using Akka.Cluster.Sharding;
using LanguageExt;

namespace Playground.Shared
{
    public class DistributedShardedActorMetaData<T> : DistributedActorMetaData
        where T: IMessageExtractor, new()
    {
        public DistributedShardedActorMetaData(string name, Option<string> role) : base(name, role)
        {            
        }

        public IActorRef StartProxy(ActorSystem system)
        {
            return ClusterSharding.Get(system).StartProxy(
                Name,
                Role.IfNoneUnsafe(() => null),
                new T()
            );
        }
        
        public IActorRef Start(ActorSystem system, Props actorProps)
        {           
            return ClusterSharding.Get(system).Start(
                Name,
                actorProps,
                ClusterShardingSettings.Create(system).WithRole(Role.IfNoneUnsafe(()=>null)),
                new T()
            );
        }
        
        public IActorRef Start(ActorSystem system, ClusterShardingSettings settings, Props actorProps)
        {           
            return ClusterSharding.Get(system).Start(
                Name,
                actorProps,
                settings.WithRole(Role.IfNoneUnsafe(()=>null)),
                new T()
            );
        }
    }
}
