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
    public class RelatedPostsViewComponent : ViewComponent
    {
        private readonly IPostRepository _posts;
        private IConfiguration _configuration;
       
        public RelatedPostsViewComponent(IPostRepository posts ,IConfiguration configuration)
        {
            _configuration = configuration;
            _posts = posts;
        }

        public IViewComponentResult Invoke(int postId)
        {
            try
            {
                var post = _posts.Posts.Where(v => v.ID == postId).First();
                List<int> randomId = new List<int>();
                var postCount = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.Category.Id == post.Category.Id && p.PostTypeID==5).Count();
                string relatedType = "Product";
                if (postCount < 2)
                {
                    relatedType = "Training";
                    postCount = _posts.Posts.Where(p => p.Poststate == PostState.Approved && p.Category.Id == post.Category.Id).Count();
                }

                List<Post> posts = new List<Post>();
                Random random = new Random();
                for (int i = 0; i < 6; i++)
                {
                    int recordno = random.Next(1, postCount+1);
                    Post post1=new Post() ;
                    if (relatedType == "Product")
                    {
                        post1=_posts.Posts.Where(p => p.Category == post.Category && p.Poststate == PostState.Approved && p.PostTypeID==5).Skip(recordno - 1).First();
                    }
                    else
                    {
                        post1=_posts.Posts.Where(p => p.Category == post.Category && p.Poststate == PostState.Approved).Skip(recordno - 1).First();
                    }
                    if (!posts.Contains(post1))
                    {
                        posts.Add(post1);
                    }
                    else if (postCount > 6) i--;
                }


                if (posts.Count() > 0)
                {
                    return View(posts);
                }


                //var relatedPosts = _uow.Posts.Find(v => v.ID!=post.ID && v.PostTags.Select(vt=>vt.TagId).Count()>0);


                return View("Empty");

            }
            catch(Exception e)
            {
            
                return View("Empty");
            }
        }
    }
}
