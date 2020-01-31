using System.Threading.Tasks;

namespace VPets.Persistence.Repositories
{
    /// <summary>
    /// Create an abstraction layer between the data access layer and the
    /// business logic layer of the application using the 'Unit of Work' pattern.
    /// <see href="https://tinyurl.com/ybqfe5j5">Read More</see>
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Represent a way to finish a transaction.
        /// </summary>
        /// <returns>A Task representing the state of the operation</returns>
        Task CompleteAsync();
    }
}
