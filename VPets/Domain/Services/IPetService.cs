using System.Collections.Generic;
using System.Threading.Tasks;
using VPets.Domain.Models;
using static VPets.Domain.Models.Metric;

namespace VPets.Domain.Services
{
    public interface IPetService
    {
        Task<Pet> GetAsync(int id);

        Task<IEnumerable<Pet>> ListAsync();

        Task<IEnumerable<Pet>> ListAsyncForUser(int userId);

        Task<Pet> CreateAsync(Pet pet);

        Task<Pet> DeleteAsync(int id);

        Task<Pet> InteractAsync(int id, MetricType onMetric);

        Task DegradeMetrics();
    }
}
