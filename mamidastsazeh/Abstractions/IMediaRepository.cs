using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Abstractions
{
    public interface IMediaRepository
    {
        IQueryable<Media> Medias { get; }
        void SaveChanges();
        void Add(Media media);
    }
}
