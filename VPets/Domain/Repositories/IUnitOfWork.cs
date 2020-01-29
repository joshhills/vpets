using System;
using System.Threading.Tasks;

namespace VPets.Persistence.Repositories
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
