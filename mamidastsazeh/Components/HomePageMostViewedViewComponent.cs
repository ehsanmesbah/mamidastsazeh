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
    public class HomePageMostViewedViewComponent : ViewComponent
    {
        public IPostRepository _posts;
        public IHashId _hashId;
       
        public HomePageMostViewedViewComponent(IPostRepository post,
            IHashId hashId)
        {
            _posts = post;
            _hashId = hashId;
        }

        public IViewComponentResult Invoke()
        {
            try
            {
                var posts = _posts.Posts.Where(p => p.Poststate==PostState.Approved && p.Created>DateTime.Now.AddDays(-7)).OrderByDescending(p=>p.NumberOfViews).Take(5);
                return View(posts);

            }
            catch (Exception e)
            {
                return View("Empty");
            }
        }
    }
}
