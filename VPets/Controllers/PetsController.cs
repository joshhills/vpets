using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPets.Domain.Models;
using VPets.Domain.Models.Pets;
using VPets.Domain.Services;
using VPets.Resources;
using static VPets.Domain.Models.Metric;

namespace VPets.Controllers
{
    [Route("/api/v1/pets")]
    [Produces("application/json")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetService petService;
        private readonly IMapper mapper;

        public PetsController(IPetService petService, IMapper mapper)
        {
            this.petService = petService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets a list of all Pets.
        /// </summary>
        /// <returns>A list of Pets</returns>
        /// <response code="200">Returns a list of Pets</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<PetResource>> GetAsync()
        {
            var pets = await petService.ListAsync();

            var resources = mapper.Map<IEnumerable<Pet>, IEnumerable<PetResource>>(pets);

            return resources;
        }

        /// <summary>
        /// Gets a specific Pet by Id.
        /// </summary>
        /// <returns>A Pet if one exists</returns>
        /// <param name="id">The Pet's unique Id</param>
        /// <response code="200">Pet found</response>
        /// <response code="404">Pet not found</response>
        /// <response code="400">Bad Id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PetResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var pet = await petService.GetAsync(id);

            if (pet == null)
            {
                return NotFound();
            }

            var resource = mapper.Map<Pet, PetResource>(pet);

            return Ok(resource);
        }

        /// <summary>
        /// Creates a Pet.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /pets
        ///     {
        ///        "name": "Woofus",
        ///        "type": "Dog",
        ///        "userId": 1
        ///     }
        ///
        /// </remarks>
        /// <returns>A Pet if one was created</returns>
        /// <param name="createPetResource">Pet information</param>
        /// <response code="201">Pet created</response>
        /// <response code="400">Bad request body</response>
        [HttpPost]
        [ProducesResponseType(typeof(PetResource), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostAsync([FromBody] CreatePetResource createPetResource)
        {
            Pet pet = null;

            // Figure out which kind of pet to create.
            switch (createPetResource.Type)
            {
                case PetType.CAT:
                    pet = mapper.Map<CreatePetResource, Cat>(createPetResource);
                    break;
                case PetType.DOG:
                    pet = mapper.Map<CreatePetResource, Dog>(createPetResource);
                    break;
            }

            if (pet == null)
            {
                return BadRequest();
            }

            var result = await petService.CreateAsync(pet);

            if (result == null)
            {
                return BadRequest();
            }

            var resource = mapper.Map<Pet, CreatedPetResource>(pet);

            return Created($"/api/v1/pets/{resource.Id}", resource);
        }

        /// <summary>
        /// Deletes a specific Pet.
        /// </summary>
        /// <param name="id">The Pet's unique Id</param>
        /// <response code="200">Pet deleted</response>
        /// <response code="400">Bad Id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(PetResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var pet = await petService.DeleteAsync(id);

            if (pet == null)
            {
                return BadRequest();
            }

            var resource = mapper.Map<Pet, PetResource>(pet);

            return Ok(resource);
        }

        /* Interactions */

        /// <summary>
        /// Interact with an arbitrary metric attached to a Pet,
        /// making it more positive.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /pets/1/interact?onMetric=hunger
        ///
        /// </remarks>
        /// <param name="id">The Pet's unique Id</param>
        /// <param name="onMetric">The metric to act on.</param>
        /// <response code="200">Pet metric updated</response>
        /// <response code="400">Bad Id or metric does not exist</response>
        [HttpPost("{id}/interact")]
        [ProducesResponseType(typeof(PetResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostInteractAsync(int id, [FromQuery] MetricType onMetric)
        {
            return await HandleInteractAsync(id, onMetric);
        }

        /// <summary>
        /// Interact with the hunger metric attached to a Pet,
        /// making it more positive - if it exists.
        /// </summary>
        /// <param name="id">The Pet's unique Id</param>
        /// <response code="200">Pet metric updated</response>
        /// <response code="400">Bad Id or metric does not exist</response>
        [HttpPost("{id}/interact/feed")]
        [ProducesResponseType(typeof(PetResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostInteractFeedAsync(int id)
        {
            return await HandleInteractAsync(id, MetricType.HUNGER);
        }

        /// <summary>
        /// Interact with the happiness metric attached to a Pet,
        /// making it more positive - if it exists.
        /// </summary>
        /// <param name="id">The Pet's unique Id</param>
        /// <response code="200">Pet metric updated</response>
        /// <response code="400">Bad Id or metric does not exist</response>
        [HttpPost("{id}/interact/stroke")]
        [ProducesResponseType(typeof(PetResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostInteractStrokeAsync(int id)
        {
            return await HandleInteractAsync(id, MetricType.HAPPINESS);
        }

        /// <summary>
        /// Reduce boilerplate code, enable nicer endpoints and demonstrate
        /// extensibility by extracting pet interaction handling into one
        /// place.
        /// </summary>
        /// <param name="id">Unique Pet Id</param>
        /// <param name="onMetric">Metric type to interact with</param>
        /// <returns>The outcome of the interaction</returns>
        private async Task<IActionResult> HandleInteractAsync(int id, MetricType onMetric)
        {
            var pet = await petService.InteractAsync(id, onMetric);

            if (pet == null)
            {
                return BadRequest();
            }

            var resource = mapper.Map<Pet, PetResource>(pet);

            return Ok(resource);
        }
    }
}
