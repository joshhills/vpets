using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPets.Domain.Models;
using VPets.Domain.Repositories;
using VPets.Domain.Services;
using VPets.Persistence.Repositories;

namespace VPets.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository petRepository;
        private readonly IUnitOfWork unitOfWork;

        public PetService(IPetRepository petRepository, IUnitOfWork unitOfWork)
        {
            this.petRepository = petRepository;
            this.unitOfWork = unitOfWork;
        }

        public Task<Pet> CreateAsync(Pet pet)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Pet>> ListAsync()
        {
            return await petRepository.ListAsync();
        }
    }
}
