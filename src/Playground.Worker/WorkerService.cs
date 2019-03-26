using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Cluster.Tools.Singleton;
using Akka.Configuration;
using Petabridge.Cmd.Host;
using Playground.Protocol;

namespace Playground.Worker
{
    public class WorkerService
    {
        private readonly string _actorSystemName;
        private IActorRef _singleton;
        private readonly Config _akkaConfig;

        public WorkerService(string actorSystemName, Config akkaConfig)
        {
            _actorSystemName = actorSystemName;
            _akkaConfig = akkaConfig;
        }

        private ActorSystem _system;
        public Task TerminationHandle => _system.WhenTerminated;

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
            _system.ActorOf(ActorPaths.TicketCounterActor.ManagerProps(_system, TicketCounterActor.Props(10)), ActorPaths.TicketCounterActor.Name);
            ActorPaths.AnimalActors.Start(_system, AnimalActor.Props());
        }

        private async void StopAsync()
        {
            await CoordinatedShutdown.Get(_system).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }
}
