using System;
using System.ComponentModel;
using Akka.Actor;
using static Playground.Protocol.TicketCounterProtocol;

namespace Playground.Worker
{
    public class TicketCounterActor : ReceiveActor
    {
        private int _ticketCount = 0;
        
        public TicketCounterActor(int maxTickets)
        {
            _ticketCount = maxTickets;
            Receive<GetRemainingTicketCount>(_ =>
            {
                Context.Sender.Tell(new RetrievedRemainingTicketCount(_ticketCount));                
            });
            Receive<BuyTicket>(msg =>
            {
                if (_ticketCount <= 0)
                {
                    Context.Sender.Tell(new TicketsSoldOut());
                }
                else
                {
                    --_ticketCount;
                    Context.Sender.Tell(new TicketPurchased(_ticketCount));                    
                }                                                
            });
        }

        public static Props Props(int ticketCount) => 
            Akka.Actor.Props.Create(() => new TicketCounterActor(ticketCount));
    }
}
