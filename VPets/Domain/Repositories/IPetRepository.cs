using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPets.Domain.Models;

namespace VPets.Domain.Repositories
{
    public interface IPetRepository
    {
        Task<IEnumerable<Pet>> ListAsync();

        Task CreateAsync(Pet pet);
    }
}
