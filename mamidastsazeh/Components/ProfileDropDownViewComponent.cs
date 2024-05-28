using mamidastsazeh.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Components
{
    public class ProfileDropDownViewComponent : ViewComponent
    {
        private readonly IUserRepository _users;

        public ProfileDropDownViewComponent(IUserRepository users)
        {
            _users=users;
        }

        public IViewComponentResult Invoke()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = _users.Users
                    .Where(c => c.UserName == HttpContext.User.Identity.Name)
                    .FirstOrDefault();

                var model = user?.ProfileImage?.Length > 0 ? user.ProfileImage : "default.gif";


                return View("LoggedIn", model);
            }
            else
            {
                return View();
            }
        }
    }
}
