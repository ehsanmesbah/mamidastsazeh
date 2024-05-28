using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Data;
namespace mamidastsazeh.Data
{
    public class MainCategoryRepository: IMainCategoryRepository
    {
        private readonly mamidastsazehContext _context;
        public MainCategoryRepository(mamidastsazehContext context) 
        {
            _context = context;
        }

        public IQueryable<MainCategory> MainCategories => _context.MainCategory;
       
    }
}
