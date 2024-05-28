using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public class PostView
    {
        public long Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public  DateTime ViewDate { get; set; }

    }
}
