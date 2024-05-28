using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using Microsoft.Extensions.Configuration;
using Castle.Core.Internal;

namespace mamidastsazeh.Components
{
    public class PostCommentsViewComponent : ViewComponent
    {
        public IPostCommentRepository _postComments;
        public IHashId _hashId;
       
        public PostCommentsViewComponent(IPostCommentRepository postComments,
            IHashId hashId)
        {
            _postComments = postComments;
            _hashId = hashId;
        }

        public IViewComponentResult Invoke(string postId, int skip, int limit)
        {
            try
            {
                int id=_hashId.Decode(postId);
                var comments = _postComments.PostComments.Where(c => c.PostId == id && (c.CommentState==CommentState.Approved || c.CommentState==CommentState.New)).OrderBy(c=>c.CommentDate).Skip(skip).Take(limit);
                ViewBag.Skip = skip + limit;
                ViewBag.Limit = limit;
                    
                if (comments.Count() > 0)
                {
                    return View(comments);
                }


                //var relatedPosts = _uow.Posts.Find(v => v.ID!=post.ID && v.PostTags.Select(vt=>vt.TagId).Count()>0);


                return View("Empty");

            }
            catch (InvalidOperationException)
            {
                return View("Empty");
            }
        }
    }
}
