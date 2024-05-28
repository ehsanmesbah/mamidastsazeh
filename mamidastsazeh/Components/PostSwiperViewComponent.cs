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
    public class PostSwiperViewComponent: ViewComponent
    {
        
        private readonly IPostRepository _posts;
        
        public PostSwiperViewComponent(IPostRepository posts)
        {
            _posts = posts;
        }
        public IViewComponentResult Invoke(string postListType,int catid=1,string categoryType="Training")
        {
            ViewData["swipertype"] = postListType;
            List<Post> posts = new List<Post>();
            if (postListType == "mostlike")
            {
                posts = _posts.Posts.Where(p=> p.Poststate == PostState.Approved && p.PostType.ID!=5).OrderByDescending(p => p.NumberOfLikes).Take(10).ToList();
                if (posts.Count() > 0)
                {
                    ViewData["swipertype"] = postListType;
               }
                
            }
            else if (postListType == "mostview")
            {
                posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.Created>DateTime.Now.AddDays(-30) && p.PostType.ID != 5).OrderByDescending(p => p.NumberOfViews).Take(15).ToList();

            }
            else if (postListType == "new")
            {
                posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.CategoryId!=136 && p.PostType.ID != 5).OrderByDescending(p => p.Created).Take(12).ToList();
               
            }
            else if (postListType.ToLower() == "category")
            {
                if (categoryType == "Training")
                {
                    posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.CategoryId == catid && p.PostType.ID != 5).OrderByDescending(p => p.Created).Take(12).ToList();
                }
                else
                {
                    posts= _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.CategoryId == catid && p.PostType.ID == 5).OrderByDescending(p => p.Created).Take(12).ToList();
                }
               
            }
            else if (postListType == "business")
            {
                posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.CategoryId == catid && p.PostType.ID != 5).OrderByDescending(p => p.Created).Take(12).ToList();
              
            }
            else if (postListType == "download")
            {
                 posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && (p.CategoryId==142 || p.CategoryId ==143) && p.PostType.ID != 5).OrderByDescending(p => p.Created).Take(12).ToList();
               
            }
            else if (postListType.ToLower() == "product")
            {
                posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.PostType.ID ==5).OrderByDescending(p => p.Created).Take(12).ToList();
              

            }
            else if (postListType == "maminevesht")
            {
                 posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.Category.MainCategory.Id==8 && p.PostType.ID != 5).OrderByDescending(p => p.Created).Take(12).ToList();
            

            }
            else if (postListType == "smallswiper")
            {
                 posts = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.PostType.ID != 5).OrderByDescending(p => p.Created).Take(12).ToList();
              
            }
            else if (postListType == "video")
            {
                 posts = _posts.Posts.Where(p => p.ContentType.ID == 2 && p.Poststate == PostState.Approved && p.PostType.ID != 5).OrderByDescending(p => p.NumberOfComments).Take(12).ToList();
              
            }
            else if (postListType == "random")
            {
                List<int> randomId = new List<int>();
                var postMax = _posts.Posts.Where(p=> p.Poststate == PostState.Approved && p.PostType.ID != 5).OrderByDescending(p=>p.ID).Take(1).ToList();
                int maxId = postMax.First().ID;
                
                Random random = new Random();
                for (int i=0;i<14;i++)
                {
                    randomId.Add(random.Next(1, maxId));
                }
                posts = _posts.Posts.Where(p => randomId.Contains(p.ID) && p.Poststate == PostState.Approved && p.PostType.ID != 5).ToList();
               
            }
            else return View("Empty");
            if (posts.Count() > 0)
            {
                return View(posts);
            }
            else return View("Empty");
            
        }
    }
}
