using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using mamidastsazeh.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Identity;
using DNTCaptcha.Core;
using mamidastsazeh.Data;
using mamidastsazeh.Services;

namespace mamidastsazeh.Areas.Membership.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _users;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IsmsRepository _smss;
        private readonly IHashId _hashId;
        private readonly ISmsManager _smsManager;
        public AccountController(IsmsRepository smss, UserManager<User> userManager, IUserRepository users, SignInManager<User> signInManager, IHashId hashId, RoleManager<IdentityRole> roleManager, ISmsManager smsMamager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _users = users;
            _smss = smss;
            _signInManager = signInManager;
            _hashId = hashId;
            _smsManager = smsMamager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("UserSetting", "Panel");
        }


        //[AllowAnonymous]
        /*public IActionResult Register(string mobile,string code)
        {

            return View("Register",new UserRegisterViewModel { Mobile=mobile,Code=code}) ;
        }*/
        public IActionResult RequireLogin(string returnUrl = "") {

            return View("RequireLogin",returnUrl);
        }
        public IActionResult VerifyMobile(string returnUrl = "")
        {
            MobileViewModel model = new MobileViewModel
            {
                ReturnUrl = returnUrl,
                Type = "Register"
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyMobile(MobileViewModel mobilestr,
            [FromServices] IMamiEventRepository _mamiEvent)
        {
            if (ModelState.IsValid)
            {
                var code = _smss.Smss.Where(s => s.MobileNumber == mobilestr.MobileNumber && s.SendTime > DateTime.Now.AddMinutes(-5)).OrderByDescending(s => s.SendTime).FirstOrDefault();
                string sendcode = (code != null) ? code.Code : "0";
                if (sendcode != "0" && sendcode != "" && sendcode == mobilestr.Code)
                {
                    var existUser = _users.Users.Where(u => u.UserName == mobilestr.MobileNumber).FirstOrDefault();
                    if (existUser != null)
                    {
                        await _signInManager.SignInAsync(existUser, true);

                        var mamiEvent = new MamiEvent()
                        {
                            EventType = MamiEventType.LoginBySms,
                            User = existUser,
                            EventTime = DateTime.Now,
                            Details = "لاگین با پیامک",
                            Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                        };
                        _mamiEvent.Add(mamiEvent);
                        _mamiEvent.SaveChanges();
                        return RedirectToAction("ResetPasswordPage", "Account", new { returnUrl = mobilestr.ReturnUrl });

                    }
                    else
                    {
                        UserRegisterViewModel user = new UserRegisterViewModel()
                        {
                            Mobile = mobilestr.MobileNumber,
                            ReturnUrl = mobilestr.ReturnUrl,
                            Code = mobilestr.Code
                        };
                        return View("Register", user);

                    }
                }
                else
                {

                    mobilestr.Message = "کد وارد شده صحیح نیست یا منقضی شده است";
                    return View("VerifyMobile", mobilestr);
                }
            }
            else
            {
                mobilestr.Message = "خطا! لطفا مجدد تلاش نمایید";
                return View("VerifyMobile", mobilestr);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "لطفا کد امنیتی تصویر بالا را وارد کنید",
                   CaptchaGeneratorLanguage = Language.English,
                   CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public JsonResult SendSmsAjaxMethod(MobileViewModel mobilestr)
        {

            if (mobilestr.Type != "ResetPassword" && ModelState.IsValid && _users.Users.Where(u => u.Mobile == mobilestr.MobileNumber || u.UserName == mobilestr.MobileNumber).Count() > 0)
            {
                MobileViewModel mobileinvalid = new MobileViewModel
                {
                    MobileNumber = "0",
                    Message = " شماره موبایل وارد شده در سایت وجود دارد. از فرم لاگین یا فراموشی پسورد استفاده نمایید"
                };
                return Json(mobileinvalid);
            }
            else if (mobilestr.Type == "ResetPassword" && ModelState.IsValid && _users.Users.Where(u => u.UserName == mobilestr.MobileNumber).Count() == 0)
            {
                MobileViewModel mobileinvalid = new MobileViewModel
                {
                    MobileNumber = "0",
                    Message = " شماره موبایل وارد شده در سایت وجود ندارد. از فرم ثبت نام استفاده کنید"
                };
                return Json(mobileinvalid);
            }
            if (ModelState.IsValid && _smss.Smss.Where(s => s.MobileNumber == mobilestr.MobileNumber && s.SendTime > DateTime.Now.AddMinutes(-2)).Count() > 0)
            {
                MobileViewModel mobileinvalid = new MobileViewModel
                {
                    MobileNumber = "0",
                    Message = "فاصله بین 2 پیام حداقل 2 دقیقه می باشد. لطفا بعد از 2 دقیقه مجددا تلاش نمایید"
                };
                return Json(mobileinvalid);
            }
            if (ModelState.IsValid)
            {
                MobileViewModel mobilevalid = mobilestr;

                var result = _smsManager.SendSms(mobilestr.MobileNumber, Request.HttpContext.Connection.RemoteIpAddress.ToString(), mobilestr.Type);
                if (result == "fail")
                {
                    MobileViewModel mobileinvalid = new MobileViewModel
                    {
                        MobileNumber = "0",
                        Message = "در ارسال پیام خطایی رخداده است لطفا بعدا تلاش نمایید"
                    };
                    return Json(mobileinvalid);
                }
                mobilestr.Message = null;
                return Json(mobilevalid);
            }
            else
            {
                MobileViewModel mobileinvalid = new MobileViewModel
                {
                    MobileNumber = "0",
                    Message = "لطفا شماره موبایل و کد امنیتی صحیح وارد کنید"
                };
                return Json(mobileinvalid);
            }


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel model,

            [FromServices] IWebHostEnvironment environment)
        {

            int checkexistsMobile = _users.Users.Where(u => u.UserName == model.Mobile).Count();
            if (checkexistsMobile > 0)
            {
                ModelState.AddModelError("", "شماره موبایل قبلا استفاده شده، از فرم ورود یا فراموشی رمز عبور استفاده نمایید");
            }
            int checkExistSms = _smss.Smss.Where(s => s.MobileNumber == model.Mobile && s.Code == model.Code && s.SendTime > DateTime.Now.AddMinutes(-30)).Count();
            if (checkExistSms == 0)
            {
                ModelState.AddModelError("", "کد منقضی شده یا مشکل دیگری پیش آمده لطفا مجددا کد اعتبار سنجی دریافت کنید");
            }
            if (string.IsNullOrEmpty(model.NickName))
            {
                ModelState.AddModelError("", "لطفا نام مستعار را وارد کنید");
            }
            else
            {
                model.NickName = model.NickName.Replace('ك', 'ک').Replace('ي', 'ی');
            }
            int checkExistNickName = _users.Users.Where(u => u.NickName.Replace(" ", "") == model.NickName.Replace(" ", "")).Count();
            if (checkExistNickName > 0)
            {
                ModelState.AddModelError("", "این نام تکراری است، لطفا نام دیگری انتخاب کنید");
            }

            if (ModelState.IsValid)
            {
                var user = new User
                {

                    UserName = model.Mobile,
                    NickName = model.NickName,
                    Mobile = model.Mobile,
                    UniquePrefix = Guid.NewGuid().ToString(),
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    IsBusinessAccount = false,
                    ProvinceId=1                    

                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Users");
                    var resultlogin = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
                    ViewBag.ReturnUrl = model.ReturnUrl;
                    return View("Success");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            else
            {
                ModelState.AddModelError("", "خطا در تکمیل فرم");
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public async Task<IActionResult> Login(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            var model = new UserLoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel model,
            [FromServices] IMamiEventRepository _mamiEvent)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    if (!(bool)user.IsActive)
                    {
                        await _signInManager.SignOutAsync();
                        ModelState.AddModelError("", "اکانت شما به دلیلی غیر فعال شده لطفا با ادمین سایت تماس بگیرید");
                        model.Password = "";
                        model.Username = "";
                        return View(model);
                    }
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);

                    if (result.Succeeded)
                    {
                        // await _roleManager.CreateAsync(new IdentityRole("Users"));
                        // await _roleManager.CreateAsync(new IdentityRole("PageOwner"));
                        
                        MamiEvent newEvent = new MamiEvent()
                        {
                            EventTime = DateTime.Now,
                            User = user,
                            EventType = MamiEventType.Login,
                            Details = "لاگین",
                            Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString()

                        };
                        _mamiEvent.Add(newEvent);
                        _mamiEvent.SaveChanges();
                        
                        return Redirect(model.ReturnUrl ?? "/");
                    }
                    else if (result.IsLockedOut)
                    {

                        ModelState.AddModelError("", "اکانت شما به دلیل ورود 5 بار رمز اشتباه غیر فعال شده است، لطفا 5 دقیقه منتظر بمانید بعد از آن میتوانید از گزینه فراموشی رمز عبور استفاده نمایید");
                        model.Password = "";
                        model.Username = "";
                        return View(model);
                    }
                }
                ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است.");
            }
            model.Password = "";
            model.Username = "";
            return View(model);
        }

        public IActionResult ResetPassword(string returnUrl = "")
        {
            MobileViewModel model = new MobileViewModel()
            {
                Type = "ResetPassword",
                ReturnUrl = returnUrl
            };
            return View("VerifyMobile", model);
        }

        [Authorize]
        public IActionResult ResetPasswordPage(string returnUrl = "")
        {
            UpdateUserPasswordViewModel userPassword = new UpdateUserPasswordViewModel
            {
                OldPassword = "-",
                ReturnUrl = returnUrl
            };

            return View(userPassword);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordPage(UpdateUserPasswordViewModel model,

        [FromServices] IWebHostEnvironment environment,
        [FromServices] IMamiEventRepository _mamiEvent)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                ModelState.AddModelError("", "این کاربر موجود نیست");
            }


            if (ModelState.IsValid)
            {


                var removeResult = await _userManager.RemovePasswordAsync(user);
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);

                if (result.Succeeded)
                {

                    TempData["SuccessMessage"] = "تغییرات با موفقیت اعمال شد";
                    var mamiEvent = new MamiEvent()
                    {
                        EventType = MamiEventType.ResetPassword,
                        User = user,
                        EventTime = DateTime.Now,
                        Details = "ریست پسورد با پیامک",
                        Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    _mamiEvent.Add(mamiEvent);
                    _mamiEvent.SaveChanges();
                    ViewBag.ReturnUrl = model.ReturnUrl;
                    return View("SuccessResetPasssword");
                }
                else
                {
                    ModelState.AddModelError("", " رمز جدید باید شامل حداقل 6 حرف انگلیسی کوچک و یک عدد باشد");
                }
            }

            return View();
        }

    }
}
