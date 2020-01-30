using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VPets.Domain.Models;
using VPets.Domain.Repositories;
using VPets.Persistence.Contexts;

namespace VPets.Persistence.Repositories
{
    public class PetRepository : BaseRepository, IPetRepository
    {
        public PetRepository(AppDbContext context) : base(context)
        {
        }

        public async Task CreateAsync(Pet pet)
        {
            await context.Pets.AddAsync(pet);
        }

        public async Task<Pet> GetAsync(int id)
        {
            return await context.Pets.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pet>> ListAsync()
        {
            return await context.Pets.Include(p => p.User).ToListAsync();
        }
    }
}
