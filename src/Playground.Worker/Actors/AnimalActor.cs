using Akka.Actor;
using Akka.Cluster.Sharding;
using Playground.Shared.Domain;
using static Playground.Protocol.AnimalProtocol;

namespace Playground.Worker
{
    public class AnimalActor : ReceiveActor
    {
        private bool _initialized;
        private Animal _animal;
        public AnimalActor()
        {
            Receive<AddAnimal>(_ => !_initialized, msg =>
            {
                _initialized = true;
                _animal = new Animal(msg.AnimalName, msg.Species);
                Sender.Tell(new AnimalAdded(_animal));
            });

            Receive<AddAnimal>(_ => Sender.Tell(new AnimalAlreadyAdded(_animal)));
            
            Receive<FindAnimal>(_ => _initialized, _ => Sender.Tell(new FoundAnimalResponse(_animal)));

            Receive<FindAnimal>( r => { 
                Sender.Tell(new CouldNotFindAnimalResponse(r.AnimalName));
                Context.Parent.Tell(new Passivate(PoisonPill.Instance));
            });
        }

        //Sharded Actor Props cannot contain actor-specific state information so we need to initialize an Animal's species via message.
        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new AnimalActor());
        }
        
    }
}
