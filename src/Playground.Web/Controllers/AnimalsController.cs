using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using static Playground.Protocol.AnimalProtocol;

namespace Playground.Web.Controllers
{
    [Route("/api/animals")]
    public class AnimalsController : Controller
    {
        private readonly WebService _webService;

        public AnimalsController(WebService webService)
        {
            _webService = webService;
        }

        public class AnimalInputModel
        {
            public string Species { get; set; }
        }

        /// <summary>
        /// Add a new Animal to the Zoo. The json body requires a species.
        /// <see cref="AnimalInputModel"/>
        /// </summary>
        /// <param name="animalName"></param>
        /// <param name="animalInputModel"></param>
        /// <returns></returns>
        [HttpPost("{animalName}")]
        public async Task<IActionResult> AddAnimal(string animalName, [FromBody] AnimalInputModel animalInputModel)
        {
            var result = await _webService.AnimalActor.Ask<AnimalCreationResponseMessage>(new AddAnimal(animalName, animalInputModel.Species), TimeSpan.FromSeconds(10));
            if (result is AnimalAdded a)
            {
                return Created($"/api/animals/{animalName}", a.Animal);
            }
            else if (result is AnimalAlreadyAdded)
            {
                return Conflict("Animal already added");
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadGateway);
            }
        }

        /// <summary>
        /// Locate a previously added animal by name.
        /// </summary>
        /// <param name="animalName"></param>
        /// <returns></returns>
        [HttpGet("{animalName}")]
        public async Task<IActionResult> TryFindAnimal(string animalName)
        {
            var result = await _webService.AnimalActor.Ask<FindAnimalResponseMessage>(new FindAnimal(animalName), TimeSpan.FromSeconds(10));
            if (result is FoundAnimalResponse a)
            {
                return Ok(a.Animal);
            }
            else if (result is CouldNotFindAnimalResponse)
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