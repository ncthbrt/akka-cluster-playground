using System.ComponentModel;
using Akka.Actor;
using static Playground.Protocol.TicketCounterProtocol;

namespace Playground.Worker
{
    public class TicketCounterActor : ReceiveActor
    {
        private int _ticketCount = 0;
        
        public TicketCounterActor()
        {
            Receive<GetTicketCount>(_ => Context.Sender.Tell(new RetrievedTicketCount(_ticketCount)));
            Receive<IncrementTickets>(msg =>
            {
                _ticketCount += msg.TicketCount;                
                Context.Sender.Tell(new IncrementedTickets(_ticketCount));
            });
        }

        public static Props Props() => 
            Akka.Actor.Props.Create(() => new TicketCounterActor());
    }
}
