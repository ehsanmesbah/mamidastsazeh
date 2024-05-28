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
    public class PostSwiperTitleViewComponent: ViewComponent
    {


        private ICategoryRepository _categories;


        public PostSwiperTitleViewComponent(ICategoryRepository categories)
        {
            _categories = categories;
        }
        public IViewComponentResult Invoke(string action, string controller,  string id, string title
            )
        {
            if (title=="-")
            {
                title = _categories.Categories.Where(c => c.Id == System.Convert.ToInt32(id)).FirstOrDefault().Name;
            }
            PostSwiperTitleViewModel postswipertitle = new PostSwiperTitleViewModel();
            postswipertitle.Action = action;
            postswipertitle.Controller = controller;
            postswipertitle.Title = title;
            postswipertitle.Id = id;
            return View(postswipertitle);
        }
    }
}
