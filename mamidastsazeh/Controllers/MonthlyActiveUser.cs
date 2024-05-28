using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
namespace mamidastsazeh.Controllers
{
    public class MonthlyActiveUser : Controller
    {
        public IUserRepository _users;
        public MonthlyActiveUser(IUserRepository users)
        {
            _users = users;
        }
        public IActionResult Index()
        {
            try
            {
                var users = _users.Users.OrderByDescending(u => u.Posts.Where(p => p.Created > DateTime.Now.AddDays(-30) && p.Poststate == PostState.Approved).Count()).Take(30);
                return View(users);
            }
            catch (Exception e)
            {
                return View("Empty");
            }
        }
    }
}
