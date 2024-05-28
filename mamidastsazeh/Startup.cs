using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mamidastsazeh.Abstractions;
using mamidastsazeh.Data;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mamidastsazeh.Extensions.DependencyInjection;
using mamidastsazeh.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using mamidastsazeh.Models;
using mamidastsazeh.Areas.Membership.Services;
using DNTCaptcha.Core;
using Newtonsoft.Json.Serialization;
using WebEssentials.AspNetCore.Pwa;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http.Features;

namespace mamidastsazeh
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IHashId, HashId>();
            services.AddScoped<ICheckUserPermission, CheckUserPermission>();
            services.AddScoped<IMamiFileManager, MamiFileManager>();
            services.AddScoped<ISmsManager, SmsManager>();
            services.AddDbContext<mamidastsazehContext>(optionsBuilder => {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_configuration["Data:Mami"]);
                //optionsBuilder.UseLazyLoadingProxies().UseMySQL(_configuration["Data:Mami"]);
            });
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IContentTypeRepository, ContentTypeRepository>();
            services.AddTransient<IMediaRepository, MediaRepository>();
            services.AddTransient<IPostCommentRepository, PostCommentRepository>();
            services.AddTransient<IPostLikeRepository, PostLikeRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IPostTagRepository, PostTagRepository>();
            services.AddTransient<IPostTypeRepository, PostTypeRepository>();
            services.AddTransient<IPostViewRepository, PostViewRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IsmsRepository, SmsRepository>();
            services.AddTransient<IMainCategoryRepository, MainCategoryRepository>();
            services.AddTransient<IsmsTokenRepository, SmsTokenRepository>();
            services.AddTransient<IMamiEventRepository, MamiEventRepository>();
            services.AddTransient<ICommentLikeRepository, CommentLikeRepository>();
            services.AddTransient<IRestrictedWordRepository,RestrictedWordRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<ICommentLikeRepository, CommentLikeRepository>();
            services.AddTransient<IProvinceRepository, ProvinceRepository>();
            services.AddIdentity<User, IdentityRole>(options => {
                options.User.AllowedUserNameCharacters = "0987654321";
                

                //options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                //options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 6;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Lockout.AllowedForNewUsers = true;
               

            })
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
              .AddEntityFrameworkStores<mamidastsazehContext>()
              .AddDefaultTokenProviders();

            services.AddDNTCaptcha(options =>
             options.UseCookieStorageProvider() // -> It relies on the server and client's times. It's ideal for scalability, because it doesn't save anything in the server's memory.
            );
            //services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Authenticate/Login");
            services.ConfigureApplicationCookie(options =>
            {
                
                options.LoginPath =new PathString( "/membership/Account/Login");
                options.LogoutPath =new PathString("/membership/Account/Logout");
               
            });
            

            services.AddMami();
            // services.AddControllersWithViews();

            //services.AddControllers().;

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = 100000000;
                x.MultipartBodyLengthLimit = 100000000; // In case of multipart
            });

            //            services.AddProgressiveWebApp(new PwaOptions { RegisterServiceWorker = false });

            services.AddMiniProfiler(options=> {
                options.PopupRenderPosition = StackExchange.Profiling.RenderPosition.BottomLeft;
                options.PopupShowTimeWithChildren = true;
            }).AddEntityFramework();

            services.AddDataProtection()
           .PersistKeysToFileSystem(new DirectoryInfo(_configuration["KeyPath:DirPath"] ))
           .SetApplicationName("mamiwebservice")
           .ProtectKeysWithCertificate(new X509Certificate2(_configuration["KeyPath:CertPath"], "Aft@b8090ssl"))
           .SetDefaultKeyLifetime(TimeSpan.FromDays(9000));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage(new DeveloperExceptionPageOptions
                { SourceCodeLineCount = 10 });
                app.UseBrowserLink();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseAuthorization();
          


            app.UseMiniProfiler();
        

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();

              
                endpoints.MapAreaControllerRoute(
                  name: "UploadPost",
                  areaName: "Membership",
                  pattern: "Membership/Panel/UploadPost/{postTypeString?}",
                  defaults: new { controller = "Panel", action = "UploadPost" }
                  );
                endpoints.MapAreaControllerRoute(
                  name: "Account",
                  areaName: "Membership",
                  pattern: "Membership/{controller}/{action}",
                  defaults: new { controller = "Panel", action = "UserSettings" }
                  );

                endpoints.MapControllerRoute(
                    name: "Post",
                    pattern: "post/{hashId}/{text?}",
                    defaults: new { controller = "Post", action = "Index" }
                    );

                endpoints.MapControllerRoute(
                    name: "Category",
                    pattern: "category/{categoryHashId}/{pagetype?}/{page?}/{sortorder?}/{categoryName?}/{categoryType?}",
                    defaults: new { controller = "Category", action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name: "UserPosts",
                    pattern: "userposts/{pagetype}/{page}/{sortorder}/{nickname}",
                    defaults: new { controller = "UserPosts", action = "Index" }
                    );
                endpoints.MapControllerRoute(
                  name: "SearchPage",
                  pattern: "searchpage/",
                  defaults: new { controller = "SearchPage", action = "Index" }
                  );
                endpoints.MapControllerRoute(
                  name: "SearchPageResult",
                  pattern: "searchresult/{pagetype}/{page}/{limit}/{sortorder}/{searchTerm}",
                  defaults: new { controller = "SearchPage", action = "Search" }
                  );
                
                //endpoints.MapControllerRoute(
                //  name: "test",
                //  pattern: "Test",
                //  defaults: new { controller = "Test", action = "Index" }
                //  );
                endpoints.MapControllerRoute(
                 name: "AllMainCategory",
                 pattern: "maincategory/allmaincategory/{pageType}",
                 defaults: new { controller = "MainCategory", action = "AllMainCategory" }
                 );
                endpoints.MapControllerRoute(
                  name: "MainCategory",
                  pattern: "maincategory/{hashId}/{maincategoryName}/{categoryType?}",
                  defaults: new { controller = "MainCategory", action = "Index" }
                  );
                endpoints.MapControllerRoute(
                 name: "SpecialPost",
                 pattern: "specialpost/{pageType}/{pageNumber?}",
                 defaults: new { controller = "SpecialPost", action = "Index" }
                 );
                endpoints.MapControllerRoute(
                    name: "CustomizedType",
                    pattern: "CustomizedType/{page?}/{hashId?}/{categoryName?}",
                    defaults: new { controller = "CustomizedType", action = "Index" }
                );
                //endpoints.MapControllerRoute(
                //    name: "Register",
                //    pattern: "Register/{action}",
                //    defaults: new { controller = "Register", action = " Index" }
                //);
                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{controller}/{action}",
                    defaults: new { controller = "Home", action = "Index" }
                );
            });
          
        }
    }
}
