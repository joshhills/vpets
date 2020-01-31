using VPets.Persistence.Contexts;

namespace VPets.Persistence.Repositories
{
    /// <summary>
    /// Useful to divvy up data store between models.
    /// </summary>
    public abstract class BaseRepository
    {
        protected readonly AppDbContext context;

        public BaseRepository(AppDbContext context)
        {
            this.context = context;
        }
    }
}
