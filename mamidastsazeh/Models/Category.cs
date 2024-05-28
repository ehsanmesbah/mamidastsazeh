using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public enum CategoryType
    {
        All=1,
        Training=2,
        Product=3
    };
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int? sortorder { get; set; }
        public CategoryType? CategoryType { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual MainCategory MainCategory { get; set; }
        

        }
}
