using System.Collections.Generic;
using System.Threading.Tasks;
using VPets.Domain.Models;

namespace VPets.Domain.Repositories
{
    /// <summary>
    /// CRUD operations for Pets.
    /// </summary>
    public interface IPetRepository
    {
        Task<Pet> GetAsync(int id);

        Task<IEnumerable<Pet>> ListAsync();

        Task<IEnumerable<Pet>> ListAsyncForUser(int userId);

        Task CreateAsync(Pet pet);

        void Delete(Pet pet);

        void Put(Pet pet);
    }
}
