using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public class Media
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public virtual Post Post { get; set; }
        public long? instapostid { get; set; }
        public int? SortOrder { get; set; }
    }
}
