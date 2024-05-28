using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Data;
namespace mamidastsazeh.Data
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly mamidastsazehContext _context;
        public CategoryRepository(mamidastsazehContext context) 
        {
            _context = context;
        }

        public IQueryable<Category> Categories => _context.Category;
        
      
    }
}
