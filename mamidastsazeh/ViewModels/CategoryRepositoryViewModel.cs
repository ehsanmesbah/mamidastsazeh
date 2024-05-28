using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.ViewModels
{
    public class CategoryRepositoryViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}
