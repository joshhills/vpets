using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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

        [HttpGet]
        public async Task<IEnumerable<PetResource>> GetAsync()
        {
            var pets = await petService.ListAsync();

            var resources = mapper.Map<IEnumerable<Pet>, IEnumerable<PetResource>>(pets);

            return resources;
        }

        [HttpGet("{id}")]
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

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreatePetResource createPetResource)
        {
            Pet pet = null;

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

        [HttpDelete("{id}")]
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

        [HttpPost("{id}/interact")]
        public async Task<IActionResult> PostInteractAsync(int id, [FromQuery] MetricType onMetric)
        {
            return await HandleInteractAsync(id, onMetric);
        }

        [HttpPost("{id}/interact/feed")]
        public async Task<IActionResult> PostInteractFeedAsync(int id)
        {
            return await HandleInteractAsync(id, MetricType.HUNGER);
        }

        [HttpPost("{id}/interact/stroke")]
        public async Task<IActionResult> PostInteractStrokeAsync(int id)
        {
            return await HandleInteractAsync(id, MetricType.HAPPINESS);
        }

        private async Task<IActionResult> HandleInteractAsync(int id, MetricType onMetric)
        {
            var pet = await petService.InteractAsync(id, onMetric);

            if (pet == null)
            {
                return NotFound();
            }

            var resource = mapper.Map<Pet, PetResource>(pet);

            return Ok(resource);
        }
    }
}
