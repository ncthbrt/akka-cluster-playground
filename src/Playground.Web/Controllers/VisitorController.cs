using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using static Playground.Protocol.VisitorProtocol;

namespace Playground.Web.Controllers
{
    [Route("/api/visitors")]
    public class VisitorsController : Controller
    {
        private readonly WebService _webService;

        public VisitorsController(WebService webService)
        {
            _webService = webService;
        }

        /// <summary>
        /// Add a new Visitor to the Zoo.
        /// <see cref="AnimalInputModel"/>
        /// </summary>
        /// <param name="visitorName"></param>
        /// <returns></returns>
        [HttpPost("{visitorName}")]
        public async Task<IActionResult> AddVisitor(string visitorName)
        {
            var result = await _webService.VisitorActor.Ask<VisitorCreationResponseMessage>(
                new AddVisitor(visitorName), TimeSpan.FromSeconds(10));

            if (result is VisitorAdded v)
            {
                return Created($"/api/visitors/{visitorName}", v);
            }
            else if (result is VisitorAlreadyAdded)
            {
                return Conflict("Visitor already added");
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadGateway);
            }
        }

        /// <summary>
        /// Locate a previously added visitor by name.
        /// </summary>
        /// <param name="visitorName"></param>
        /// <returns></returns>
        [HttpGet("{visitorName}")]
        public async Task<IActionResult> TryFindVisitor(string visitorName)
        {
            var result = await _webService.VisitorActor.Ask<FindVisitorResponseMessage>(new FindVisitor(visitorName), TimeSpan.FromSeconds(10));
            if (result is FoundVisitorResponse v)
            {
                return Ok(v);
            }
            else if (result is CouldNotFindVisitorResponse)
            {
                return NotFound();
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadGateway);
            }
        }
    }
}