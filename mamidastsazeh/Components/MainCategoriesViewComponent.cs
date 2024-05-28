using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Abstractions;
using mamidastsazeh.ViewModels;

namespace mamidastsazeh.Components
{
    
    
    public class MainCategoriesViewComponent: ViewComponent
    {

        private readonly IMainCategoryRepository _maincategories;
        private readonly IHashId _hashid;
        public MainCategoriesViewComponent(IMainCategoryRepository maincategories,IHashId hashid)
        {
            _maincategories=maincategories;
            _hashid = hashid;
        }
        public IViewComponentResult Invoke(string listType,string strid)
        {
            int id = _hashid.Decode(strid);
            var maincategories= _maincategories.MainCategories.Where(mt => mt.Id > 0); 
            if (listType == "specialmaincategory")
            {
                var specialcat = maincategories.Where(mt => mt.Id == id);
                return View(specialcat );
            }
           else  return View(maincategories);
        }
    }
}
