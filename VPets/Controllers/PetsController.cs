using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VPets.Domain.Models;
using VPets.Domain.Services;
using VPets.Services;

namespace VPets.Controllers
{
    [Route("/api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class PetsController
    {
        private readonly IPetService petService;
        private readonly IMapper mapper;

        public PetsController(IPetService petService, IMapper mapper)
        {
            this.petService = petService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Pet>> GetAsync()
        {
            return await petService.ListAsync();
        }
    }
}
