﻿using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Abstractions
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        public int SaveChanges();
    }

}
