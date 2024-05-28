using mamidastsazeh.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace mamidastsazeh.Data
{
    public class FileDBPostRepository 
    {
        private readonly IHostingEnvironment _environment;
        private readonly ICategoryRepository _categories;
        private readonly IContentTypeRepository _contenttypes;
        private readonly IPostTypeRepository _postypes;

        public FileDBPostRepository(IHostingEnvironment environment, ICategoryRepository categories,IContentTypeRepository contenttypes, IPostTypeRepository posttypes)
        {
            _environment = environment;
            _categories = categories;
            _contenttypes = contenttypes;
            _postypes = posttypes;
        }

        public IEnumerable<Post> Posts => GetPosts();

        private IEnumerable<Post> GetPosts()
        {
            var dbPath = Path.Combine(_environment.ContentRootPath, "Data", "Posts.db");

            using (var file = new StreamReader(dbPath))
            {
                var line = "";
                var i = 0;
                var info = new string[10];
                do
                {
                    line = file.ReadLine();
                    if (i < 10)
                    {
                        info[i] = line;
                        i++;
                    }
                    else
                    {
                        i = 0;

                        yield return new Post
                        {
                            ID = int.Parse(info[0]),
                            FileName = info[1],
                            Thumbnail = info[2],
                            Title = info[3],
                            Description = info[4],
                            Category = _categories.Categories.Where(c => c.Id == int.Parse(info[5])).FirstOrDefault(),
                          
                            NumberOfLikes = int.Parse(info[6]),
                            NumberOfComments= int.Parse(info[7]), 
                            NumberOfViews= int.Parse(info[8]),
                           
                            Created = DateTime.Parse(info[9]),
                            PostType = _postypes.PostTypes.Where(c => c.ID == 1).FirstOrDefault(),
                            ContentType = _contenttypes.ContentTypes.Where(c => c.ID == 1).FirstOrDefault()


                        };
                    }

                } while (line != null);
            }
        }
    }
}
