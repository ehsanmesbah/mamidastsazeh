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
using SixLabors.ImageSharp;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Xabe.FFmpeg;

namespace mamidastsazeh.Areas.Membership.Controllers
{
    [Authorize]
    public class PanelController : Controller
    {
        private IUserRepository _users;
        private readonly UserManager<User> _userManager;
        private readonly IMamiFileManager _mamiFileManager;
        private readonly ICategoryRepository _categories;
        private readonly IProvinceRepository _provinces;
        private readonly IPostRepository _posts;
        private readonly IMediaRepository _medias;
        private readonly IMamiEventRepository _mamiEvents;
        private readonly ICheckUserPermission _checkUserPermission;
        private readonly IMainCategoryRepository _mainCategories;
        public PanelController(IMamiEventRepository mamiEvents,IMediaRepository medias, IPostRepository posts ,IUserRepository users,UserManager<User> userManager,ICategoryRepository categories,
            IMamiFileManager mamiFileManager,IProvinceRepository provinces
            ,ICheckUserPermission checkUserPermission,IMainCategoryRepository mainCategories)
        {
            _provinces = provinces;
            _mamiFileManager = mamiFileManager;
            _userManager = userManager;
            _users = users;
            _categories = categories;
            _posts = posts;
            _medias = medias;
            _mamiEvents = mamiEvents;
            _checkUserPermission = checkUserPermission;
            _mainCategories = mainCategories;
        }
        public IActionResult Index() => View();

       

       
        public async Task<IActionResult> UserSettings()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.IsActive == true)
            {
                var provinces = _provinces.Provinces.OrderBy(c => c.Id);

                var model = new UserUpdateViewModel
                {
                    Mobile = user.UserName,
                    Description = user.Description,
                    Email = user.Email,
                    NickName = user.NickName,
                    FullName = user.FullName,
                    InstaAddress = user.InstaAddress,
                    Provinces=provinces,
                    Province=user.Province.Id,
                    City=user.CityName,
                    IsBusiness=user.IsBusinessAccount,
                    BusinessName=user.BusinessName,
                    BusinessPersianName=user.BusinessPersianName
                };

                return View(model);
            }
            else return RedirectToAction("Login","Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserSettings(UserUpdateViewModel model,
            
            [FromServices] IWebHostEnvironment environment)
        {
            if (!string.IsNullOrEmpty(model.Description))
            {
                model.Description = model.Description.Replace('ك', 'ک').Replace('ي', 'ی');
            }
            if (!string.IsNullOrEmpty(model.FullName))
            {
                model.FullName = model.FullName.Replace('ك', 'ک').Replace('ي', 'ی');
            }
            if (!string.IsNullOrEmpty(model.NickName))
            {
                model.NickName = model.NickName.Replace('ك', 'ک').Replace('ي', 'ی');
            }
            if (model.IsBusiness)
            {
                if (!string.IsNullOrEmpty(model.City))
                {
                    model.City = model.City.Replace('ك', 'ک').Replace('ي', 'ی');
                }
                if (!model.Province.HasValue) {
                    ModelState.AddModelError("", "لطفا استان خود را وارد کنید");
                }
                if (string.IsNullOrEmpty(model.BusinessName))
                {
                    ModelState.AddModelError("", "آدرس اینترنتی وارد نشده است");
                }
                else if (_users.Users.Where(u => u.BusinessName == model.BusinessName && u.UserName != model.Mobile).Count() > 0)
                {
                    model.BusinessName= model.BusinessName.Replace(" ", "");

                    ModelState.AddModelError("", "آدرس اینترنتی تکراری است لطفا از آدرس دیگری استفاده کنید");
                }
                if (string.IsNullOrEmpty(model.BusinessPersianName))
                {
                   
                    ModelState.AddModelError("", "عنوان فارسی فروشگاه وارد نشده است");
                }
                else if (_users.Users.Where(u => u.BusinessPersianName.Replace(" ","") == model.BusinessPersianName.Replace(" ","") && u.UserName != model.Mobile).Count() > 0)
                {
                    ModelState.AddModelError("", "عنوان فروشگاه تکراری است لطفا از عنوان دیگری استفاده کنید");
                }
            }
            if (model.NickName != null && _users.Users.Where(u => u.NickName == model.NickName && u.UserName != model.Mobile).Count() > 0)
            {
                ModelState.AddModelError("", "نام مستعار تکراری است لطفا از نام دیگری استفاده کنید");
            }
            
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null  )
            {
                ModelState.AddModelError("", "این کاربر موجود نیست");
            }
            if (model.Mobile != user.UserName)
            {
                ModelState.AddModelError("", "دستکاری غیر مجاز در شماره موبایل");
                
            }
            if (!string.IsNullOrEmpty(model.InstaAddress))
            {
                model.InstaAddress.Replace(" ", "");
            }

            if(model.InstaAddress!=null && _users.Users.Where(u=>u.InstaAddress==model.InstaAddress && u.UserName != model.Mobile).Count() > 0)
            {
                ModelState.AddModelError("", "بروز رسانی انجام نشد! صفحه اینسنتاگرام وارد شده قبلا توسط اکانت دیگری ثبت شده، لطفا در صورتیکه صفحه به شما تعلق دارد از دایرکت اینستاگرام با @mamidastsazeh تماس بگیرید");
            }

            if (ModelState.IsValid)
            {

                model.Mobile = user.UserName;
                try
                {
                    if (model.ProfileImage?.Length > 0)
                    {
                        var extension = Path.GetExtension(model.ProfileImage.FileName);
                        List<string> validextension =new List<string> () { ".jpg", ".png", ".gif", ".jpeg" };
                        if (!validextension.Contains(extension.ToLower()))
                        {
                            ViewBag.UserSettingFailMessage = "فرمت فایل ارسالی صحیح نیست";
                            return View(model);
                        }
                        var oldFileName = "";
                        if (user.ProfileImage?.Length > 0)
                        {
                            oldFileName = user.ProfileImage;
                        }
                        var fileName = $"{user.UniquePrefix}{Guid.NewGuid()}.jpg";


                        var path = Path.Combine(environment.WebRootPath, "ProfileImages", fileName);

                        var image = SixLabors.ImageSharp.Image.Load(model.ProfileImage.OpenReadStream());
                        double wToH = (double)image.Width / (double)image.Height;
                        int newW = 400;
                        int newH = (int)(newW / (double)wToH);
                        //xyz

                        bool compand = true;
                        ResizeMode mode = ResizeMode.Stretch;
                        var resizeOptions = new ResizeOptions
                        {
                            Size = new SixLabors.ImageSharp.Size(newW, newH),
                            Sampler = KnownResamplers.Lanczos2,
                            //Sampler = KnownResamplers.Bicubic,
                            Compand = compand,
                            Mode = mode
                        };

                        image.Mutate(x => x
                             .Resize(resizeOptions)
                        );

                        var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder()
                        {
                            Quality = 50
                           
                        };
                        image.Save(path, encoder);
                        
                        if (oldFileName != "")
                        {
                            var oldpath = Path.Combine(environment.WebRootPath, "ProfileImages", oldFileName);
                            var movepath = Path.Combine(environment.WebRootPath, "oldimages", oldFileName);
                            try
                            {
                                System.IO.File.Move(oldpath, movepath);
                            }catch(Exception e)
                            {

                            }
                        }


                        user.ProfileImage = fileName;
                    }
                }
                catch(Exception e)
                {
                   ViewBag.UserSettingFailMessage = "مشکلی پیش آمده،ممکن است فایل وارد شده صحیح نباشد لطفا بعدا تلاش نمایید";
                    return View(model );
                }
                user.Description = model.Description;
                user.FullName = model.FullName;
                user.NickName = model.NickName;
                user.InstaAddress = model.InstaAddress;
                user.IsBusinessAccount = model.IsBusiness;
                
                user.CityName = model.City;
                if (model.IsBusiness)
                {
                    user.ProvinceId = model.Province;
                    user.BusinessName = model.BusinessName.Replace(" ", "");
                    user.BusinessPersianName = model.BusinessPersianName.Trim();
                }
                var result = await _userManager.UpdateAsync(user);
                
                if(result.Succeeded)
                {
                   
                   ViewBag.UserSettingSuccessMessage = "تغییرات با موفقیت اعمال شد. از منوی سمت راست جهت ارسال آموزش یا شرکت در بازارچه استفاده نمایید";
                }
                else
                {
                   ViewBag.UserSettingFailMessage = "مشکلی پیش آمده، لطفا بعدا تلاش نمایید";
                }
      
            }
            var provinces = _provinces.Provinces.OrderBy(c => c.Id);
            model.Provinces = provinces;
            return View(model);
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UpdateUserPasswordViewModel model,

            [FromServices] IWebHostEnvironment environment)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                ModelState.AddModelError("", "این کاربر موجود نیست");
            }

            if (string.IsNullOrEmpty(model.OldPassword))
            {
                ModelState.AddModelError("", "رمز عبور قبلی را وارد کنید");
            }
            if (ModelState.IsValid)
            {

                
                var result = await _userManager.ChangePasswordAsync(user,model.OldPassword,model.NewPassword);

                if (result.Succeeded)
                {

                    ViewBag.SuccessMessage = "تغییرات با موفقیت اعمال شد";
                }
                else
                {
                    
                    ModelState.AddModelError("", "لطفا رمز عبور فعلی را درست وارد کنید، رمز جدید باید شامل حداقل 6 حرف انگلیسی کوچک و یک عدد باشد");
                }

            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UploadPost([FromServices] IWebHostEnvironment environment,string postTypeString="Training")
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var categories = _categories.Categories.OrderBy(c => c.Name).ToList();
            var maincategories = _mainCategories.MainCategories.OrderBy(c => c.maincategoryname).ToList();
            if(user.InstaAddress==null || user.InstaAddress == "")
            {
                return View("SetInstagram");
            }
            if (!User.IsInRole("MamiAdministrator"))
            {
                maincategories = _mainCategories.MainCategories.Where(c=>c.Id != 8).OrderBy(c => c.maincategoryname).ToList();
                //categories = categories.Where(c => c.Id != 136).OrderBy(c=>c.Name).ToList();
            }

            var model = new PostSubmitViewModel
            {
                MainCategories = maincategories,
                Categories = categories
            };
            if (postTypeString == "Product") model.PostType = 5;
            else model.PostType = 3;
            model.Price = 0;
            model.DiscountPrice = 0;
            
            return View(model);
        }
        
        public async Task<IActionResult> RefreshCategory(string MainCategoryId,
           [FromServices] IWebHostEnvironment environment)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            
            if (String.IsNullOrEmpty(MainCategoryId))
            {
                throw new ArgumentNullException("countryId");
            }
            int id = 0;
            bool isValid = Int32.TryParse(MainCategoryId, out id);
            var categories = _categories.Categories.Where(c=>c.MainCategory.Id==id).OrderBy(c=>c.Name);
         
            var result = (from s in categories
                          select new
                          {
                              Id = s.Id,
                              Name = s.Name
                          }).ToList();
            return Json(result);
        }
        [HttpPost]
        [RequestSizeLimit(100000000)]
        public async Task<IActionResult> UploadingImages(IFormFileCollection file,
           [FromServices] IWebHostEnvironment environment)
        {
            //var imageFiles = file;
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                
                List<string> guids= new List<string>();
                foreach (IFormFile imageFile in file) {
                    try
                    {
                        var guid = Guid.NewGuid();

                        var extension = Path.GetExtension(imageFile.FileName);
                        List<string> validImageExtension = new List<string>() { ".jpg", ".png", ".gif", ".jpeg", ".bmp", ".jpeg", ".gif", ".heif", ".heic" };
                        List<string> validVideoExtension = new List<string>() { ".mp4", ".mpg", ".wmv", ".mov", ".mpeg", ".3gp", ".mp3", ".m4a", ".wma" };

                        if (extension.ToLower() != ".pdf" && !validImageExtension.Contains(extension.ToLower()) && !validVideoExtension.Contains(extension.ToLower()))
                        {
                            TempData["ErrorMessage"] = "فرمت فایل ارسالی صحیح نیست";
                            return Json(new { success = false });
                        }
                        var fileName = "";
                        if (validImageExtension.Contains(extension.ToLower()))
                        {
                            fileName = $"{guid}" + ".jpg";
                            var path = Path.Combine(environment.WebRootPath, "photos/tempphotos", fileName);
                            var result = _mamiFileManager.SaveImage(imageFile, path);
                            if ( result== SaveResult.Fail)
                            {
                                return Json(new { success = false, error = "اشکال در ذخیره تصویر." }); ;
                            }
                        }
                        else if (extension.ToLower() == ".pdf")
                        {
                            fileName = $"{guid}" + "raw" + extension.ToLower();
                            var path = Path.Combine(environment.WebRootPath, "pdf/tempfiles", fileName);
                            var result= await _mamiFileManager.SavePdf(imageFile, path);
                            if (result == SaveResult.Fail)
                            {
                                return Json(new { success = false, error = "اشکال در ذخیره فایل." }); ;
                            }

                        }
                        else if (validVideoExtension.Contains(extension.ToLower()))
                        {
                            fileName = $"{guid}" + "raw" + extension.ToLower();
                            var path = Path.Combine(environment.WebRootPath, "videos/tempfiles", fileName);
                            var result= await _mamiFileManager.SaveVideo(imageFile, path);
                            if (result == SaveResult.Fail)
                            {
                                return Json(new { success = false, error = "اشکال در ذخیره ویدیو." }); ;
                            }

                        }

                        guids.Add(fileName);
                        _mamiEvents.Add(new MamiEvent()
                        {
                            Details = fileName,
                            EventType = MamiEventType.uploadfile,
                            Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                            UserId = user.Id,
                            EventTime = DateTime.Now
                        });
                    }
                    catch(Exception e)
                    {
                        return Json(new { success = false, error = "اشکال در ذخیره تصویر." });
                    }
                }
                _mamiEvents.SaveChanges();
                return Json(new { success = true,guid=guids });
                
                
            }
            catch (Exception e)
            {
                return Json(new { success = false, error = "نوع فایل ارسال شده معتبر نیست." });
            }
        }
        [ValidateAntiForgeryToken]
        public IActionResult RemovePhotosAjax(string[] videoGuids,
           [FromServices] IWebHostEnvironment env)
        {
            
            foreach(var guid in videoGuids)
            {
                var fileDir = Path.Combine(env.WebRootPath, "photos", "tempphotos");
                var filePath = Path.Combine(fileDir, guid);
                System.IO.File.Delete(filePath);
            }
            return Json(new { success = true });
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPost(PostSubmitViewModel post)
        {
        
            var user = await _userManager.GetUserAsync(HttpContext.User);
            post.Description = post.Description.Replace('ك', 'ک').Replace('ي', 'ی');
            if(!_checkUserPermission.AllowedHtml(user.UserName)) post.Description=post.Description.Replace("<","-").Replace(">", "-");
            post.Title = post.Title.Replace('ك', 'ک').Replace('ي', 'ی');
            if(!_checkUserPermission.AllowedHtml(user.UserName)) post.Title.Replace("<", "-").Replace(">", "-");

            if(!string.IsNullOrEmpty(post.RawTags)) post.RawTags = post.RawTags.Replace('ك', 'ک').Replace('ي', 'ی').Replace("<", "-").Replace(">", "-");
            if (ModelState.IsValid)
            {
                
                try
                {
                    
                    string refreshTag = "";
                    if (!string.IsNullOrEmpty(post.RawTags)) {
                        int counter= 0;
                        var tags = post.RawTags.Replace(" ", "").Remove(0,(post.RawTags.StartsWith("#")?1:0)).Split("#");

                        foreach (var tag in tags)
                        {
                            if (counter < 5)
                            {
                                refreshTag = refreshTag + " #" + tag;
                                counter++;
                            }
                            refreshTag.Remove(0, 1);
                        }
                    }
                    Post newpost = new Post()
                    {
                        CategoryId = post.Category,
                        Poststate = PostState.New,
                        Title = post.Title,
                        Description = post.Description,
                        ContentTypeID = 1,
                        PostTypeID = post.PostType,
                        Price=post.Price,
                        DiscountPrice=post.DiscountPrice,
                        SendType=post.SendType,
                        NumberOfComments = 0,
                        NumberOfCommentsinsta = 0,
                        NumberOfLikes = 0,
                        NumberOfLikesinsta = 0,
                        NumberOfViews = 0,
                        NumberOfViewsinsta = 0,
                        Thumbnail = post.imageGuids[post.imageGuids.Count()-1],
                        Created = DateTime.Now,
                        RawPostTags =refreshTag,
                        UserId = user.Id
                    };
                    
                    _posts.Add(newpost);
                    _posts.SaveChanges();
                    
                    int i = 1;
                    int numberOfImages = 1;
                    int numberOfVideo = 1;
                    int numberOfPdf = 1;
                    foreach (var imageguid in post.imageGuids)
                    {
                        var extension = Path.GetExtension(imageguid);
                        List<string> validVideoExtension = new List<string>() { ".mp3", ".m4a", ".wma" , ".mp4", ".mpg", ".wmv", ".mov", ".mpeg", ".3gp"};
                        
                        if (validVideoExtension.Contains(extension.ToLower()) && numberOfVideo<2)
                        {                        
                            _medias.Add(new Media()
                            {
                                Path = "videos/tempfiles/" + imageguid,
                                Type = "video",
                                Post = newpost,
                                SortOrder = i

                            });
                            numberOfVideo++;
                        }
                        else if (extension.ToLower() == ".pdf" && numberOfPdf < 2)
                        {
                            _medias.Add(new Media()
                            {
                                Path = "pdf/tempfiles/" + imageguid,
                                Type = "PDF",
                                Post = newpost,
                                SortOrder = i

                            });
                        }
                        else if(!validVideoExtension.Contains(extension.ToLower()) &&numberOfImages <=5)
                        {
                            _medias.Add(new Media()
                            {
                                Path = "photos/tempphotos/" + imageguid,
                                Type = "photo",
                                Post = newpost,
                                SortOrder = i

                            });
                            numberOfImages++;
                        }
                        i++;
                    }
                    _medias.SaveChanges();
                    MamiEvent mamiEvent = new MamiEvent()
                    {
                        EventType = MamiEventType.NewPost,
                        Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        Post = newpost,
                        UserId = user.Id,
                        Details = "ارسال پست جدید",
                        EventTime = DateTime.Now

                    };
                    _mamiEvents.Add(mamiEvent);
                    _mamiEvents.SaveChanges();
                    var categories = _categories.Categories;
                    post.Categories = categories;

                    return View("SubmitSuccess");
                }
                catch(Exception e)
                {
                    MamiEvent mamiEvent = new MamiEvent()
                    {
                        EventType = MamiEventType.ErrorSendPost,
                        Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        UserId = user.Id,
                        Details = e.Message,
                        EventTime = DateTime.Now

                    };
                    _mamiEvents.Add(mamiEvent);
                    _mamiEvents.SaveChanges();
                    return View("SubmitFail");
                }
            }
            else
            {
                try
                {
                    MamiEvent mamiEvent = new MamiEvent()
                    {
                        EventType = MamiEventType.ErrorSendPost,
                        Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        UserId = user.Id,
                        Details = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)),
                        EventTime = DateTime.Now

                    };
                    _mamiEvents.Add(mamiEvent);
                    _mamiEvents.SaveChanges();
                }
                catch(Exception e)
                {
                    return View("SubmitFail");
                }
                return View("SubmitFail");
            }
        }

       
        public async Task<IActionResult> UploadingFile(IFormFile file,
            [FromServices] IWebHostEnvironment env,
            [FromServices] IConfiguration _configuration)
        {
            var videoFile = file;
            if (videoFile == null)
            {
                throw new ArgumentNullException(nameof(videoFile));
            }
            var extension = Path.GetExtension(videoFile.FileName);
            List<string> validextension = new List<string>() { ".mp4", ".mov", ".pdf",".wmv",".3gp"};
            if (!validextension.Contains(extension.ToLower()))
            {
                TempData["ErrorMessage"] = "فرمت فایل ارسالی صحیح نیست";
                return Json(new { success = false,error="فرمت فایل صحیح نیست" });
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var guid = Guid.NewGuid();
            var fileName = guid+"raw"+extension;

            var fileDir = Path.Combine(env.WebRootPath, "videos", "tempfiles");
            var filePath = Path.Combine(fileDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                   await videoFile.CopyToAsync(stream);
                   
            }
           
            _mamiEvents.Add(new MamiEvent()
            {
                    Details = fileName,
                    EventType = MamiEventType.uploadfile,
                    Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    UserId = user.Id,
                    EventTime = DateTime.Now
            });
            _mamiEvents.SaveChanges();
            
            return Json(new { success = true, FileName=fileName });
        }
        [HttpGet]
        [Authorize(Roles = "MamiAdministrator")]
        public IActionResult UploadFilePost()
        {
            var categories = _categories.Categories.OrderBy(c=>c.Name);
            var model = new PostFileSubmitViewModel
            {
                Categories = categories
            };

            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "MamiAdministrator")]
        public async Task<IActionResult> UploadFilePost(PostFileSubmitViewModel post,

            [FromServices] IWebHostEnvironment environment)
        {
            /*post.Description = post.Description.Replace('ك', 'ک').Replace('ي', 'ی');
            post.Title = post.Title.Replace('ك', 'ک').Replace('ي', 'ی');

            if (ModelState.IsValid)
            {
                string thumbnailPath = "";
                string path="",fileName="";
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (post.Thumbnail != null)
                {
                    
                    var extension = Path.GetExtension(post.Thumbnail.FileName);
                    List<string> validextension = new List<string>() { ".jpg", ".png", ".gif", ".jpeg" };
                    if (!validextension.Contains(extension.ToLower()))
                    {
                        TempData["ErrorMessage"] = "فرمت فایل ارسالی صحیح نیست";
                        return View(post);
                    }

                    fileName = $"{Guid.NewGuid()}{extension}";
                    DateTime currentTime = DateTime.Now;
                    int currentmonth = currentTime.Month;
                    string yearmonth = (currentmonth < 10) ? currentTime.Year.ToString() + "0" + currentmonth.ToString() : currentTime.Year.ToString() + currentmonth.ToString();


                    thumbnailPath = "postthumbnail/" + yearmonth;
                    path = Path.Combine(environment.WebRootPath, thumbnailPath, fileName);

                    var image = SixLabors.ImageSharp.Image.Load(post.Thumbnail.OpenReadStream());
                    if (image.Height >= image.Width)
                    {
                        double wToH = (double)image.Width / (double)image.Height;
                        int newW = 400;
                        int newH = (int)(newW / (double)wToH);

                        image.Mutate(x => x.Resize(newW, newH));
                        image.Mutate(x => x.Crop(newW, newW));
                    }
                    else
                    {
                        double htoW = (double)image.Height / (double)image.Width;
                        int newH = 400;
                        int newW = (int)(newH / (double)htoW);

                        image.Mutate(x => x.Resize(newW, newH));
                        image.Mutate(x => x.Crop(newH, newH));
                    }

                    //image.Mutate(x => x.Resize(newW, newH));
                    var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder()
                    {
                        Quality = 50 //Use variable to set between 5-30 based on your requirements
                    };
                    image.Save(path, encoder);
                }
                
                string type = "";
                int contenttype = 0;
                int posttype = 0;
                if (post.Guid.ToLower().EndsWith(".pdf"))
                {
                    type = "PDF";
                    contenttype = 4;
                    posttype = 4;
                }
                else
                {
                    type = "video";
                    contenttype = 2;
                    posttype = 3;
                }

                //end
                
                Post newpost = new Post()
                {
                    CategoryId = post.Category,
                    Poststate = PostState.New,
                    Title = post.Title,
                    Description = post.Description,
                    ContentTypeID = contenttype,
                    PostTypeID = posttype,
                    NumberOfComments = 0,
                    NumberOfCommentsinsta = 0,
                    NumberOfLikes = 0,
                    NumberOfLikesinsta = 0,
                    NumberOfViews = 0,
                    NumberOfViewsinsta = 0,
                    Thumbnail = thumbnailPath+"/" + fileName,
                    Created = DateTime.Now,
                    RawPostTags = post.RawTags,
                    UserId = user.Id
                };
                _posts.Add(newpost);
                _posts.SaveChanges();

              
                
                _medias.Add(new Media()
                    {
                        Path ="videos/tempfiles/"+ post.Guid,
                        Type = type,
                        Post = newpost,
                        SortOrder = 1

                });
                _medias.SaveChanges();
                
                MamiEvent mamiEvent = new MamiEvent()
                {
                    EventType = MamiEventType.NewPost,
                    Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Post = newpost,
                    UserId = user.Id,
                    Details = "ارسال پست جدید",
                    EventTime = DateTime.Now

                };
                _mamiEvents.Add(mamiEvent);
                _mamiEvents.SaveChanges();
                
                return View("SubmitSuccess");
            }
            else
            {
                return View("SubmitFail");
            }
            */
            return View();
        }

        public async Task<IActionResult> ManageMyPosts()
        {
            var user =await _userManager.GetUserAsync(HttpContext.User);
            var posts = _posts.Posts.Where(p => p.User == user && p.Poststate!=PostState.DeletedByUser).OrderByDescending(p=>p.Created).Take(200);
            return View(posts);

        }
        [HttpGet]
        public IActionResult SuspendMyUser() => View();

        [HttpPost]
        public async Task<IActionResult> SuspendMyUser(
       [FromServices] IWebHostEnvironment environment,
       [FromServices] IMamiEventRepository _mamiEvent,
       [FromServices] IPostCommentRepository _postComment,
       [FromServices] SignInManager<User> _signInManager)
        {

            var userM = await _userManager.GetUserAsync(HttpContext.User);
            await _userManager.SetUserNameAsync(userM, "Suspended" + userM.UserName);
            
            var user = _users.Users.Where(u => u.Id == userM.Id).FirstOrDefault();

            user.Mobile = "Suspended" + user.Mobile;
            user.NormalizedUserName = userM.UserName.ToUpper();
            user.NickName = "Suspended" + user.NickName;
            
            user.IsActive = false;
            _users.SaveChanges();
            var posts = _posts.Posts.Where(p => p.UserId == user.Id);
            foreach(var post in posts)
            {
                post.Poststate = PostState.SuspendedAccount;
            }
            _posts.SaveChanges();
            var postComments = _postComment.PostComments.Where(p => p.UserId == user.Id);
            foreach(var comment in postComments)
            {
                comment.CommentState = CommentState.SuspendedAccount;
            }
            _postComment.SaveChanges();
            var mamiEvent = new MamiEvent
            {
                UserId = user.Id,
                EventTime = DateTime.Now,
                Details = "حذف اکانت توسط کاربر",
                Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                EventType = MamiEventType.SuspendUser
            };
            _mamiEvent.Add(mamiEvent);
            _mamiEvent.SaveChanges();
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });

        }
    }
}
