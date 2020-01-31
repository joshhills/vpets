using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPets.Domain.Models;
using VPets.Domain.Services;
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

        /// <summary>
        /// Gets a list of all Users.
        /// </summary>
        /// <returns>A list of Users</returns>
        /// <response code="200">Returns a list of Users</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<UserResource>> GetAsync()
        {
            var users = await userService.ListAsync();
            var resources = mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(users);

            return resources;
        }

        /// <summary>
        /// Gets a specific User by Id.
        /// </summary>
        /// <returns>A User if one exists</returns>
        /// <param name="id">The User's unique Id</param>
        /// <response code="200">User found</response>
        /// <response code="404">User not found</response>
        /// <response code="400">Bad Id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Gets a list of all Pets related to a specific User.
        /// </summary>
        /// <returns>A list of Pets belonging to the User if they exist</returns>
        /// <param name="id">The User's unique Id</param>
        /// <response code="200">User found</response>
        /// <response code="404">User not found</response>
        /// <response code="400">Bad Id</response>
        [HttpGet("{id}/pets")]
        [ProducesResponseType(typeof(IEnumerable<UserPetResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Creates a User.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /users
        ///     {
        ///        "name": "Josh"
        ///     }
        ///
        /// </remarks>
        /// <returns>A User if one was created</returns>
        /// <param name="createUserResource">User information</param>
        /// <response code="201">User created</response>
        /// <response code="400">Bad request body</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserResource), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Deletes a specific User.
        /// </summary>
        /// <param name="id">The User's unique Id</param>
        /// <response code="200">User deleted</response>
        /// <response code="400">Bad Id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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