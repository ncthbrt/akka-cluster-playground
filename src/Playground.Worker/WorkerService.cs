using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Cluster.Tools.Singleton;
using Akka.Configuration;
using Petabridge.Cmd.Host;
using Playground.Protocol;
using System.Threading;
using System.Threading.Tasks;

namespace Playground.Worker
{
    /// <summary>
    /// The Worker service contains actual actors.
    /// </summary>
    public class WorkerService
    {
        private readonly string _actorSystemName;
        private readonly Config _akkaConfig;

        public WorkerService(string actorSystemName, Config akkaConfig)
        {
            _actorSystemName = actorSystemName;
            _akkaConfig = akkaConfig;
        }

        private ActorSystem _system;
        public Task TerminationHandle => _system.WhenTerminated;

        public static IActorRef TicketCounter;

        //Creating a new WorkerService creates a new ActorSystem.
        public void Start(CancellationToken token)
        {
            _system = ActorSystem.Create(_actorSystemName, _akkaConfig);

            var cmd = PetabridgeCmd.Get(_system);
            cmd.RegisterCommandPalette(Petabridge.Cmd.Cluster.ClusterCommands.Instance);
            cmd.RegisterCommandPalette(Petabridge.Cmd.Cluster.Sharding.ClusterShardingCommands.Instance);
            // Register custom cmd commands here
            cmd.Start();

            DistributedPubSub.Get(_system);

            StartActors();

            token.Register(StopAsync);
        }

        private void StartActors()
        {
            //Start a singleton.
            ActorPaths.TicketCounterActor.Start(_system, TicketCounterActor.Props(10));

            //Start a sharded actor definition.
            ActorPaths.AnimalActors.Start(_system, AnimalActor.Props());
        }

        private async void StopAsync()
        {
            //await CoordinatedShutdown.Get(_system).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }
}