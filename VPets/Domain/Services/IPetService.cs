using System.Collections.Generic;
using System.Threading.Tasks;
using VPets.Domain.Models;
using static VPets.Domain.Models.Metric;

namespace VPets.Domain.Services
{
    /// <summary>
    /// CRUD and other mutative operations on Pets.
    /// </summary>
    public interface IPetService
    {
        Task<Pet> GetAsync(int id);

        Task<IEnumerable<Pet>> ListAsync();

        Task<IEnumerable<Pet>> ListAsyncForUser(int userId);

        Task<Pet> CreateAsync(Pet pet);

        Task<Pet> DeleteAsync(int id);

        /// <summary>
        /// Improve a metric on a specific Pet.
        /// </summary>
        /// <param name="id">The unique Id of a Pet</param>
        /// <param name="onMetric">The Metric to operate on</param>
        /// <returns>The Pet once the task is complete</returns>
        Task<Pet> InteractAsync(int id, MetricType onMetric);

        /// <summary>
        /// Apply the Degrade method to all Metrics on all Pets
        /// in the data store.
        /// </summary>
        /// <returns>A Task representing the state of the operation</returns>
        Task DegradeMetrics();
    }
}
