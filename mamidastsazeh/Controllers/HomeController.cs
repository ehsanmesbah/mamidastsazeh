
using mamidastsazeh.Abstractions;
using mamidastsazeh.Data;
using mamidastsazeh.Models;
using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Threading.Tasks;

//using mamidastsazeh.ViewModels;
//using StackExchange.Profiling;

namespace mamidastsazeh.Controllers
{
    public class HomeController :Controller
    {
        private readonly IPostRepository _posts;
       
        public HomeController(IPostRepository posts)
        {
            _posts = posts;
        }
        public IActionResult Index()
        {
            ViewBag.Type = "Index";
            ViewBag.Description = "، mamidastsazeh ، مامی دست سازه ، مر جع آموزش هنری دست سازه ساز جهت بانوان مادران رنگ آمیزی دکوپاژ دانلود دکوری تزئینات کاردستی عید دانلود تم تولد";
            ViewBag.Title = " مامی دستسازه | آموزش هنرهای خانگی مادر و کودک";
            ViewBag.URL = "https://mamidastsazeh.com";
            //return RedirectToAction("Index", "SpecialPost", new { pageType = "product" });
            return View();
            
            
        }
        public IActionResult Error()
        {
            return View();
        }
        
      public string List(string id)
        {
            //var id = RouteData.Values["id"];
            return $"id= {id}";
        }
        public IActionResult AboutUs()
        {
            ViewBag.Description = "رقیه آقاجانی، سمیه آقاجانی، Roghayeh Aghajani Somayeh Aghajani";
            ViewBag.Title = "درباره مامی دست سازه سمیه آقاجانی";
            ViewBag.OgImage = "https://mamidastsazeh.com/images/aboutme.jpg";
            ViewBag.URL = "https://mamidastsazeh.com";

            return View();
        }
        public IActionResult AboutMe()
        {
            ViewBag.Description = "رقیه آقاجانی، سمیه آقاجانی، Roghayeh Aghajani Somayeh Aghajani";
            ViewBag.Title = "درباره من، سمیه آقاجانی";
            ViewBag.OgImage = "https://mamidastsazeh.com/images/aboutme.jpg";

            return View();
        }
        public IActionResult Ads()
        {
            return View();
        }
        public IActionResult MamiTrainings()
        {
            return View();
        }
    }
}
