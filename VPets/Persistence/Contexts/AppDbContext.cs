﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VPets.Domain.Models;
using VPets.Domain.Models.Pets;
using static VPets.Domain.Models.Metric;

namespace VPets.Persistence.Contexts
{
    /// <summary>
    /// Bootstrap ORM and seed data.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// The column in the data store corresponding to the DateCreated
        /// field on Entity.
        /// </summary>
        private const string DATE_CREATED_PROPERTY_NAME = "DateCreated";

        public DbSet<User> Users { get; set; }
        public DbSet<Pet> Pets { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<User>().Property(u => u.DateCreated).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<User>().Property(u => u.Name).IsRequired().HasMaxLength(32);
            builder.Entity<User>().HasMany(u => u.Pets).WithOne(p => p.User).HasForeignKey(p => p.UserId);

            builder.Entity<Cat>().HasBaseType<Pet>();
            builder.Entity<Dog>().HasBaseType<Pet>();

            builder.Entity<Pet>().ToTable("Pets");
            builder.Entity<Pet>().HasKey(p => p.Id);
            builder.Entity<Pet>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Pet>().Property(p => p.DateCreated).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Pet>().Property(p => p.Name).IsRequired().HasMaxLength(32);
            builder.Entity<Pet>().Property(p => p.Type).IsRequired();
            // Serialise Metrics i.e. game-data as string in column - temporary solution!
            // Would probably use a separate table or NoSQL
            builder.Entity<Pet>().Property(p => p.Metrics).IsRequired().HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => v == null
                    ? new Dictionary<MetricType, Metric>()
                    : JsonConvert.DeserializeObject<Dictionary<MetricType, Metric>>(v));

            Seed(builder);
        }

        private void Seed(ModelBuilder builder)
        {
            var dummyUser = new User { Id = 1, Name = "Josh", DateCreated = DateTime.Now };

            builder.Entity<User>().HasData(dummyUser);
        }

        /// <summary>
        /// Override to saving changes that establishes a DateCreated field on
        /// entities.
        /// </summary>
        /// <param name="cancellationToken">Whether the operation should stop</param>
        /// <returns>Response code</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            DateTime saveTime = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
            {
                if (entry.Property(DATE_CREATED_PROPERTY_NAME).CurrentValue == null)
                    entry.Property(DATE_CREATED_PROPERTY_NAME).CurrentValue = saveTime;
            }

            return await base.SaveChangesAsync();
        }
    }
}
