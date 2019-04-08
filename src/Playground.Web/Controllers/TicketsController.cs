using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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

        /// <summary>
        /// Get remaining tickets for sale.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRemainingTicketCount()
        {
            var result = await _webService.TicketActor.Ask<RetrievedRemainingTicketCount>(new GetRemainingTicketCount(), TimeSpan.FromSeconds(10));
            return Ok(result);
        }

        /// <summary>
        /// Buy one ticket.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> BuyTicket()
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