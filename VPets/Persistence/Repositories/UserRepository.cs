using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VPets.Domain.Models;
using VPets.Persistence.Contexts;

namespace VPets.Persistence.Repositories
{
    /// <summary>
    /// I/O on the databsae context for User table.
    /// </summary>
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task CreateAsync(User user)
        {
            await context.Users.AddAsync(user);
        }

        public void Delete(User user)
        {
            context.Users.Remove(user);
        }

        public async Task<User> GetAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await context.Users.ToListAsync();
        }
    }
}
