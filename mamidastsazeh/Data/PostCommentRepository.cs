using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class PostCommentRepository:IPostCommentRepository
    {
        private readonly mamidastsazehContext _context;
        public PostCommentRepository(mamidastsazehContext context)
        {
            _context = context;
        
       }
        public IQueryable<PostComment> PostComments => _context.PostComment;

        public void SaveChanges() {
            _context.SaveChanges();
        }
        public void Add(PostComment postComment)
        {
            _context.Add(postComment);
        }
    }
}
