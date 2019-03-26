using System;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using static Playground.Protocol.TicketCounterProtocol;

namespace Playground.Web.Controllers
{
    [Route("/api/tickets")]
    public class TicketsController : Controller
    {
        private readonly WebService _webService;

        public TicketsController(WebService webService)
        {
            _webService = webService;            
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var result = await _webService.TicketActor.Ask<RetrievedRemainingTicketCount>(new GetRemainingTicketCount(), TimeSpan.FromSeconds(10));
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> PostTickets()
        {
            var result = await _webService.TicketActor.Ask<TicketPurchaseResult>(new BuyTicket(), TimeSpan.FromSeconds(10));
            if (result is TicketPurchased purchased)
            {
                return Ok(new { remainingTickets = purchased.RemainingTicketCount });
            }
            else
            {
                return BadRequest("No tickets remaining");    
            }            
        }
        
    }
}
