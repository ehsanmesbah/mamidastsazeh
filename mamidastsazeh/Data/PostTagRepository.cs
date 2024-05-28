using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class PostTagRepository:IPostTagRepository
    {
        private readonly mamidastsazehContext _context;
        public IQueryable<PostTag> PostTags => _context.PostTag;

        public PostTagRepository(mamidastsazehContext context)
        {
            _context = context;

        }
    }
}
