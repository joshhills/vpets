using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPets.Domain.Models;
using VPets.Persistence.Repositories;

namespace VPets.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<User> CreateAsync(User user)
        {
            try
            {
                await userRepository.CreateAsync(user);
                await unitOfWork.CompleteAsync();

                return user;
            } catch (Exception)
            {
                // TODO: Better error handling!
                return null;
            }
        }

        public async Task<User> DeleteAsync(int id)
        {
            // Find User to delete...
            var existingUser = await userRepository.GetAsync(id);

            if (existingUser == null)
            {
                return null;
            }

            try
            {
                userRepository.Delete(existingUser);
                await unitOfWork.CompleteAsync();

                return existingUser;
            } catch (Exception)
            {
                // TODO: Better error handling!
                return null;
            }
        }

        public async Task<User> GetAsync(int id)
        {
            return await userRepository.GetAsync(id);
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await userRepository.ListAsync();
        }
    }
}
