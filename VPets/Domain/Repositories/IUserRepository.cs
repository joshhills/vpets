using System.Collections.Generic;
using System.Threading.Tasks;
using VPets.Domain.Models;

namespace VPets.Persistence.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> ListAsync();

        Task<User> GetAsync(int id);

        Task CreateAsync(User user);

        void Delete(User user);
    }
}
