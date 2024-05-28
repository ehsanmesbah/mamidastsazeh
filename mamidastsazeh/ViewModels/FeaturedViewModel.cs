using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Models;

namespace mamidastsazeh.ViewModels
{
    public class FeaturedViewModel
    {
        public FeaturedViewModel(IEnumerable<Post> posts, int top)
        {
            TopPosts = posts.Take(top);
            MorePosts = posts.Skip(top);
        }

        public IEnumerable<Post> TopPosts { get; set; }
        public IEnumerable<Post> MorePosts { get; set; }
    }
}
