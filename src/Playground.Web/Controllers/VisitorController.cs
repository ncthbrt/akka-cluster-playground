using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using static Playground.Protocol.VisitorProtocol;

namespace Playground.Web.Controllers
{
    /// <summary>
    /// Visitors to the zoo should require a ticket to enter!
    /// </summary>
    [Route("/api/visitors")]
    public class VisitorsController : Controller
    {
        private readonly WebService _webService;

        /// <summary>
        /// Visitors Controller
        /// </summary>
        /// <param name="webService"></param>
        public VisitorsController(WebService webService)
        {
            _webService = webService;
        }

        /// <summary>
        /// Add a new Visitor to the Zoo.
        /// </summary>
        /// <param name="visitorName"></param>
        /// <returns></returns>
        [HttpPost("{visitorName}")]
        public async Task<IActionResult> AddVisitor(string visitorName)
        {
            return StatusCode((int)HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// Locate a previously added visitor by name.
        /// </summary>
        /// <param name="visitorName"></param>
        /// <returns></returns>
        [HttpGet("{visitorName}")]
        public async Task<IActionResult> TryFindVisitor(string visitorName)
        {
            return StatusCode((int)HttpStatusCode.NotImplemented);
        }
    }
}