using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Abstractions;
using System.Security.Policy;
using mamidastsazeh.Models;

namespace mamidastsazeh.Controllers
{
    public class CustomizedTypeController : Controller
    {
        private readonly IHashId _hashId;
        private readonly ICategoryRepository _categories;
        private readonly IPostRepository _posts;

        public CustomizedTypeController(ICategoryRepository categories,IPostRepository posts,IHashId hashId)
        {
            _categories = categories;
            _hashId = hashId;
            _posts = posts;
        }
        public IActionResult Index( string categoryName, string hashId = "1", int page = 1)
        {
            
            if (_hashId.Decode(hashId) == 1) { 
               

                var posts = _posts.Posts.Where(p => p.ContentType.ID== 2 && p.Poststate == PostState.Approved).OrderByDescending(p => p.NumberOfViews).Skip(20 * (page - 1)).Take(20);
                int postcount = _posts.Posts.Where(p => p.ContentType.ID == 2 && p.Poststate == PostState.Approved).Count();
                if (postcount % 20 > 0)
                {
                    ViewData["TotalPage"] = ((int)(postcount / 20)) + 1;
                }
                else ViewData["TotalPage"] = (int)(postcount / 20);

                ViewData["FinalPage"] = "false";
                if (posts.Count() < 20) ViewData["FinalPage"] = "true";


                ViewData["CategoryId"] = 1;
                ViewData["CategoryName"] = "ویدیو";
                ViewData["PageNumber"] = page;
                return View(posts);
            }
            return View("Empty");
        }
    }
}
