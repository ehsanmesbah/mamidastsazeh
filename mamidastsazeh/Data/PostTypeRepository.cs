using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class PostTypeRepository:IPostTypeRepository
    {
        private readonly mamidastsazehContext _context;
        public IQueryable<PostType> PostTypes => _context.PostType;
        public PostTypeRepository(mamidastsazehContext context)
        {
            _context = context;
        }
       
    }
}
