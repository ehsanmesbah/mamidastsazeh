using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Models;

namespace mamidastsazeh.Abstractions
{
    public interface IPostTypeRepository
    {
        IQueryable<PostType> PostTypes { get; }
    }
}
