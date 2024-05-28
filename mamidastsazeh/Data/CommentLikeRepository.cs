using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class CommentLikeRepository : ICommentLikeRepository
    {
        private readonly mamidastsazehContext _context;
        public IQueryable<CommentLike> CommentLikes => _context.CommentLike;

        public CommentLikeRepository(mamidastsazehContext context)
        {
            _context = context;
        }
        public void SaveChanges(){
            _context.SaveChanges();
        }
        public void Add(CommentLike commentLike)
        {
            _context.Add(commentLike);
        }
        public void Remove(CommentLike commentLike)
        {
            _context.Remove(commentLike);
            
        }
        public void RemoveRange(IEnumerable<CommentLike> commentLikes)
        {
            _context.RemoveRange(commentLikes);
            

        }
    }
}
