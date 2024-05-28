using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public enum PostState
    {
        UnSet=0,
        New = 1,
        Edited = 2,
        Approved = 3,
        Rejected = 4,
        AskForEdit = 5,
        DeletedByAdmin=6,
        DeletedByUser=7,
        SuspendedAccount=8
    };
    
    public class Post
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
       
        public PostState Poststate { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int? ContentTypeID { get; set; }
        public virtual ContentType ContentType { get; set; }
        public long? instapostid { get; set; }

        public bool? istitlecorrect { get; set; }

        public int? NumberOfLikes { get; set; }
        public int? NumberOfComments { get; set; }
        public int? NumberOfViews { get; set; }
        public int? NumberOfLikesinsta { get; set; }
        public int? NumberOfCommentsinsta { get; set; }
        public int? NumberOfViewsinsta { get; set; }
        public string FileName { get; set; }
        public string Thumbnail { get; set; }
        [UIHint("date")]
        public DateTime Created { get; set; }


        public int? PostTypeID { get; set; }
        
        public int? Price { get; set; }
        public string SendType { get; set; }
        public int? DiscountPrice { get; set; }
        
     
        
        public virtual PostType PostType { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
        

        public virtual ICollection<Media> Medias { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual ICollection<PostView> PostViews { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }

        public string RawPostTags { get; set; }
        public string AdminView { get; set; }
        
        public virtual ICollection<MamiEvent> MamiEvents { get; set; }

    }
}
