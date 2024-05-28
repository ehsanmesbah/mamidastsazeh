using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class RestrictedWordRepository:IRestrictedWordRepository
    {
        private readonly mamidastsazehContext _context;
        public RestrictedWordRepository(mamidastsazehContext context)
        {
            _context = context;
        }
        public IQueryable<RestrictedWord> RestrictedWords => _context.RestirictedWord;
       
    }
}
