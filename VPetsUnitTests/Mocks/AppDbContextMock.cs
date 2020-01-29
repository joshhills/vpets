using System;
using Microsoft.EntityFrameworkCore;
using VPets.Persistence.Contexts;

namespace VPetsUnitTests.Mocks
{
    public class AppDbContextMock
    {
        public static AppDbContext GetAppDbContext()
        {
            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "vpets-in-memory-utest")
                .Options;

            // Create instance of DbContext
            var dbContext = new AppDbContext(options);

            return dbContext;
        }
    }
}
