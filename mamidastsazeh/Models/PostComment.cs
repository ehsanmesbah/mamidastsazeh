using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public enum CommentState
    {
        UnSet = 0,
        New = 1,
        Approved = 3,
        Rejected = 4,
        DeletedByAdmin = 6,
        DeletedByUser = 7,
        Reported=8,
        ContainRestricted=9,
        SuspendedAccount=10
    };
    public class PostComment
    {
        public long Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        
        public string Text { get; set; }
        public bool Approved { get; set; }
        public DateTime CommentDate { get; set; }
        public virtual  User User { get; set; }
        public virtual  Post Post { get; set; }

        public long? ParentCommentId { get; set; }
        public virtual PostComment ParentComment { get; set; }
        public virtual ICollection<CommentLike> CommentLikes { get; set; }
        public int NumberOfLikes { get; set; }
        public CommentState CommentState { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
