using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class SmsTokenRepository:IsmsTokenRepository
    {

        private readonly mamidastsazehContext _context;
        public IQueryable<SmsToken> SmsTokens => _context.SmsToken;
        

        public SmsTokenRepository(mamidastsazehContext context)
        {
            _context = context;
        }
        public int SaveChanges()
        {
           return _context.SaveChanges();
        }
        public void Add(SmsToken entity)
        {
            _context.Add(entity);
        }
    }
}
