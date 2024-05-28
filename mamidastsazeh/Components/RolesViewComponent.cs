using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Data;
using mamidastsazeh.Models;
using mamidastsazeh.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace mamidastsazeh.Components
{
    public class RolesViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesViewComponent(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(User user)
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();

            var userInRoles = await _userManager.GetRolesAsync(user);
            var userOutRoles = new List<string>();

            foreach (var role in roles)
            {
                if (!userInRoles.Contains(role))
                {
                    userOutRoles.Add(role);
                }
            }

            var model = new  UserRolesEditViewModel
            {
                UserInRoles = userInRoles,
                UserOutRoles = userOutRoles
            };

            return View(model);
        }
        
    }
}
