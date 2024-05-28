using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Abstractions;
using System.Security.Policy;

using mamidastsazeh.Models;
using Microsoft.AspNetCore.Authorization;
using mamidastsazeh.ViewModels;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Castle.Core.Internal;

namespace mamidastsazeh.Controllers
{
    public class SearchPageController:Controller
    {
        private readonly IPostRepository _posts;
        
        public SearchPageController(IPostRepository posts)
        {
            _posts = posts;
        }
        
     
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Search(PostsPaginationViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string searchTerm = inputModel.SearchTerm;
                    inputModel.Limit = 12;
                    if (inputModel.Page < 1) inputModel.Page = 1;
                    int total = 0;
                    if (string.IsNullOrEmpty(searchTerm))
                    {

                        return View("index");
                    }
                    searchTerm = searchTerm.Replace('ك', 'ک').Replace('ي', 'ی');
                    List<Post> posts = new List<Post>();
                    if (searchTerm.Length > 50)
                    {
                        ModelState.AddModelError(nameof(searchTerm), " عبارت جستجو طولانی تر از مقدار مجاز می باشد");
                    }

                    else
                    {

                        var terms = searchTerm.Split(" ").Take(4);
                        var termcount = terms.Count();



                        if (termcount == 1)
                        {
                            posts.AddRange(_posts.Posts.AsQueryable().Where(p => p.Poststate == PostState.Approved && p.Description.Contains(terms.ToList()[0])).OrderBy(inputModel.SortOrder.Replace("-", " ")).Skip((inputModel.Page - 1) * inputModel.Limit).Take(inputModel.Limit).ToList());

                            total = _posts.Posts.AsQueryable().Where(p => p.Poststate == PostState.Approved && p.Description.Contains(terms.ToList()[0])).Count();
                        }
                        /*else if (termcount == 2)
                        {
                            posts.AddRange(_posts.Posts.AsQueryable().Where(p => p.Poststate == PostState.Approved && ( p.Description.Contains(terms.ToList()[0]) || p.Description.Contains(terms.ToList()[1]))).OrderBy(sortOrder.Replace("-", " ")).Skip((page - 1) * limit).Take(limit));
                            total = _posts.Posts.Where(p => p.Poststate == PostState.Approved && (p.Description.Contains(terms.ToList()[0]) || p.Description.Contains(terms.ToList()[1]))).Count();
                        }
                        else if (termcount == 3)
                        {
                            posts.AddRange(_posts.Posts.AsQueryable().Where(p => p.Poststate == PostState.Approved && (p.Description.Contains(terms.ToList()[0]) || p.Description.Contains(terms.ToList()[1]) || p.Description.Contains(terms.ToList()[2]))).OrderBy(sortOrder.Replace("-", " ")).Skip((page - 1) * limit).Take(limit));
                            total = _posts.Posts.Where(p => p.Poststate == PostState.Approved && (p.Description.Contains(terms.ToList()[0]) || p.Description.Contains(terms.ToList()[1]) || p.Description.Contains(terms.ToList()[2]))).Count();

                        }
                        if (termcount >= 4)
                        {
                            posts.AddRange(_posts.Posts.AsQueryable().Where(p => p.Poststate == PostState.Approved && (p.Description.Contains(terms.ToList()[0]) || p.Description.Contains(terms.ToList()[1]) || p.Description.Contains(terms.ToList()[2]) || p.Description.Contains(terms.ToList()[3]))).OrderByDescending(sortOrder.Replace("-", " ")).Skip((page - 1) * limit).Take(limit));
                            total = _posts.Posts.Where(p => p.Poststate == PostState.Approved && (p.Description.Contains(terms.ToList()[0]) || p.Description.Contains(terms.ToList()[1]) || p.Description.Contains(terms.ToList()[2]) || p.Description.Contains(terms.ToList()[3]))).Count();

                        }
                      */

                    }

                    PostsPaginationViewModel model = new PostsPaginationViewModel()
                    {
                        Limit = 12,
                        Page = inputModel.Page,
                        Posts = posts,
                        PageType = "search",
                        Total = total,
                        SortOrder = inputModel.SortOrder,
                        SearchTerm = searchTerm
                    };
                    //ViewData["SearchTerm"] = searchTerm;
                    return View("Result", model);
                }
                catch(Exception e)
                {
                    return View("index");
                }
            }
            else
            {
                return View("index");
            }
        }
        
    }
    
   
}
