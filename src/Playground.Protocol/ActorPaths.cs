using Playground.Shared;

namespace Playground.Protocol
{
    public static class ActorPaths
    {    
        public static readonly ActorMetaData PubSubMediator = new ActorMetaData("distributedPubSubMediator", isSystemActor: true);
        public static readonly DistributedSingletonActorMetaData TicketCounterActor = new DistributedSingletonActorMetaData("ticket-counter", ActorRoles.Worker);
        
        public static readonly DistributedShardedActorMetaData<AnimalProtocol.AnimalMessageExtractor> AnimalActors = 
            new DistributedShardedActorMetaData<AnimalProtocol.AnimalMessageExtractor>("animals", ActorRoles.Worker);                
    }
}
