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
    public class PostSliderViewComponent: ViewComponent
    {
        private readonly ICategoryRepository _categories;
        private readonly IPostRepository _posts;
        
        public PostSliderViewComponent(IPostRepository posts,ICategoryRepository categories)
        {
            _categories = categories;
            _posts = posts;
        }
        public IViewComponentResult Invoke(string postListType, int lastCategoryID)
        {
            
            if (postListType == "mostlike")
            {
                var category = _categories.Categories.Where(c => c.Id > lastCategoryID).FirstOrDefault();
                var posts = _posts.Posts.Where(p=>p.ID>0 && p.Poststate == PostState.Approved).OrderByDescending(p => p.NumberOfLikes).Take(10).ToList();
                if (posts.Count() > 0)
                {
                    var viewModel = new PostSliderViewModel
                    {
                        Category = category,
                        postListType = "بیشترین لایک",
                        Posts = posts
                    };

                    return View(viewModel);
                }
                else return View("Empty");
            }
            else if (postListType == "mostview")
            {
                var category = _categories.Categories.Where(c => c.Id > lastCategoryID).FirstOrDefault();
                var posts = _posts.Posts.Where(p => p.ID > 0 && p.Poststate == PostState.Approved).OrderByDescending(p => p.NumberOfViews).Take(10).ToList();
                if (posts.Count() > 0)
                {
                    var viewModel = new PostSliderViewModel
                    {
                        Category = category,
                        postListType = "بیشترین بازدید",
                        Posts = posts
                    };

                    return View(viewModel);
                }
                else return View("Empty");
            }
            else if (postListType == "lastcreated")
            {
                var category = _categories.Categories.Where(c => c.Id > lastCategoryID).FirstOrDefault();
                var posts = _posts.Posts.Where(p => p.ID > 0 && p.Poststate == PostState.Approved).OrderByDescending(p => p.Created).Take(10).ToList();
                if (posts.Count() > 0)
                {
                    var viewModel = new PostSliderViewModel
                    {
                        Category = category,
                        postListType = "آخرین پست ها",
                        Posts = posts
                    };

                    return View(viewModel);
                }
                else return View("Empty");
            }
            else if (postListType == "category")
            {
                var category = _categories.Categories.Where(c => c.Id == lastCategoryID).FirstOrDefault();
                var posts = _posts.Posts.Where(p => p.Category== category && p.Poststate == PostState.Approved).OrderByDescending(p => p.Created).Take(10).ToList();
                if (posts.Count() > 0)
                {
                    var viewModel = new PostSliderViewModel
                    {
                        Category = category,
                        postListType = category.Name,
                        Posts = posts
                    };

                    return View(viewModel);
                }
                else return View("Empty");
            }
            else return View("Empty");
        }
    }
}
