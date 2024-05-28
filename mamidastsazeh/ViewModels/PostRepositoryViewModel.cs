using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Models;
namespace mamidastsazeh.ViewModels
{
    public class PostRepositoryViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
    }
}
