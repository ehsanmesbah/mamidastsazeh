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
    public class SpecialPost : Controller
    {
        private readonly IHashId _hashId;
        private readonly ICategoryRepository _categories;
        private readonly IPostRepository _posts;

        public SpecialPost(IPostRepository posts)
        {
            _posts = posts;
        }
        public IActionResult Index( string pageType, int pageNumber=1)
        {
            int numberOfPost = 72;
            IEnumerable<Post> posts=null;
            pageType = pageType.ToLower();
            int postcount= _posts.Posts.Where(p => p.Poststate==PostState.Approved && p.PostTypeID!=5).Count()  ;
            if(pageType == "mostview") postcount = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.PostTypeID!=5 && p.Created>DateTime.Now.AddDays(-30)).Count();
            if (pageType == "mostviewproduct") postcount = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.PostTypeID == 5 && p.Created > DateTime.Now.AddDays(-30)).Count();
            if (pageType == "product") postcount = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.PostTypeID==5).Count();
            if (postcount % numberOfPost > 0)
            {
                ViewData["TotalPage"] = ((int)(postcount / numberOfPost)) + 1;
            }
            else ViewData["TotalPage"] = (int)(postcount / numberOfPost);
            if (pageType == "mostlike")
            {
                
                posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.PostTypeID != 5).OrderByDescending(p=>p.NumberOfLikes).Skip(numberOfPost * (pageNumber - 1)).Take(numberOfPost);
            }
            else if (pageType == "new")
            {

                posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.PostTypeID != 5 && p.CategoryId!=136).OrderByDescending(p => p.Created).Skip(numberOfPost * (pageNumber - 1)).Take(numberOfPost);
            }
            else if (pageType == "mostview")
            {
                posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.PostTypeID != 5 && p.Created>DateTime.Now.AddDays(-30)).OrderByDescending(p => p.NumberOfViews).Skip(numberOfPost * (pageNumber - 1)).Take(numberOfPost);
            }
            else if (pageType == "mostviewproduct")
            {
                posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.PostTypeID == 5 && p.Created > DateTime.Now.AddDays(-30)).OrderByDescending(p => p.NumberOfViews).Skip(numberOfPost * (pageNumber - 1)).Take(numberOfPost);
            }
            else if (pageType == "product")
            {
                posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.PostTypeID==5).OrderByDescending(p => p.Created).Skip(numberOfPost * (pageNumber - 1)).Take(numberOfPost);
            }
            else if (pageType == "mostcomment")
            {
                posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.PostTypeID != 5).OrderByDescending(p => p.NumberOfComments).Skip(numberOfPost * (pageNumber - 1)).Take(numberOfPost);
            }
            ViewData["FinalPage"] = "false";
            if (posts.Count() < numberOfPost) ViewData["FinalPage"] = "true";
            ViewData["pageType"] = pageType;
            ViewData["PageNumber"] = pageNumber;
            return View(posts);
        }
    }
}
