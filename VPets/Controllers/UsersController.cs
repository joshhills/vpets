using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VPets.Domain.Models;
using VPets.Domain.Services;
using VPets.Models;
using VPets.Resources;
using VPets.Services;

namespace VPets.Controllers
{
    [Route("/api/v1/users")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IPetService petService;
        private readonly IMapper mapper;

        public UsersController(IUserService userService, IPetService petService, IMapper mapper)
        {
            this.userService = userService;
            this.petService = petService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<UserResource>> GetAsync()
        {
            var users = await userService.ListAsync();
            var resources = mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(users);

            return resources;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var user = await userService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var resource = mapper.Map<User, UserResource>(user);

            return Ok(resource);
        }

        [HttpGet("{id}/pets")]
        public async Task<IActionResult> GetPetsAsync(int id)
        {
            var user = await userService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var pets = await petService.ListAsyncForUser(id);
            var resources = mapper.Map<IEnumerable<Pet>, IEnumerable<UserPetResource>>(pets);

            return Ok(resources);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateUserResource createUserResource)
        {
            var user = mapper.Map<CreateUserResource, User>(createUserResource);

            var result = await userService.CreateAsync(user);

            if(result == null)
            {
                return BadRequest();
            }

            var resource = mapper.Map<User, UserResource>(user);

            return Created($"/api/v1/users/{resource.Id}", resource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var user = await userService.DeleteAsync(id);

            if (user == null)
            {
                return BadRequest();
            }

            var resource = mapper.Map<User, UserResource>(user);

            return Ok(resource);
        }
    }
}