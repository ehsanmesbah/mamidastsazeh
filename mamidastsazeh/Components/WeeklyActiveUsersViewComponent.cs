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
    
    public class WeeklyActiveUsersViewComponent : ViewComponent
    {
        public IUserRepository _users;

        public WeeklyActiveUsersViewComponent(IUserRepository users)
        {
            _users = users;
        }
        public IViewComponentResult Invoke()
        {
            try
            {
                var users = _users.Users.OrderByDescending(u => u.Posts.Where(p => p.Created > DateTime.Now.AddDays(-30) && p.Poststate==PostState.Approved).Count()).Take(5);
                return View(users);
            }
            catch (Exception e)
            {
                return View("Empty");
            }
        }
    }
}
