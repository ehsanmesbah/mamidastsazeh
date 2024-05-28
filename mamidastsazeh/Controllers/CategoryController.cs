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
using Microsoft.AspNetCore.Identity;

namespace mamidastsazeh.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IHashId _hashId;
        private readonly ICategoryRepository _categories;
        private readonly IPostRepository _posts;
        private readonly UserManager<User> _userManager;


        public CategoryController(ICategoryRepository categories,IPostRepository posts,IHashId hashId,UserManager<User> userManager)
        {
            _categories = categories;
            _hashId = hashId;
            _posts = posts;
            _userManager = userManager;
        }
        public IActionResult Index(PostsPaginationViewModel model)
        {
            var categoryId = _hashId.Decode(model.CategoryHashId);
            /* if (categoryId == 136)
             {
                 var user =  _userManager.GetUserName(HttpContext.User);
                 if (string.IsNullOrEmpty(user))
                 {
                     var returnUrl = "/category/" + model.CategoryHashId + "/category/1/Created-desc";                   
                     return RedirectToAction("RequireLogin", "Account", new { area = "Membership",ReturnUrl=returnUrl }) ;
                 }
             }*/
         

            if (string.IsNullOrEmpty( model.PageType))
            {
                model.PageType = "category";
            }
            if (string.IsNullOrEmpty(model.SortOrder))
            {
                model.PageType = "Created-desc";
            }
           
            if (model.Limit > 48) model.Limit = 48;
            
            var cat = _categories.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
            if (cat==null)
            {
                Response.StatusCode = 404;
                return View("Error");
            }
            else
            {
                int total;
                List<Post> posts = new List<Post>();
                if (model.CategoryType == "Product")
                {
                    total = _posts.Posts.Where(p => p.Category.Id == categoryId && p.Poststate == PostState.Approved && p.PostTypeID == 5).Count();
                    posts = _posts.Posts.Where(p => p.Category.Id == categoryId && p.Poststate == PostState.Approved && p.PostTypeID == 5).AsQueryable().OrderBy(model.SortOrder.Replace("-", " ")).Skip((model.Page - 1) * model.Limit).Take(model.Limit).ToList();
                }
                else
                {
                    total = _posts.Posts.Where(p => p.Category.Id == categoryId && p.Poststate == PostState.Approved && p.PostTypeID != 5).Count();
                    posts = _posts.Posts.Where(p => p.Category.Id == categoryId && p.Poststate == PostState.Approved && p.PostTypeID != 5).AsQueryable().OrderBy(model.SortOrder.Replace("-", " ")).Skip((model.Page - 1) * model.Limit).Take(model.Limit).ToList();
                }
                model.Posts = posts;
                model.Total = total;
                model.CategoryName = cat.Name;
                ViewBag.Title = "مامی دست سازه آموزش های " + cat.Name;
                ViewBag.Description = "mamidastsazeh ، مامی دست سازه ، مر جع آموزش هنری دست سازه ساز جهت بانوان مادران رنگ آمیزی دکوپاژ دانلود دکوری تزئینات کاردستی عید دانلود تم تولد" + cat.Name;
               // ViewBag.URL = "https://mamidastsazeh.com/category/" + model.CategoryHashId+"/"+model.CategoryType+"/"+model.SortOrder+model.PageType;
                return View(model);
            }
        }
    }
}
