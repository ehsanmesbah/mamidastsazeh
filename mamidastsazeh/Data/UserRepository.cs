using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class UserRepository:IUserRepository
    {

        private readonly mamidastsazehContext _context;
        public IQueryable<User> Users => _context.User;
        public UserRepository(mamidastsazehContext context)
        {
            _context = context;
        }
        public int SaveChanges()
        {
           return _context.SaveChanges();
        }
    }
}
