using mamidastsazeh.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Models;
using mamidastsazeh.ViewModels;

namespace mamidastsazeh.Components
{
    public class VideoPlayerViewComponent : ViewComponent
    {
        private readonly IPostRepository _posts;

        public VideoPlayerViewComponent(IPostRepository posts)
        {
            _posts = posts;
        }

        public IViewComponentResult Invoke(int postId)
        {
            try
            {
                var video = _posts.Posts.Where(p => p.ID == postId )
                    .First();

                return View(video);
            }
            catch (InvalidOperationException)
            {
                return View("Empty");
            }

        }
    }
}
