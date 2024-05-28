using mamidastsazeh.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Models;
using mamidastsazeh.Abstractions;
namespace mamidastsazeh.Data
{
    public class ContentTypeRepository:IContentTypeRepository
    {
        private readonly mamidastsazehContext _context;
        public ContentTypeRepository(mamidastsazehContext context)
        {
            _context = context;
        }
        public IQueryable<ContentType> ContentTypes => _context.ContentType;
    }
}
