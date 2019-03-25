using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Cluster.Tools.Singleton;
using Akka.Configuration;
using Petabridge.Cmd.Host;
using Playground.Protocol;

namespace Playground.Web
{
    public class WebService : IDisposable
    {
        public IActorRef TicketActor { get; }

        public WebService(string actorSystemName, Config akkaConfig)
        {
            _system = ActorSystem.Create(actorSystemName, akkaConfig);
            {
                var cmd = PetabridgeCmd.Get(_system);
                cmd.RegisterCommandPalette(Petabridge.Cmd.Cluster.ClusterCommands.Instance);
                cmd.RegisterCommandPalette(Petabridge.Cmd.Cluster.Sharding.ClusterShardingCommands.Instance);
                // Register custom cmd commands here             
                cmd.Start();
            }
            DistributedPubSub.Get(_system);
            
            TicketActor = _system.ActorOf(ActorPaths.TicketCounterActor.ProxyProps(_system), ActorPaths.TicketCounterActor.Name + "Proxy");            

        }       
        
        private ActorSystem _system;        
        
        private void StopAsync()
        {
            CoordinatedShutdown.Get(_system).Run(CoordinatedShutdown.ClrExitReason.Instance).Wait();
        }

        public void Dispose()
        {
            Console.WriteLine("STOPPING");
            StopAsync();            
        }
    }
}
