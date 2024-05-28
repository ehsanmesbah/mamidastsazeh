using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Abstractions;
using mamidastsazeh.ViewModels;

namespace mamidastsazeh.Components
{
    
    
    public class CategoriesViewComponent: ViewComponent
    {

        private readonly IMainCategoryRepository _maincategories;
        public CategoriesViewComponent(IMainCategoryRepository maincategories)
        {
            _maincategories=maincategories;
        }
        public IViewComponentResult Invoke()
        {

            var maincategories = _maincategories.MainCategories;
            return View(maincategories);
        }
    }
}
