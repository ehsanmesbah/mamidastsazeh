using mamidastsazeh.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;

namespace mamidastsazeh.Data
{
    public class MamiEventRepository : IMamiEventRepository
    {
        private  mamidastsazehContext _context;
        public MamiEventRepository(mamidastsazehContext context) {
            _context = context;

        }
        public IQueryable<MamiEvent> MamiEvents => _context.MamiEvent;
        

       
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void Add(MamiEvent mamiEvent)
        {
             _context.Add(mamiEvent);

        }
        
    }
}
