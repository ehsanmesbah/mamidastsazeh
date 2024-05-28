using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Abstractions;
using System.Security.Policy;
using mamidastsazeh.Models;
using mamidastsazeh.Components;
using mamidastsazeh.ViewModels;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace mamidastsazeh.Controllers
{
    public class UserPostsController : Controller
    {
        private readonly IHashId _hashId;
        private readonly IPostRepository _posts;

        public UserPostsController(IPostRepository posts,IHashId hashId)
        {
            
            _hashId = hashId;
            _posts = posts;
        }
        public IActionResult Index(PostsPaginationViewModel model,
           [FromServices] IUserRepository _user)
        {
            
            if (model.Limit > 24) model.Limit = 24;
            var user = _user.Users.Where(u => u.NickName == model.NickName).FirstOrDefault();
            if (user != null)
            {
                model.User = user;
                var total = _posts.Posts.Where(p => p.User == user && p.Poststate == PostState.Approved).Count();
                var posts = _posts.Posts.Where(p => p.User == user && p.Poststate == PostState.Approved).AsQueryable().OrderBy(model.SortOrder.Replace("-", " ")).Skip((model.Page - 1) * model.Limit).Take(model.Limit);
                model.Posts = posts;
                model.Total = total;
                return View(model);
             
            }
            else return View("Empty");
      
        }
    }
}
