using mamidastsazeh.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.ViewModels;

namespace mamidastsazeh.Components
{
    public class UsersPaginationViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public UsersPaginationViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke(int page, int limit)
        {
            var total = _userManager.Users.Count();

            if (total > 0)
            {
                var model = new UsersPaginationViewModel
                {
                    Page = page,
                    Limit = limit
                };

                Normalize(ref page, ref limit);

                var skip = (page - 1) * limit;
                var users = _userManager.Users.Skip(skip).Take(limit).ToList();
                model.Users = users;


                var buttons = CalculateButtons(page, limit, total);
                model.Buttons = buttons;


                return View(model);
            }
            else
            {
                return View("NoUsers");
            }
        }

        private IEnumerable<PaginationButton> CalculateButtons(int page, int limit, int total)
        {
            var prevDisabled = false;
            var nextDisabled = false;

            var firstPage = page - 2;
            if (firstPage <= 1)
            {
                firstPage = 1;
                prevDisabled = true;
            }

            var lastPage = firstPage + 4;
            var lastCalculatedPage = (int)Math.Ceiling((double)total / limit);
            if (lastPage >= lastCalculatedPage)
            {
                lastPage = lastCalculatedPage;
                nextDisabled = true;

                firstPage = lastPage - 4;
                if (firstPage <= 1)
                {
                    firstPage = 1;
                    prevDisabled = true;
                }
            }

            yield return new PaginationButton
            {
                Page = firstPage - 1,
                IsActive = false,
                Label = "Previous",
                Disabled = prevDisabled
            };

            for (int i = firstPage; i <= lastPage; i ++)
            {
                yield return new PaginationButton
                {
                    Page = i,
                    IsActive = page == i,
                    Label = i.ToString(),
                    Disabled = false
                };
            }

            yield return new PaginationButton
            {
                Page = lastPage + 1,
                IsActive = false,
                Label = "Next",
                Disabled = nextDisabled
            };


        }

        private void Normalize(ref int page, ref int limit)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (limit < 1)
            {
                limit = 5;
            }
        }
    }
}
