using System.Threading.Tasks;
using VPets.Persistence.Contexts;

namespace VPets.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CompleteAsync()
        {
            // Apply repository changes to data store.
            await context.SaveChangesAsync();
        }
    }
}
