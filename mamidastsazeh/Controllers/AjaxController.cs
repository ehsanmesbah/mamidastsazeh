using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Castle.Core.Internal;
using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace mamidastsazeh.Controllers
{
    public class AjaxController : Controller
    {
        private readonly IPostRepository _posts;
        public AjaxController(IPostRepository posts)
        {
            _posts = posts;
        }
        public IActionResult LoadMorePosts(int lastCategoryID)
        {
            return View(lastCategoryID);
        }
        public IActionResult Search(string term)
        {

            //
            if (!string.IsNullOrEmpty(term))
            {
                term = term.Replace('ك', 'ک').Replace('ي', 'ی');
            }
            var posts = _posts.Posts.Where(p => p.Poststate==PostState.Approved && (p.Title.Contains(term) || p.Description.Contains(term))).Take(30);

            return View("Search", posts);
        }
        /*public IActionResult SearchPage([FromServices] IUnitOfWork uow, string term)
        {
            var terms = term.Split(" ");
            
            List<Post> posts=new List<Post>();
            foreach (var t1 in terms.Take(4)) {
             
                posts.AddRange(uow.Posts.Find(p => p.Description.Contains(t1)).Take(10));
            }
           
            return View("Search", posts);
        }*/
    }
   

}
