﻿using System;
using VPets.Persistence.Contexts;

namespace VPets.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext context;

        public BaseRepository(AppDbContext context)
        {
            this.context = context;
        }
    }
}