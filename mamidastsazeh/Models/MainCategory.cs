using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public class MainCategory
    {
        public int Id { get; set; }
        public string maincategoryname { get; set; }
        public int? sortorder { get; set; }
        public virtual ICollection<Category> Categories { get; set; }

    }
}
