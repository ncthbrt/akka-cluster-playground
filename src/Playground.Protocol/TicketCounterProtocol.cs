namespace Playground.Protocol
{
    /// <summary>
    /// Messages relating to TicketCounterActor
    /// </summary>
    public static class TicketCounterProtocol
    {
        public class BuyTicket
        {                        
        }
        
        public abstract class TicketPurchaseResult {}
        public class TicketPurchased: TicketPurchaseResult 
        {
            public int RemainingTicketCount { get; }
            public TicketPurchased(int remainingTicketCount)
            {
                RemainingTicketCount = remainingTicketCount;
            }
        }
        
        public class TicketsSoldOut: TicketPurchaseResult 
        {
        }
        
        public class GetRemainingTicketCount
        {            
        }
        
        public class RetrievedRemainingTicketCount
        {
            public int Count { get; }

            public RetrievedRemainingTicketCount(int count)
            {
                Count = count;
            }
        }
    }
}
