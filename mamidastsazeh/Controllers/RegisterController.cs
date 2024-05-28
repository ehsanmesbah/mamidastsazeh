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
    public class RegisterController :Controller
    {
       
        private readonly IUserRepository _users;

       
        public RegisterController(IUserRepository users)
        {
            _users = users;
        }
        public IActionResult Index()
        {
            return View("MakeRegister");
        }
        [HttpPost]
        public IActionResult MakeRegister(User user)
        {
         
            if (ModelState.IsValid)
            {
                return View("Complete");
            }
            else
            {
                ModelState.AddModelError("", "لطفا موارد مشخص شده را اصلاح کنید");
                return View();
            }
        }
     
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyMobile(string Mobile)
        {

            int sameuser = _users.Users.Where(u => u.Mobile == Mobile).Count();
            if (sameuser > 0)
            {
                return Json("شماره موبایل قبلا ثبت شده، از فرم فراموشی رمز استفاده کنید");
            }
            return Json(true);
        }
        
    }
}
