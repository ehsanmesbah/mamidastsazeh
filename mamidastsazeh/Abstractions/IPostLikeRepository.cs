using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Abstractions
{
    public interface IPostLikeRepository
    {
        IQueryable<PostLike> PostLikes { get; }
        void SaveChanges();
        void Add(PostLike postLike);
        void Remove(PostLike postLike);
    }
}
