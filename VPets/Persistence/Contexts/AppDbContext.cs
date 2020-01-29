using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VPets.Models;

namespace VPets.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<User>().HasKey(p => p.Id);
            builder.Entity<User>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<User>().Property(p => p.DateCreated).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<User>().Property(p => p.Name).IsRequired().HasMaxLength(32);

            builder.Entity<User>().HasData(new User { Id = 1, Name = "Josh", DateCreated = DateTime.Now });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            DateTime saveTime = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == (EntityState) EntityState.Added))
            {
                if (entry.Property("DateCreated").CurrentValue == null)
                    entry.Property("DateCreated").CurrentValue = saveTime;
            }
            return await base.SaveChangesAsync();
        }
    }
}
