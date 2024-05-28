using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Abstractions
{
    public interface IPostCommentRepository
    {
        IQueryable<PostComment> PostComments { get; }
        void SaveChanges();
        void Add(PostComment postComment);
        
    }
}
