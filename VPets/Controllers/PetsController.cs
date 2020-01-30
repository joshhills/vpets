using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VPets.Domain.Models;
using VPets.Domain.Services;
using VPets.Resources;

namespace VPets.Controllers
{
    [Route("/api/v1/[controller]")]
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
    }
}
