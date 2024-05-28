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
    public class MainCategoryController : Controller
    {
        private readonly IHashId _hashId;
        private readonly ICategoryRepository _categories;
        private readonly IPostRepository _posts;
        private readonly IMainCategoryRepository _mainCategories;
        public MainCategoryController(IMainCategoryRepository mainCategories,ICategoryRepository categories,IPostRepository posts,IHashId hashId)
        {
            _mainCategories = mainCategories;
            _categories = categories;
            _hashId = hashId;
            _posts = posts;
        }
        public IActionResult Index( string hashId, string maincategoryName,string categoryType="Training")
        {

            var mainCategoryId = _hashId.Decode(hashId);
            var maincat = _mainCategories.MainCategories.Where(mc => mc.Id == mainCategoryId);
            if (maincat.Count() == 0)
            {
                Response.StatusCode = 404;
                return View("Error");
            }
            else
            {
                List<Category> categories = new List<Category>();
                if (categoryType == "Training")
                {
                    categories = _categories.Categories.Where(c => c.MainCategory.Id == mainCategoryId).ToList();
                }
                else
                {
                    categories= _categories.Categories.Where(c => c.MainCategory.Id == mainCategoryId && c.Id!=6 && c.Id!=8).ToList();
                }

                ViewData["mainCategoryName"] = maincat.First().maincategoryname;
                ViewBag.categoryType = categoryType;
                ViewBag.Title = "مامی دست سازه آموزش های " + maincat.First().maincategoryname;
                ViewBag.Description = "، mamidastsazeh ، مامی دست سازه ، مر جع آموزش هنری دست سازه ساز جهت بانوان مادران رنگ آمیزی دکوپاژ دانلود دکوری تزئینات کاردستی عید دانلود تم تولد";
                ViewBag.URL = "https://mamidastsazeh.com/maincategory/" + hashId;
                return View(categories);
            }
        }
        public IActionResult AllMainCategory(string pageType="Training")
        {

            return View("AllMainCategory", pageType);
        }
    }
}
