using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using Petabridge.Cmd.Host;

namespace Playground.Worker
{
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

        public void Start(CancellationToken token)
        {
            _system = ActorSystem.Create(_actorSystemName, _akkaConfig);
            
            var cmd = PetabridgeCmd.Get(_system);
            cmd.RegisterCommandPalette(Petabridge.Cmd.Cluster.ClusterCommands.Instance);
            cmd.RegisterCommandPalette(Petabridge.Cmd.Cluster.Sharding.ClusterShardingCommands.Instance);
            // Register custom cmd commands here             
            cmd.Start();
            
            DistributedPubSub.Get(_system);
            
            token.Register(StopAsync);
        }
        
        private async void StopAsync()
        {
            await CoordinatedShutdown.Get(_system).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }       
    }
}
