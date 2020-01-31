using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VPets.Domain.Models;
using VPets.Domain.Repositories;
using VPets.Persistence.Contexts;

namespace VPets.Persistence.Repositories
{
    /// <summary>
    /// I/O on the databsae context for Pet table.
    /// </summary>
    public class PetRepository : BaseRepository, IPetRepository
    {
        public PetRepository(AppDbContext context) : base(context)
        {
        }

        public async Task CreateAsync(Pet pet)
        {
            await context.Pets.AddAsync(pet);
        }

        public void Delete(Pet pet)
        {
            context.Pets.Remove(pet);
        }

        public async Task<Pet> GetAsync(int id)
        {
            return await context.Pets.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pet>> ListAsync()
        {
            return await context.Pets.Include(p => p.User).OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<IEnumerable<Pet>> ListAsyncForUser(int userId)
        {
            return await context.Pets.Where(p => p.UserId == userId).OrderBy(p => p.Id).ToListAsync();
        }

        public void Put(Pet pet)
        {
            context.Pets.Update(pet);
        }
    }
}
