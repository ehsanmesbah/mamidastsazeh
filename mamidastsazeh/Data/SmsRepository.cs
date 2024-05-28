using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class SmsRepository:IsmsRepository
    {

        private readonly mamidastsazehContext _context;
        public IQueryable<Sms> Smss => _context.Sms;
        

        public SmsRepository(mamidastsazehContext context)
        {
            _context = context;
        }
        public int SaveChanges()
        {
           return _context.SaveChanges();
        }
        public void Add(Sms entity)
        {
            _context.Add(entity);
        }
    }
}
