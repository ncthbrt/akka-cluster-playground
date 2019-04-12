using Playground.Shared;

namespace Playground.Protocol
{
    public static class ActorPaths
    {
        public static readonly ActorMetaData PubSubMediator = new ActorMetaData("distributedPubSubMediator", isSystemActor: true);
        public static readonly DistributedSingletonActorMetaData TicketCounterActor = new DistributedSingletonActorMetaData("ticket-counter", ActorRoles.Worker);

        public static readonly DistributedShardedActorMetaData<AnimalMessageExtractor> AnimalActors =
            new DistributedShardedActorMetaData<AnimalMessageExtractor>("animals", ActorRoles.Worker);

        public static readonly DistributedShardedActorMetaData<VisitorMessageExtractor> VisitorActor =
            new DistributedShardedActorMetaData<VisitorMessageExtractor>("visitors", ActorRoles.Worker);
    }
}