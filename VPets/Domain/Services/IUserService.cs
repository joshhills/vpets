using System.Collections.Generic;
using System.Threading.Tasks;
using VPets.Domain.Models;

namespace VPets.Services
{
    public interface IUserService
    {
        Task<User> GetAsync(int id);

        Task<IEnumerable<User>> ListAsync();

        Task<User> CreateAsync(User user);

        Task<User> DeleteAsync(int id);
    }
}
