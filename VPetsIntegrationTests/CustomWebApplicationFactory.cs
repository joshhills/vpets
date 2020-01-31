using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VPets;
using VPets.Domain.Models;
using VPets.Domain.Models.Pets;
using VPets.Persistence.Contexts;

namespace VPetsIntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context (AppDbContext) using an in-memory database for testing.
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("vpets-in-memory-itest");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database contexts
                using var scope = sp.CreateScope();

                var scopedServices = scope.ServiceProvider;
                var appDb = scopedServices.GetRequiredService<AppDbContext>();

                var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                // Ensure the database is created.
                appDb.Database.EnsureCreated();

                try
                {
                    // Seed the database with some specific test data.
                    Seed(appDb);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                                        "database with test messages. Error: {ex.Message}");
                }
            });
        }

        private void Seed(AppDbContext appDb)
        {
            appDb.Add(new User { Id = 2, Name = "Daniel" });
            appDb.Add(new Cat { Id = 100, UserId = 1, Name = "Meowser" });
            appDb.Add(new Dog { Id = 101, UserId = 1, Name = "Woofus" });

            appDb.SaveChangesAsync().Wait();
        }
    }
}
