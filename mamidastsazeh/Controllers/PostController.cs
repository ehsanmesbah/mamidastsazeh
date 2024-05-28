using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Abstractions;
using System.Security.Policy;
using mamidastsazeh.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using mamidastsazeh.Components;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using StackExchange.Profiling;
using Microsoft.Extensions.Configuration;
using Xabe.FFmpeg;

namespace mamidastsazeh.Controllers
{
    public class PostController :Controller
    {
        private readonly IHashId _hashId;
        private readonly IPostRepository _posts;
        private readonly IPostLikeRepository _postLike;
        private readonly UserManager<User> _userManager;
        private readonly ICheckUserPermission _checkUserPermission;

        public PostController(IHashId hashId, IPostRepository posts,IPostLikeRepository postLike, UserManager<User> userManager,ICheckUserPermission checkUserPermission)
        {
           
                _hashId = hashId;
                _posts = posts;
                _postLike = postLike;
            _userManager = userManager;
            _checkUserPermission = checkUserPermission;
           
        }
        public async Task<IActionResult> Index(string hashId,
            [FromServices] UserManager<User> _userManager,
            [FromServices] IMediaRepository _medias,
            [FromServices] IMamiFileManager _mamiFileManager,
            [FromServices] IWebHostEnvironment environment,
            string text = "-"
        )
        {
          
            
            var postId = _hashId.Decode(hashId);
            Post post = null;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (User.IsInRole("MamiAdministrator"))
            {
                post = _posts.Posts.Where(p => p.ID == postId).FirstOrDefault();
                if (post.Poststate == PostState.New)
                {
                    var video=_medias.Medias.Where(m => m.Post == post && m.Type=="video" && m.Path.Contains("raw")).FirstOrDefault();
                    if (video != null)
                    {
                        var sourceFile = Path.Combine(environment.WebRootPath, video.Path);
                        string outputFileName = sourceFile.Replace("raw", "");
                        if (sourceFile.Contains(".mp3") || sourceFile.Contains(".m4a") || sourceFile.Contains(".wma"))
                        {
                            outputFileName = Path.ChangeExtension(outputFileName, ".mp3");
                            await _mamiFileManager.ConvertAudio(sourceFile, outputFileName);
                            
                            video.Path = Path.ChangeExtension(video.Path.Replace("raw", ""), ".mp3");
                        }
                        else
                        {
                            bool full = false;
                            if (_checkUserPermission.AllowFullVideo( post.User.UserName)  ) full = true;
                            outputFileName = Path.ChangeExtension(outputFileName, ".mp4");
                            await _mamiFileManager.ConvertVideo(sourceFile, outputFileName,full);
                            video.Path = Path.ChangeExtension(video.Path.Replace("raw", ""), ".mp4");
                            _medias.SaveChanges();
                        }
                    }

                }
            }
            else
            {
               
                post = _posts.Posts.Where(p => p.ID == postId && p.Poststate == PostState.Approved).FirstOrDefault();
               /* if(user==null && post.CategoryId == 136)
                {
                    return RedirectToAction("RequireLogin", "Account", new { area = "Membership",
                        ReturnUrl ="/post/"+hashId});

                }*/
            }

            if (post == null && user != null)
            {
                post = _posts.Posts.Where(p => p.ID == postId && p.User == user).FirstOrDefault();
            }


            if (post == null)
            {
                Response.StatusCode = 404;
                return View("Error");
            }
            else
            {
                post.NumberOfViews++;
                _posts.SaveChanges();
                if (user != null) ViewBag.IsLiked = _postLike.PostLikes.Where(pl => pl.PostId == postId && pl.UserId == user.Id).Count() == 0 ? "far fa-heart" : "fas fa-heart";
                string title = post.Title;
                ViewBag.Title = title.Substring(0, (70 > title.Length ? title.Length:70));
                //ViewBag.PostTitle = title.Substring(0, (70 > title.Length ? title.Length : 70));
                ViewBag.Type = "Post";
                ViewBag.Description = post.Description.Substring(0, (post.Description.Length > 200 ? 200: post.Description.Length));
                ViewBag.URL = "https://mamidastsazeh.com/post/" + hashId + "/" + post.Title.Replace(" ", "-");
                //var coverMedia = post.Thumbnail;
                ViewBag.OgImage = "https://mamidastsazeh.com/" + post.Thumbnail;
                return View(post);
             }
            
        }
        [Authorize]
        public async Task<IActionResult> Delete(string id,
            [FromServices] UserManager<User> _userManager,
            [FromServices] IMamiEventRepository _mamiEvent)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "MamiAdministrator"))
            {
                var post = _posts.Posts.Where(p => p.ID == _hashId.Decode(id)).First();
                post.Poststate = PostState.DeletedByAdmin;
                _posts.SaveChanges();
                MamiEvent newEvent = new MamiEvent
                {
                    Details="حذف پست توسط ادمین",
                    AffectedUserId=post.UserId,
                    EventTime=DateTime.Now,
                    Post=post,
                    User=user,
                    EventType=MamiEventType.DeletePostByAdmin,
                    Ip= Request.HttpContext.Connection.RemoteIpAddress.ToString()
                };
                _mamiEvent.Add(newEvent);
                _mamiEvent.SaveChanges();
                return RedirectToAction("NewPosts", "Admin", new { area = "Membership" });
            }
            else
            {
                var post = _posts.Posts.Where(p => p.ID == _hashId.Decode(id)).First();
                if (user == post.User)
                {
                    post.Poststate = PostState.DeletedByUser;
                    _posts.SaveChanges();
                    MamiEvent newEvent = new MamiEvent
                    {
                        Details = "حذف پست توسط کاربر",
                        AffectedUserId = post.UserId,
                        EventTime = DateTime.Now,
                        Post = post,
                        User = user,
                        EventType = MamiEventType.DeletePost,
                        Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    _mamiEvent.Add(newEvent);
                    _mamiEvent.SaveChanges();
                    return RedirectToAction("index", "Home");
                }
                return RedirectToAction("index", "Home");
            }

        }
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComment(string id,string commentText,
            [FromServices] UserManager<User> _userManager,
            [FromServices] IPostCommentRepository _postCommnet,
            [FromServices] IMamiEventRepository _mamiEvent,
            [FromServices] IRestrictedWordRepository _restrictedWord)
        {
            if (string.IsNullOrEmpty(commentText))
            {
                return RedirectToAction("Index", "Post", new { hashId = id, text = "-" });
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            commentText = commentText.Replace('ك', 'ک').Replace('ي', 'ی').Replace("<","-").Replace(">","-");
            var restrictedWords = _restrictedWord.RestrictedWords.Where(w => w.Id > 0).ToList();
            var state = CommentState.New;
            foreach(var restrictedWord in restrictedWords)
            {
                if (commentText.Contains(restrictedWord.Word))
                {
                    state = CommentState.ContainRestricted;
                }
            }

            PostComment comment = new PostComment()
            {
                UserId = user.Id,
                Approved=true,
                CommentDate=DateTime.Now,
                PostId=_hashId.Decode(id),
                Text=commentText.Substring(0,commentText.Length<150 ?commentText.Length : 150),
                NumberOfLikes=0,
                CommentState=state
            };
            MamiEvent mamiEvent = new MamiEvent()
            {
                UserId = user.Id,
                EventTime = DateTime.Now,
                EventType = MamiEventType.SendComment,
                Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                PostID = _hashId.Decode(id)
            };
            _postCommnet.Add(comment);
            _postCommnet.SaveChanges();
            _mamiEvent.Add(mamiEvent);
            _mamiEvent.SaveChanges();
            //var post = _posts.Posts.Where(p => p.ID == _hashId.Decode(id)).FirstOrDefault();
            return RedirectToAction("Index", "Post", new { hashId = id ,text="-"});
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LikePost(string id,
             [FromServices] UserManager<User> _userManager            
            )
        {
            var postId = _hashId.Decode(id);
            var post = _posts.Posts.Where(p => p.ID == postId).FirstOrDefault();
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (post!=null)
            {
                var likeExist = _postLike.PostLikes.
                    Where(pl => pl.UserId == user.Id && pl.PostId == postId).FirstOrDefault();
              
                if (likeExist != null)
                {
                    _postLike.Remove(likeExist);
                    _postLike.SaveChanges();
                    post.NumberOfLikes--;
                    if (post.NumberOfLikes < 0) post.NumberOfLikes =0;
                    _posts.SaveChanges();
                }
                else
                {
                    PostLike postLike = new PostLike()
                    {
                        LikeDate = DateTime.Now,
                        PostId = postId,
                        UserId = user.Id
                    };
                    _postLike.Add(postLike);
                    _postLike.SaveChanges();
                    post.NumberOfLikes++;
                    _posts.SaveChanges();
                }
            }

            return Json(new { success = true ,numberOfLikes=post.NumberOfLikes});

        }
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LikeComment(long id,
             [FromServices] UserManager<User> _userManager,
             [FromServices] ICommentLikeRepository _commentLike,
             [FromServices] IPostCommentRepository _postComment
            )
        {
          
            var comment = _postComment.PostComments.Where(p => p.Id == id).FirstOrDefault();
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (comment != null)
            {
                var likeExist = _commentLike.CommentLikes.
                    Where(pl => pl.UserId == user.Id && pl.PostCommentId == id).FirstOrDefault();

                if (likeExist != null)
                {
                    _commentLike.Remove(likeExist);
                    _postLike.SaveChanges();
                    comment.NumberOfLikes--;
                    if (comment.NumberOfLikes < 0) comment.NumberOfLikes = 0;
                    _postComment.SaveChanges();
                }
                else
                {
                    CommentLike commentLike = new CommentLike()
                    {
                        LikeDate = DateTime.Now,
                        PostCommentId = id,
                        UserId = user.Id
                    };
                    _commentLike.Add(commentLike);
                    _commentLike.SaveChanges();
                    comment.NumberOfLikes++;
                    _postComment.SaveChanges();
                }
            }

            return Json(new { success = true });

        }
        public async Task<IActionResult> DeleteComment(long id,
            [FromServices] UserManager<User> _userManager,
            [FromServices] ICommentLikeRepository _commentLike,
            [FromServices] IPostCommentRepository _postComment
           )
        {

            var comment = _postComment.PostComments.Where(p => p.Id == id).FirstOrDefault();
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (comment != null)
            {
               if(comment.User==user || (await _userManager.IsInRoleAsync(user, "MamiAdministrator"))){
                  
                    comment.CommentState = comment.User==user ? CommentState.DeletedByUser:CommentState.DeletedByAdmin;
                    _postComment.SaveChanges();
                }

            }

            return Json(new { success = true });

        }
        public async Task<IActionResult> SendReportComment(long id, string reportText,
           [FromServices] UserManager<User> _userManager,
           [FromServices] IReportRepository _report,
           [FromServices] IPostCommentRepository _postComment
           
          )
        {

            
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var report = new Report
            {
                Description = reportText,
                PostCommentId = id,
                ReportState = ReportState.New,
                ReportTime = DateTime.Now,
                User = user,
                Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString()

            };
            _report.Add(report);
            _report.SaveChanges();
            if (user != null)
            {
                var comment = _postComment.PostComments.Where(pc => pc.Id == id).FirstOrDefault();
                comment.CommentState = CommentState.Reported;
                _postComment.SaveChanges();
            }
            return Json(new { success = true });

        }
        [HttpPost]
        public IActionResult MoreComments(string postId,int skip,int limit)
        {
            var postComments = ViewComponent("PostComments", new
            {
                postId,
                skip,
                limit
            });
            return postComments;
        }
        [Authorize(Roles = "MamiAdministrator")]
        [HttpPost]
        public async Task<IActionResult> Approve(string id,
            [FromServices] UserManager<User> _userManager,
            [FromServices] IMediaRepository _media,
            [FromServices] IMamiEventRepository _mamiEvent,
             [FromServices] IWebHostEnvironment environment,
             [FromServices] IConfiguration _configuration,
             [FromServices] IMamiFileManager _mamiFileManager)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var post = _posts.Posts.Where(p => p.ID == _hashId.Decode(id)).First();

            var medias = _media.Medias.Where(m => m.Post == post).ToList();
            var yearmonth = post.Created.Year.ToString() + ((post.Created.Month < 10) ? "0" + post.Created.Month.ToString() : post.Created.Month.ToString());
            
            foreach (var media in medias)
            {
                var temppath = media.Path;
                var typePath = "tempphotos";
                if (media.Type == "video" || media.Type == "PDF")
                {
                    typePath = "tempfiles";
                }

                var sourceFile = Path.Combine(environment.WebRootPath, media.Path);
                var tempmedia = media.Path;
                media.Path = media.Path.Replace(typePath, yearmonth);
                if (media.Type == "PDF")
                {
                    media.Path = media.Path.Replace("videos", "pdf");
                }
                var destinationFile = Path.Combine(environment.WebRootPath, media.Path.Replace(typePath, media.Path));
                try
                {
                    System.IO.File.Move(sourceFile, destinationFile);
                    _media.SaveChanges();
                }
                catch(Exception e)
                {
                    media.Path = temppath;
                    MamiEvent failEvent = new MamiEvent
                    {
                        Details = "خطا در انتقال فایل"+e.Message,
                        AffectedUserId = post.UserId,
                        EventTime = DateTime.Now,
                        Post = post,
                        User = user,
                        EventType = MamiEventType.ErrorMoveFile,
                        Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    _mamiEvent.Add(failEvent);
                    _mamiEvent.SaveChanges();


                    return RedirectToAction("FailFileMove", "Post");
                }
               



            }


            var lastPic= _media.Medias.Where(m => m.Post == post && m.Type=="photo").OrderBy(m=>m.SortOrder).LastOrDefault();
            if(lastPic != null)
            {
                    post.Thumbnail = "postthumbnail/" + yearmonth + "/" + post.Thumbnail.Replace(".mp4", ".jpg");
                    _mamiFileManager.GenerateThumbnailFromImage(Path.Combine(environment.WebRootPath, lastPic.Path),
                        Path.Combine(environment.WebRootPath, post.Thumbnail));
                
            }
            else
            {
                if (_media.Medias.Where(m => m.Post == post && m.Path.Contains(".mp3")).Count() > 0)
                {
                    post.Thumbnail = "images/videothumbnail.jpg";
                }
                else
                {
                    var thumbnail = Guid.NewGuid();
                    post.Thumbnail = "postthumbnail/" + yearmonth + "/" + thumbnail.ToString() + ".jpg";
                    var thumbphotopath = environment.WebRootPath + "/" + post.Thumbnail;
                    var video = _media.Medias.Where(m => m.Post == post && m.Type == "video").FirstOrDefault();
                    var sourceFile = Path.Combine(environment.WebRootPath, video.Path);
                    await _mamiFileManager.GenerateThumbnailFromVideo(sourceFile, thumbphotopath);
                }
            }
           
            post.Poststate = PostState.Approved;
            _posts.SaveChanges();
            MamiEvent newEvent = new MamiEvent
            {
                 Details = "تایید پست",
                 AffectedUserId = post.UserId,
                 EventTime = DateTime.Now,
                 Post = post,
                 User = user,
                 EventType = MamiEventType.ApprovePostByAdmin,
                 Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };
            _mamiEvent.Add(newEvent);
            _mamiEvent.SaveChanges();
            

            return RedirectToAction("NewPosts", "Admin", new { area = "Membership" });
           
            

        }
        [HttpPost]
        [Authorize(Roles = "MamiAdministrator")]
        [ValidateAntiForgeryToken]
        public IActionResult SetTitle(string title,int id)
        {
            var post = _posts.Posts.Where(p => p.ID == id).FirstOrDefault();
            post.Title = title;
            _posts.SaveChanges();
            return RedirectToAction("index", "home");
        }
        public IActionResult FailFileMove()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "MamiAdministrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetAdminViewDelete(string adminView, int id
            ,[FromServices] IMamiEventRepository _mamiEvent
            ,[FromServices] UserManager<User> _userManager)
        {
            var post = _posts.Posts.Where(p => p.ID == id).FirstOrDefault();
            post.AdminView = adminView;
            post.Poststate = PostState.DeletedByAdmin;
            _posts.SaveChanges();
            var user = await _userManager.GetUserAsync(HttpContext.User);

            MamiEvent newEvent = new MamiEvent
            {
                Details = "حذف پست توسط ادمین",
                AffectedUserId = post.UserId,
                EventTime = DateTime.Now,
                Post = post,
                User = user,
                EventType = MamiEventType.DeletePostByAdmin,
                Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };
            _mamiEvent.Add(newEvent);
            _mamiEvent.SaveChanges();
            return RedirectToAction("newposts","admin" , new { area = "Membership" });
        }

    }
}
