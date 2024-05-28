using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }
        public bool? Approved { get; set; }
        public int? NumberOfPost { get; set; }
        public int? catid { get; set; }


    }
}
