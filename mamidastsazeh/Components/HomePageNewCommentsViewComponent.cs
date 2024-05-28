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
    public class HomePageNewCommentsViewComponent : ViewComponent
    {
        public IPostCommentRepository _postComments;
        public IHashId _hashId;
       
        public HomePageNewCommentsViewComponent(IPostCommentRepository postComments,
            IHashId hashId)
        {
            _postComments = postComments;
            _hashId = hashId;
        }

        public IViewComponentResult Invoke()
        {
            try
            {
                var comments = _postComments.PostComments.Where(c => c.CommentState==CommentState.Approved ).OrderByDescending(c=>c.CommentDate).Take(5);
                return View(comments);

            }
            catch (Exception e)
            {
                return View("Empty");
            }
        }
    }
}
