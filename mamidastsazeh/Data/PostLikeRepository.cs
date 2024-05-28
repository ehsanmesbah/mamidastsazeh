using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class PostLikeRepository : IPostLikeRepository
    {
        private readonly mamidastsazehContext _context;
        public IQueryable<PostLike> PostLikes => _context.PostLike;

        public PostLikeRepository(mamidastsazehContext context)
        {
            _context = context;
        }
        public void SaveChanges(){
            _context.SaveChanges();
        }
        public void Add(PostLike postLike)
        {
            _context.Add(postLike);
        }
        public void Remove(PostLike postLike)
        {
            _context.Remove(postLike);
        }
    }
}
