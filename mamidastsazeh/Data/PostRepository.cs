using mamidastsazeh.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;

namespace mamidastsazeh.Data
{
    public class PostRepository : IPostRepository
    {
        private  mamidastsazehContext _context;
        public PostRepository(mamidastsazehContext context) {
            _context = context;

        }
        public IQueryable<Post> Posts => _context.Post;
        

        public IQueryable<Post> GetFeaturedPosts(int limit)
        {
            return _context.Post.Include(v=>v.Category)
                .OrderByDescending(v => v.Created).Take(limit);
        }

        public IQueryable<Post> GetPostsByCategory(Category category)
        {
            return _context.Post
                .Include(p=>p.Category)
                .OrderByDescending(v =>v.Category.Id==category.Id);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void Add(Post post)
        {
             _context.Add(post);

        }
        
    }
}
