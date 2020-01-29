using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAsync()
        {
            return await userService.ListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var user = await userService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] UserResource userResource)
        {
            var user = mapper.Map<UserResource, User>(userResource);

            var result = await userService.CreateAsync(user);

            if(result == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var user = await userService.DeleteAsync(id);

            if (user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }
    }
}