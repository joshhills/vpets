using System;
using VPets.Domain.Models;
using VPets.Persistence.Repositories;
using VPets.Services;
using VPetsUnitTests.Mocks;
using Xunit;

namespace VPetsUnitTests
{
    public class UserServiceTest
    {
        private readonly UserRepository userRepository;
        private readonly UserService userService;

        private readonly DateTime seedTime;

        public UserServiceTest()
        {
            // Bootstrap services with mocks (explicit as opposed to DI)
            var dbContext = AppDbContextMock.GetAppDbContext();
            userRepository = new UserRepository(dbContext);
            userService = new UserService(userRepository, new UnitOfWork(dbContext));

            seedTime = DateTime.Now; 

            // Seed data
            dbContext.Add(new User { Id = 1, Name = "Josh" });
            dbContext.SaveChangesAsync().Wait();
        }

        [Fact]
        public async void TestGetUserAsync()
        {
            var user = await userService.GetAsync(1);

            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
            Assert.Equal("Josh", user.Name);
            Assert.True(user.DateCreated > seedTime);
        }
    }
}
