using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class FileDBPostTypeRepository:IPostTypeRepository
    {
        private readonly IHostingEnvironment _environment;
        public FileDBPostTypeRepository(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public IEnumerable<PostType> PostTypes => GetPostTypes();

        private IEnumerable<PostType> GetPostTypes()
        {
            var dbPath = Path.Combine(_environment.ContentRootPath, "Data", "PostType.db");

            var info = new string[2];
            var i = 0;
            using (var file = new StreamReader(dbPath))
            {
                var line = "";
                do
                {
                    line = file.ReadLine();
                    if (i < 2)
                    {
                        info[i] = line;
                        i++;
                    }
                    else
                    {
                        i = 0;
                        yield return new PostType
                        {
                            ID = int.Parse(info[0]),
                            Name = info[1]
                        };
                    }

                } while (line != null);
            }

        }
    }
}
