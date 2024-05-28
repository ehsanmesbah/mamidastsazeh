using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public class CommentLike
    {
        public long Id { get; set; }
        public long PostCommentId { get; set; }
        public string UserId { get; set; }
        public virtual PostComment PostComment { get; set; }
        public virtual User User { get; set; }
        public DateTime LikeDate { get; set; }
    }
}
