using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Data;
using mamidastsazeh.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using mamidastsazeh.Components;

namespace mamidastsazeh.Controllers
{
    public class SeedController:Controller
    {
        private IHostingEnvironment _environment;


        public SeedController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public int Populate()
        {
            var categories = new FileDBCategoryRepository(_environment);
            var contenttypes = new FileDBContentTypeRepository(_environment);
            var posttypes = new FileDBPostTypeRepository(_environment);
            var posts = new FileDBPostRepository(_environment, categories,contenttypes,posttypes);

            var optionsBuilder = new DbContextOptionsBuilder<mamidastsazehContext>();
            var conString = @"Server=(localdb)\MSSQLLocalDB;Database=mamidastsazeh;Trusted_Connection=True";
            optionsBuilder
                .UseSqlServer(conString)
                .EnableSensitiveDataLogging();
                
            var count = 0;
            using (var context = new mamidastsazehContext(optionsBuilder.Options))
            {
                var transaction = context.Database.BeginTransaction();
               
                /*
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Category ON");
                
                if (!context.Category.Any())
                {
                    context.Category.AddRange(categories.Categories);
                    count += context.SaveChanges();
                }
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Category OFF");
                
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT PostType ON");
                if (!context.PostType.Any())
                {
                    context.PostType.AddRange(posttypes.PostTypes);
                    count += context.SaveChanges();
                }
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT PostType OFF");
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT ContentType ON");
                if (!context.ContentType.Any())
                {
                    context.ContentType.AddRange(contenttypes.ContentTypes);
                    count += context.SaveChanges();
                }
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT ContentType OFF");
               
    */
               

                if (!context.Post.Any())
                {
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Post ON");

                    foreach (var item in posts.Posts) {
                        Category cat = context.Category.Find(item.Category.Id);
                        item.Category = cat;
                        ContentType con = context.ContentType.Find(item.ContentType.ID);
                        item.ContentType = con;
                        PostType pt = context.PostType.Find(item.PostType.ID);
                        item.PostType = pt;

                        context.Post.Add(item);
                       
                        
                
                        count += context.SaveChanges();

                    }
                    
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Post OFF");

                }
      


            
                transaction.Commit();
            }

            return count;
        }
    }
}
