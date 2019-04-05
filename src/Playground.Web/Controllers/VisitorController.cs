using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<IActionResult> GetVisitors()
        {
            return Ok();
        }
    }
}
