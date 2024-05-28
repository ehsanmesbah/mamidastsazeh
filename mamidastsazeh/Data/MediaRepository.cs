using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class MediaRepository:IMediaRepository
    {
        private readonly mamidastsazehContext _context;
        public MediaRepository(mamidastsazehContext context)
        {
            _context = context;
        }
        public IQueryable<Media> Medias => _context.Media;
        public void SaveChanges()
        {
            _context.SaveChanges();

        }
        public void Add(Media media)
        {
            _context.Add(media);
        }

    }
}
