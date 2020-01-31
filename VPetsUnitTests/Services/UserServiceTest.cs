using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VPets.Domain.Models;
using VPets.Persistence.Contexts;
using VPets.Persistence.Repositories;
using VPets.Services;
using VPetsUnitTests.Mocks;
using Xunit;

namespace VPetsUnitTests.Services
{
    public class UserServiceTest
    {

        [Fact]
        public async void TestGetUserAsync()
        {
            // Arrange
            AppDbContext appDbContext = AppDbContextMock.GetAppDbContext();
            UserRepository userRepository = new UserRepository(appDbContext);
            UserService userService = new UserService(userRepository, new UnitOfWork(appDbContext));
            appDbContext.Users.Add(new User { Id = 100, Name = "Josh" });
            appDbContext.SaveChangesAsync().Wait();

            // Act
            var user = await userService.GetAsync(100);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(100, user.Id);
            Assert.Equal("Josh", user.Name);
        }

        [Fact]
        public async void TestListUsersAsync()
        {
            // Arrange
            AppDbContext appDbContext = AppDbContextMock.GetAppDbContext();
            UserRepository userRepository = new UserRepository(appDbContext);
            UserService userService = new UserService(userRepository, new UnitOfWork(appDbContext));
            appDbContext.Users.Add(new User { Id = 105, Name = "Johnny" });
            appDbContext.SaveChangesAsync().Wait();

            // Act
            var users = await userService.ListAsync();

            // Assert
            Assert.True(users.ToList().Count > 0);
        }

        [Fact]
        public async void TestDeleteUserAsync()
        {
            // Arrange
            AppDbContext appDbContext = AppDbContextMock.GetAppDbContext();
            UserRepository userRepository = new UserRepository(appDbContext);
            UserService userService = new UserService(userRepository, new UnitOfWork(appDbContext));
            appDbContext.Add(new User { Id = 110, Name = "Daniel" });
            await appDbContext.SaveChangesAsync();

            // Act
            var userDeleted = await userService.DeleteAsync(110);
            var findUser = await userService.GetAsync(110);

            // Assert
            Assert.NotNull(userDeleted);
            Assert.Equal("Daniel", userDeleted.Name);
            Assert.Null(findUser);
        }

        [Fact]
        public async void TestCreateUserAsync()
        {
            // Arrange
            AppDbContext appDbContext = AppDbContextMock.GetAppDbContext();
            UserRepository userRepository = new UserRepository(appDbContext);
            UserService userService = new UserService(userRepository, new UnitOfWork(appDbContext));

            // Act
            var user = await userService.CreateAsync(new User { Name = "Alasdair" });

            // Assert
            Assert.NotNull(user);
        }
    }
}
