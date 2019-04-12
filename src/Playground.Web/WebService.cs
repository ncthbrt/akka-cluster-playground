using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using Petabridge.Cmd.Host;
using Playground.Protocol;
using System;

namespace Playground.Web
{
    /// <summary>
    /// The Web service allows us to access actors via proxy.
    /// We register a proxy accessor for both TicketActor and AnimalActor here.
    /// </summary>
    public class WebService : IDisposable
    {
        private ActorSystem _system;

        public IActorRef TicketActor { get; }
        public IActorRef AnimalActor { get; }

        public IActorRef VisitorActor { get; }

        //Creating a new WebService creates a new ActorSystem.
        public WebService(string actorSystemName, Config akkaConfig)
        {
            _system = ActorSystem.Create(actorSystemName, akkaConfig);

            var cmd = PetabridgeCmd.Get(_system);
            cmd.RegisterCommandPalette(Petabridge.Cmd.Cluster.ClusterCommands.Instance);
            cmd.RegisterCommandPalette(Petabridge.Cmd.Cluster.Sharding.ClusterShardingCommands.Instance);
            // Register custom cmd commands here
            cmd.Start();

            DistributedPubSub.Get(_system);

            //Singleton proxy for TicketActor
            TicketActor = ActorPaths.TicketCounterActor.StartProxy(_system);

            //Shard proxy for AnimalActor
            AnimalActor = ActorPaths.AnimalActors.StartProxy(_system);

            VisitorActor = ActorPaths.VisitorActor.StartProxy(_system);
        }

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