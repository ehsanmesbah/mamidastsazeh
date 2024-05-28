using mamidastsazeh.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.ViewModels;
using mamidastsazeh.Models;

namespace mamidastsazeh.Components
{
    public class PostRouteViewComponent: ViewComponent
    {

        private readonly IPostRepository _posts;
        private readonly ICategoryRepository _categories;
        
        public PostRouteViewComponent(IPostRepository posts,ICategoryRepository categories)
        {
            _posts = posts;
            _categories = categories;
        }
        public IViewComponentResult Invoke(int postId)
        {
            var post = _posts.Posts.Where(p => p.ID == postId ).First();
            if (post != null)
            {
                var category = _categories.Categories.Where(c => c.Id == post.Category.Id).First();
                if (category != null)
                {
                    ViewBag.categoryType = post.PostTypeID;
                    return View(category);

                }
                else return View("Empty");
            }
            return View("Empty");
        }
    }
}
