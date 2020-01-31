using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPets.Domain.Models;
using VPets.Domain.Repositories;
using VPets.Domain.Services;
using VPets.Persistence.Repositories;

namespace VPets.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository petRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public PetService(IPetRepository petRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.petRepository = petRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Pet> CreateAsync(Pet pet)
        {
            try
            {
                // Ensure Pet exists to be linked with...
                var existingUser = await userRepository.GetAsync(pet.UserId);
                if (existingUser == null)
                {
                    return null;
                }

                await petRepository.CreateAsync(pet);
                await unitOfWork.CompleteAsync();

                return pet;
            }
            catch (Exception)
            {
                // TODO: Better error handling!
                return null;
            }
        }

        public async Task<Pet> GetAsync(int id)
        {
            return await petRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Pet>> ListAsync()
        {
            return await petRepository.ListAsync();
        }

        public async Task<Pet> DeleteAsync(int id)
        {
            // Find the Pet to delete...
            var existingPet = await petRepository.GetAsync(id);

            if (existingPet == null)
            {
                return null;
            }

            try
            {
                petRepository.Delete(existingPet);
                await unitOfWork.CompleteAsync();

                return existingPet;
            }
            catch (Exception)
            {
                // TODO: Better error handling!
                return null;
            }
        }

        public async Task<Pet> InteractAsync(int id, Metric.MetricType onMetric)
        {
            var existingPet = await petRepository.GetAsync(id);

            if (existingPet == null)
            {
                return null;
            }

            try
            {
                existingPet.Metrics.TryGetValue(onMetric, out Metric metric);
                metric.Improve();

                petRepository.Put(existingPet);
                await unitOfWork.CompleteAsync();

                return existingPet;
            } catch (Exception)
            {
                // TODO: Better error handling!
                return null;
            }
        }

        public async Task DegradeMetrics()
        {
            var pets = await ListAsync();

            // Not very efficient...
            foreach (var pet in pets)
            {
                foreach (var metric in pet.Metrics)
                {
                    metric.Value.Degrade();
                }
                petRepository.Put(pet);
            }
        }

        public async Task<IEnumerable<Pet>> ListAsyncForUser(int userId)
        {
            return await petRepository.ListAsyncForUser(userId);
        }
    }
}
