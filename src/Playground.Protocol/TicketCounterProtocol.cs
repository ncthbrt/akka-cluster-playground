namespace Playground.Protocol
{
    public static class TicketCounterProtocol
    {
        public class IncrementTickets
        {
            public int TicketCount { get; }

            public IncrementTickets(int ticketCount)
            {
                TicketCount = ticketCount;
            }
        }
        public class IncrementedTickets
        {
            public int NewTotalTicketCount { get; }

            public IncrementedTickets(int ticketCount)
            {
                NewTotalTicketCount = ticketCount;
            }
        }
        
        public class GetTicketCount
        {            
        }
        
        public class RetrievedTicketCount
        {
            public int Count { get; }

            public RetrievedTicketCount(int count)
            {
                Count = count;
            }
        }
    }
}
