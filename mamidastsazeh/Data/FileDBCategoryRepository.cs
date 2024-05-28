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
    public class FileDBCategoryRepository 
    {
        private readonly IHostingEnvironment _environment;

        public FileDBCategoryRepository(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public IEnumerable<Category> Categories => GetCategories();

        private IEnumerable<Category> GetCategories()
        {
            var dbPath = Path.Combine(_environment.ContentRootPath, "Data", "Categories.db");

            var info = new string[5];
            var i = 0;
            using (var file = new StreamReader(dbPath))
            {
                var line = "";
                do
                {
                    line = file.ReadLine();
                    if (i < 5)
                    {
                        info[i] = line;
                        i++;
                    }
                    else
                    {
                        i = 0;
                        yield return new Category
                        {
                            Id = int.Parse(info[0]),
                            Name = info[1],
                            Icon=info[2],
                            Promoted=bool.Parse(info[3]),
                            Prominent=bool.Parse(info[4])
                        };
                    }

                } while (line != null);
            }
        }
    }
}
