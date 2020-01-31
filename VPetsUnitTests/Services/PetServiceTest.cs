using System;
using System.Linq;
using VPets.Domain.Models;
using VPets.Domain.Models.Pets;
using VPets.Persistence.Contexts;
using VPets.Persistence.Repositories;
using VPets.Services;
using VPetsUnitTests.Mocks;
using Xunit;
using static VPets.Domain.Models.Metric;

namespace VPetsUnitTests.Services
{
    public class PetServiceTest
    {
        [Fact]
        public async void TestGetPetAsync()
        {
            // Arrange
            AppDbContext appDbContext = AppDbContextMock.GetAppDbContext();
            PetRepository petRepository = new PetRepository(appDbContext);
            UserRepository userRepository = new UserRepository(appDbContext);
            PetService petService = new PetService(petRepository, userRepository, new UnitOfWork(appDbContext));
            appDbContext.Users.Add(new User { Id = 200, Name = "Owen" });
            appDbContext.Pets.Add(new Dog { Id = 100, Name = "Woofus", UserId = 200 });
            appDbContext.SaveChangesAsync().Wait();

            // Act
            var pet = await petService.GetAsync(100);

            // Assert
            Assert.NotNull(pet);
            Assert.Equal(100, pet.Id);
            Assert.Equal("Woofus", pet.Name);
        }

        [Fact]
        public async void TestListPetsAsync()
        {
            // Arrange
            AppDbContext appDbContext = AppDbContextMock.GetAppDbContext();
            PetRepository petRepository = new PetRepository(appDbContext);
            UserRepository userRepository = new UserRepository(appDbContext);
            PetService petService = new PetService(petRepository, userRepository, new UnitOfWork(appDbContext));
            appDbContext.Users.Add(new User { Id = 205, Name = "Eva" });
            appDbContext.Pets.Add(new Dog { Id = 105, Name = "Meowser", UserId = 205 });
            appDbContext.SaveChangesAsync().Wait();

            // Act
            var pets = await petService.ListAsync();

            // Assert
            Assert.True(pets.ToList().Count > 0);
        }

        [Fact]
        public async void TestDeletePetAsync()
        {
            // Arrange
            AppDbContext appDbContext = AppDbContextMock.GetAppDbContext();
            PetRepository petRepository = new PetRepository(appDbContext);
            UserRepository userRepository = new UserRepository(appDbContext);
            PetService petService = new PetService(petRepository, userRepository, new UnitOfWork(appDbContext));
            appDbContext.Users.Add(new User { Id = 210, Name = "Jessica" });
            appDbContext.Add(new Cat { Id = 110, Name = "Mae", UserId = 210 });
            appDbContext.SaveChangesAsync().Wait();

            // Act
            var petDeleted = await petService.DeleteAsync(110);
            var findPet = await petService.GetAsync(110);

            // Assert
            Assert.NotNull(petDeleted);
            Assert.Equal("Mae", petDeleted.Name);
            Assert.Null(findPet);
        }

        [Fact]
        public async void TestCreatePetAsync()
        {
            // Arrange
            AppDbContext appDbContext = AppDbContextMock.GetAppDbContext();
            PetRepository petRepository = new PetRepository(appDbContext);
            UserRepository userRepository = new UserRepository(appDbContext);
            PetService petService = new PetService(petRepository, userRepository, new UnitOfWork(appDbContext));
            appDbContext.Users.Add(new User { Id = 215, Name = "Michael" });
            appDbContext.SaveChangesAsync().Wait();

            // Act
            var pet = await petService.CreateAsync(new Cat { Name = "Clause", UserId = 215 });

            // Assert
            Assert.NotNull(pet);
        }

        [Fact]
        public async void TestInteractAsync()
        {
            // Arrange
            AppDbContext appDbContext = AppDbContextMock.GetAppDbContext();
            PetRepository petRepository = new PetRepository(appDbContext);
            UserRepository userRepository = new UserRepository(appDbContext);
            PetService petService = new PetService(petRepository, userRepository, new UnitOfWork(appDbContext));
            appDbContext.Users.Add(new User { Id = 220, Name = "Rachel" });
            var hungerAtTimeOfCreation = appDbContext.Add(new Dog { Id = 120, Name = "Harvey", UserId = 220 })
                .Entity.Metrics[MetricType.HUNGER].Value;
            appDbContext.SaveChangesAsync().Wait();

            // Act
            var newPetState = await petService.InteractAsync(120, MetricType.HUNGER);

            // Assert
            Assert.True(newPetState.Metrics[MetricType.HUNGER].Value < hungerAtTimeOfCreation);
        }

        [Fact]
        public async void TestDegradeMetrics()
        {
            // Arrange
            AppDbContext appDbContext = AppDbContextMock.GetAppDbContext();
            PetRepository petRepository = new PetRepository(appDbContext);
            UserRepository userRepository = new UserRepository(appDbContext);
            PetService petService = new PetService(petRepository, userRepository, new UnitOfWork(appDbContext));
            appDbContext.Users.Add(new User { Id = 225, Name = "Noel" });
            var happinessAtTimeOfCreation = appDbContext.Add(new Cat { Id = 125, Name = "Molly", UserId = 225 })
                .Entity.Metrics[MetricType.HAPPINESS].Value;
            appDbContext.SaveChangesAsync().Wait();

            // Act
            await petService.DegradeMetrics();
            var pet = await petService.GetAsync(125);

            // Assert
            Assert.True(pet.Metrics[MetricType.HAPPINESS].Value < happinessAtTimeOfCreation);
        }
    }
}
