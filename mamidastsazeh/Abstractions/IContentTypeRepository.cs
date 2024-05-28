using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Models;

namespace mamidastsazeh.Abstractions
{
    public interface IContentTypeRepository
    {
        IQueryable<ContentType> ContentTypes { get; }
    }
}
