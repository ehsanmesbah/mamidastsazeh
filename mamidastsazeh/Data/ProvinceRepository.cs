using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Data;
namespace mamidastsazeh.Data
{
    public class ProvinceRepository: IProvinceRepository
    {
        private readonly mamidastsazehContext _context;
        public ProvinceRepository(mamidastsazehContext context) 
        {
            _context = context;
        }

        public IQueryable<Province> Provinces => _context.Province;
        
      
    }
}
