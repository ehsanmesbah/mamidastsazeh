using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using mamidastsazeh.Models;
namespace mamidastsazeh.Abstractions
{
    public interface IPostRepository 
    {
        IQueryable<Post> Posts { get;  }
        IQueryable<Post> GetFeaturedPosts(int limit);
        void SaveChanges();
        void Add(Post post);
        
    }
}
