using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Abstractions
{
    public interface ICommentLikeRepository
    {
        IQueryable<CommentLike> CommentLikes { get; }
        void SaveChanges();
        void Add(CommentLike commentLike);
        void Remove(CommentLike commentLike);
        void RemoveRange(IEnumerable<CommentLike> commentLikes);
    }
}
