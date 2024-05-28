using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

namespace mamidastsazeh.Abstractions
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }

    

    }
}
