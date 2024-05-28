using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class TagRepository:ITagRepository
    {

        private readonly mamidastsazehContext _context;
        public IQueryable<Tag> Tags => _context.Tag;
        public TagRepository(mamidastsazehContext context){
            _context = context;
        }
    }
}
