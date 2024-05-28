using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.ViewModels
{
    public class PostSliderViewModel
    {
        public Category Category{get;set;}
        public IEnumerable<Post> Posts { get; set; }
        public string postListType { get; set; }
    }
}
