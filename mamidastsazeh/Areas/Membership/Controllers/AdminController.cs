using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using mamidastsazeh.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace mamidastsazeh.Areas.Membership.Controllers
{
    [Authorize(Roles = "MamiAdministrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserValidator<User> _userValidator;
        private readonly IPasswordValidator<User> _passwordValidator;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IPostRepository _posts;
        public AdminController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserValidator<User> userValidator,
            IPasswordValidator<User> passwordValidator,
            IPasswordHasher<User> passwordHasher,
            IPostRepository posts)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userValidator = userValidator;
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
            _posts = posts;
        }

        public IActionResult Users(int page = 1, int limit = 100)
        {
            var model = new PaginationViewModel
            {
                Page = page,
                Limit = limit
            };

            return View(model);
        }

        public async Task<IActionResult> EditUser(string userId, int page, int limit)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            ViewBag.Page = page;
            ViewBag.Limit = limit;

            var model = new UserEditViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                Mobile = user.Mobile,
                NickName=user.NickName,
                Description=user.Description,
                IsActive=user.IsActive,
                Score=user.Score,
                ProfileImage=user.ProfileImage
                
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserEditViewModel model, int page, int limit)
        {
            ViewBag.Page = page;
            ViewBag.Limit = limit;

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ModelState.AddModelError("", "چنین کاربری موجود نیست");
            }
            if (ModelState.IsValid)
            {
                user.FullName = model.FullName;
                user.Mobile = model.Mobile;
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.NickName = model.NickName;
                user.InstaAddress = model.InstaAddress;


                var validUser = await _userValidator.ValidateAsync(_userManager, user);
                if (validUser.Succeeded)
                {
                    var validPass = true;
                    if (model.Password?.Length > 0)
                    {
                        validPass = false;
                        var validatePassword = await _passwordValidator
                            .ValidateAsync(_userManager, user, model.Password);

                        if (validatePassword.Succeeded)
                        {
                            validPass = true;
                            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                        }
                        else
                        {
                            foreach (var error in validatePassword.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }

                    if (validPass)
                    {
                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            var roles = _roleManager.Roles;

                            var addingRoles = model.Roles;
                            var removingRoles = new List<string>();

                            if (addingRoles?.Count() > 0)
                            {
                                await _userManager.AddToRolesAsync(user, model.Roles);

                                removingRoles = roles
                                .Where(r => !model.Roles.Contains(r.Name))
                                .Select(r => r.Name)
                                .ToList();
                            }
                            else
                            {
                                removingRoles = roles.Select(r => r.Name).ToList();
                            }

                            if (removingRoles?.Count() > 0)
                            {
                                await _userManager.RemoveFromRolesAsync(user, removingRoles);
                            }

                            TempData["SuccessMessage"] = "تنظیمات ذخیره شد";
                        }
                    }
                }
                else
                {
                    foreach (var error in validUser.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteUser(string userId, int page, int limit)
        {
            ViewBag.Page = page;
            ViewBag.Limit = limit;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            var model = new UserDeleteViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                
                UserName = user.UserName,
                Mobile = user.Mobile,
               
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(UserDeleteViewModel model, int page, int limit)
        {
            ViewBag.Page = page;
            ViewBag.Limit = limit;

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ModelState.AddModelError("", "چنین کاربری یافت نشد");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"کاربر {user.UserName} با موفقیت حذف شد.";
                return RedirectToAction("Users", "Admin", new { area = "Membership", page, limit });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }
        public IActionResult ManagePosts(string postState, int page=1,int limit=100)
        {
            IEnumerable<Post> posts=null ;
            int total=0;
            if(postState == "new")
            {
                total = _posts.Posts.Where(p => p.Poststate == PostState.New).Count();
               posts = _posts.Posts.Where(p => p.Poststate == PostState.New).OrderByDescending(p=>p.Created).Skip((page-1)*limit).Take(limit);
            }
            PostsPaginationViewModel model = new PostsPaginationViewModel()
            {
                Limit = 100,
                Page = 1,
                Posts = posts,
                PageType = "ManagePosts",
                Total = total
            };
            return View(model);
        }
        public IActionResult ReportedComments([FromServices]IReportRepository _report)
        {
            var reports = _report.Reports.Where(r => r.ReportState == ReportState.New && (r.PostComment.CommentState==CommentState.Approved || r.PostComment.CommentState==CommentState.New ||r.PostComment.CommentState==CommentState.Reported)  ).OrderBy(r => r.Id).Take(100);
            return View(reports);

        }
        [HttpPost]
        public  IActionResult AcceptCommentReport(long id,[FromServices] IReportRepository _report,
            [FromServices] IPostCommentRepository _postComment)
        {
            
            try
            {
                var report = _report.Reports.Where(r => r.Id == id).FirstOrDefault();
                var comment = _postComment.PostComments.Where(c => c.Id == report.PostComment.Id).FirstOrDefault();
                comment.CommentState = CommentState.DeletedByAdmin;
                _postComment.SaveChanges();
                report.ReportState = ReportState.Accepted;
                _report.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public IActionResult RejectCommentReport(long id, [FromServices] IReportRepository _report,
            [FromServices] IPostCommentRepository _postComment)
        {
            try
            {
                var report = _report.Reports.Where(r => r.Id == id).FirstOrDefault();
                var comment = _postComment.PostComments.Where(c => c.Id == report.PostComment.Id).FirstOrDefault();
                comment.CommentState = CommentState.Approved;
                _postComment.SaveChanges();
                report.ReportState = ReportState.Rejected;
                _report.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
        }
        public IActionResult NewPosts()
        {
            var posts = _posts.Posts.Where(p => p.Poststate == PostState.New).Take(100);
            return View(posts);
        }
        public IActionResult AllComments([FromServices] IPostCommentRepository _postComments)
        {
            var comments = _postComments.PostComments.Where(c=>c.CommentState==CommentState.New || c.CommentState==CommentState.ContainRestricted).Take(200);
            return View(comments);
        }
        public IActionResult GenerateSiteMap([FromServices] IPostRepository _posts,[FromServices] ICategoryRepository _categories,
            [FromServices] IHashId _hashId)
        {
            var posts = _posts.Posts.Where(p=>p.Poststate==PostState.Approved).OrderByDescending(p=>p.ID);
            var categories = _categories.Categories;
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDoc.DocumentElement;
            xmlDoc.InsertBefore(xmldecl, root);
            

            //xmlDoc.CreateElement("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            XmlNode rootNode = xmlDoc.CreateElement("urlset", (string)new System.Xml.Linq.XAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9"));
            
            xmlDoc.AppendChild(rootNode);
            
            //for root

            foreach (var post in posts)
            {
                XmlNode userNodeurl = xmlDoc.CreateElement("url");
                XmlNode userNode = xmlDoc.CreateElement("loc");
                userNode.InnerText = "https://mamidastsazeh.com/post/" + _hashId.Encode(post.ID) ;
                XmlNode userNode1 = xmlDoc.CreateElement("lastmod");
                userNode1.InnerText = post.Created.ToString("yyyy-MM-dd");
                //   XmlNode userNode2 = xmlDoc.CreateElement("changefrq");
                //   userNode2.InnerText = "monthly";
                var pri = "0.5";
                if (post.Created > DateTime.Now.AddDays(-30)) pri = "1";
                else if (post.Created < DateTime.Now.AddDays(-700)) pri = "0.6";
                else if (post.NumberOfViews > 200) pri = "1";
                else  pri = "0.7";

                XmlNode userNode3 = xmlDoc.CreateElement("priority");
                userNode3.InnerText = pri;

                userNodeurl.AppendChild(userNode);
                userNodeurl.AppendChild(userNode1);

                //   userNodeurl.AppendChild(userNode2);
                //userNodeurl.AppendChild(userNode3);
                rootNode.AppendChild(userNodeurl);

            }
            
            foreach (var category in categories)
            {
                XmlNode userNodeurl = xmlDoc.CreateElement("url");
                XmlNode userNode = xmlDoc.CreateElement("loc");
                userNode.InnerText = "https://mamidastsazeh.com/category/" + _hashId.Encode(category.Id) + "/category/1/Created-desc/"+category.Name.Replace(" ","-")+"/Category";
                XmlNode userNode1 = xmlDoc.CreateElement("lastmod");
                userNode1.InnerText = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
                //   XmlNode userNode2 = xmlDoc.CreateElement("changefrq");
                //   userNode2.InnerText = "weekly";


                var pri = "1";


                XmlNode userNode3 = xmlDoc.CreateElement("priority");
                userNode3.InnerText = pri;

                userNodeurl.AppendChild(userNode);
                userNodeurl.AppendChild(userNode1);

                // userNodeurl.AppendChild(userNode2);
                //  userNodeurl.AppendChild(userNode3);
                rootNode.AppendChild(userNodeurl);
            }
            foreach (var category in categories)
            {
                XmlNode userNodeurl = xmlDoc.CreateElement("url");
                XmlNode userNode = xmlDoc.CreateElement("loc");
                userNode.InnerText = "https://mamidastsazeh.com/category/" + _hashId.Encode(category.Id) + "/category/1/Created-desc/" + category.Name.Replace(" ", "-") + "/Product";
                XmlNode userNode1 = xmlDoc.CreateElement("lastmod");
                userNode1.InnerText = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
                //   XmlNode userNode2 = xmlDoc.CreateElement("changefrq");
                //   userNode2.InnerText = "weekly";


                var pri = "1";


                XmlNode userNode3 = xmlDoc.CreateElement("priority");
                userNode3.InnerText = pri;

                userNodeurl.AppendChild(userNode);
                userNodeurl.AppendChild(userNode1);

                // userNodeurl.AppendChild(userNode2);
                //  userNodeurl.AppendChild(userNode3);
                rootNode.AppendChild(userNodeurl);
            }
            if (true)
            {
                XmlNode userNodeurl = xmlDoc.CreateElement("url");
                XmlNode userNode = xmlDoc.CreateElement("loc");
                userNode.InnerText = "https://mamidastsazeh.com";
                XmlNode userNode1 = xmlDoc.CreateElement("lastmod");
                userNode1.InnerText = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
                //   XmlNode userNode2 = xmlDoc.CreateElement("changefrq");
                //   userNode2.InnerText = "weekly";


                var pri = "1";


                XmlNode userNode3 = xmlDoc.CreateElement("priority");
                userNode3.InnerText = pri;

                userNodeurl.AppendChild(userNode);
                userNodeurl.AppendChild(userNode1);

                // userNodeurl.AppendChild(userNode2);
                //  userNodeurl.AppendChild(userNode3);
                rootNode.AppendChild(userNodeurl);
            }
            
            xmlDoc.Save("/var/www/maminewweb/wwwroot/sitemap.xml");
            //xmlDoc.Save("d:/sitemap.xml");
            return View("GenerateSiteMap");
        }
        public IActionResult RejectComment(long id,
           [FromServices] IPostCommentRepository _postComment,
           [FromServices] IMamiEventRepository _mamiEvent)
        {
            try
            {
                
                var comment = _postComment.PostComments.Where(c => c.Id == id).FirstOrDefault();
                comment.CommentState = CommentState.DeletedByAdmin;
                _postComment.SaveChanges();
                var mamiEvent = new MamiEvent()
                {
                    EventTime = DateTime.Now,
                    Details = comment.Id.ToString(),
                    AffectedUserId=comment.UserId,
                    Post=comment.Post,
                    EventType=MamiEventType.DeleteCommentByAdmin
                    
                };
                _mamiEvent.Add(mamiEvent);
                _mamiEvent.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
        }
        public IActionResult ApproveComment(long id,
           [FromServices] IPostCommentRepository _postComment)
        {
            try
            {

                var comment = _postComment.PostComments.Where(c => c.Id == id).FirstOrDefault();
                comment.CommentState = CommentState.Approved;
                _postComment.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
        }
        [HttpGet]
        public IActionResult ApprovePost(string hashid,
            [FromServices] IHashId _hashId,
            [FromServices] ICategoryRepository _categories,
            [FromServices] IMediaRepository _media
            )
        {
            int id = _hashId.Decode(hashid);
            var post = _posts.Posts.Where(p => p.ID == id).FirstOrDefault();
            PostApproveViewModel postview = new PostApproveViewModel
            {
                Categories = _categories.Categories.OrderBy(c=>c.Name),
                post=post,
                videoGuid=_media.Medias.Where(m=>m.Post.ID==post.ID).OrderBy(m=>m.SortOrder).Select(m=>m.Path).ToArray()
                
            };


            return View(postview);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovePost(PostApproveViewModel model,
            [FromServices] UserManager<User> _userManager,
            [FromServices] IMediaRepository _media,
            [FromServices] IMamiEventRepository _mamiEvent,
             [FromServices] IWebHostEnvironment environment,
             [FromServices] IHashId _hashId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var post = _posts.Posts.Where(p => p.ID == model.post.ID).FirstOrDefault();


            var medias = _media.Medias.Where(m => m.Post == post).ToList();
            post.CategoryId = model.post.Category.Id;
            post.Title = model.post.Title;
            post.Description = model.post.Description;
            post.Price = model.post.Price;
            post.DiscountPrice = model.post.DiscountPrice;
            post.SendType = model.post.SendType;
            post.RawPostTags = model.post.RawPostTags;
            int order = 1;
            foreach (var guid in model.videoGuid)
            {
                medias.Where(m => m.Path == guid).FirstOrDefault().SortOrder = order;
                order++;
            }
            _media.SaveChanges();
            _posts.SaveChanges();
            

            return RedirectToAction("Index", "Post", new { area = "",hashId=_hashId.Encode(post.ID) });



        }

    }
}
