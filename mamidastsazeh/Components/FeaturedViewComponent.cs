using mamidastsazeh.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.ViewModels;

namespace mamidastsazeh.Components
{
    public class FeaturedViewComponent:ViewComponent
    {
        private readonly IPostRepository _posts;
        public FeaturedViewComponent(IPostRepository posts)
        {
            _posts = posts;
        }
        public IViewComponentResult Invoke(int limit, int top)
        {
            var posts = _posts.GetFeaturedPosts(limit);
            var viewModel = new FeaturedViewModel(posts, top);

            return View(viewModel);
        }
    }
}
