using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPets.Domain.Models;

namespace VPets.Domain.Repositories
{
    public interface IPetRepository
    {
        Task<Pet> GetAsync(int id);

        Task<IEnumerable<Pet>> ListAsync();

        Task CreateAsync(Pet pet);

        void Delete(Pet pet);

        void Put(Pet pet);
    }
}
