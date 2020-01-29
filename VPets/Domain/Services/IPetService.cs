﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPets.Domain.Models;

namespace VPets.Domain.Services
{
    public interface IPetService
    {
        Task<IEnumerable<Pet>> ListAsync();

        Task<Pet> CreateAsync(Pet pet);
    }
}