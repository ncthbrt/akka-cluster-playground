using Akka.Actor;
using Akka.Cluster.Sharding;
using Playground.Shared.Domain;
using System;
using static Playground.Protocol.TicketCounterProtocol;
using static Playground.Protocol.VisitorProtocol;

namespace Playground.Worker
{
    public class VisitorActor : ReceiveActor
    {
        private bool _initialized;
        private Visitor _visitor;

        public VisitorActor()
        {
            Receive<AddVisitor>(_ => !_initialized, msg =>
            {
                _initialized = true;

                var getTicket = WorkerService.TicketCounter.Ask<TicketPurchaseResult>(
                    new BuyTicket(),
                    TimeSpan.FromSeconds(10))
                    .Result;

                _visitor = new Visitor(msg.Name, getTicket is TicketPurchased);
                var message = new VisitorAdded(_visitor);
                Sender.Tell(message);
            });

            Receive<AddVisitor>(_ =>
            {
                Sender.Tell(new VisitorAlreadyAdded(_visitor));
            });

            Receive<FindVisitor>(_ => _initialized, _ => Sender.Tell(new FoundVisitorResponse(_visitor)));

            Receive<FindVisitor>(r =>
            {
                Sender.Tell(new CouldNotFindVisitorResponse(r.Name));
                Context.Parent.Tell(new Passivate(PoisonPill.Instance));
            });
        }

        //Sharded Actor Props cannot contain actor-specific state information so we need to initialize an visitor's species via message.
        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new VisitorActor());
        }
    }
}