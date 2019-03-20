using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using Petabridge.Cmd.Host;

namespace Playground.Web
{
    public class WebService : IDisposable
    {
        private readonly string _actorSystemName;
        private readonly Config _akkaConfig;

        public WebService(string actorSystemName, Config akkaConfig)
        {
            _actorSystemName = actorSystemName;
            _akkaConfig = akkaConfig;
            _system = ActorSystem.Create(_actorSystemName, _akkaConfig);
            {
                var cmd = PetabridgeCmd.Get(_system);
                cmd.RegisterCommandPalette(Petabridge.Cmd.Cluster.ClusterCommands.Instance);
                cmd.RegisterCommandPalette(Petabridge.Cmd.Cluster.Sharding.ClusterShardingCommands.Instance);
                // Register custom cmd commands here             
                cmd.Start();
            }
            DistributedPubSub.Get(_system);
        }       
        
        private ActorSystem _system;        
        
        private async void StopAsync()
        {
            await CoordinatedShutdown.Get(_system).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }

        public void Dispose()
        {
            StopAsync();            
        }
    }
}
