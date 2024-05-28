using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class PostViewRepository:IPostViewRepository
    {

        private readonly mamidastsazehContext _context;
        public IQueryable<PostView> PostViews => _context.PostView;
        public PostViewRepository(mamidastsazehContext context) 
        {
            _context = context;
        }
    }
}
