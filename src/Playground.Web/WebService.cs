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

        /// <summary>
        /// Reference To Ticket Singleton Proxy
        /// </summary>
        public IActorRef TicketActor { get; }

        /// <summary>
        /// Reference to Animal Shard Proxy
        /// </summary>
        public IActorRef AnimalActor { get; }

        /// <summary>
        ///  Creating a new WebService creates a new ActorSystem.
        /// </summary>
        public WebService(string actorSystemName, Config akkaConfig)
        {
            _system = ActorSystem.Create(actorSystemName, akkaConfig);

            DistributedPubSub.Get(_system);

            //Singleton proxy for TicketActor
            TicketActor = ActorPaths.TicketCounterActor.StartProxy(_system);

            //Shard proxy for AnimalActor
            AnimalActor = ActorPaths.AnimalActors.StartProxy(_system);
        }

        private void StopAsync()
        {
            CoordinatedShutdown.Get(_system).Run(CoordinatedShutdown.ClrExitReason.Instance).Wait();
        }

        /// <summary>
        /// Disposes the actorSystem!
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine("STOPPING");
            StopAsync();
        }
    }
}