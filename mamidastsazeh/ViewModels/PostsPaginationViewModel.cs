using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.ViewModels
{
    public class PostsPaginationViewModel
    {
        public IEnumerable<Post> Posts { get; set; }

        public IEnumerable<PaginationButton> Buttons { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "فقط اعداد مثبت")]
        public int Page { get; set; }
        [Range(1, 200, ErrorMessage = "بین 1 تا 200")]
        public int Limit { get; set; }

        public string PageType { get; set; }
        public int Total { get; set; }
        public string SortOrder { get; set; }
        public string SearchTerm { get; set; }
        public string CategoryHashId { get; set; }
        public string CategoryName { get; set; }
        public string NickName { get; set; }
        public string CategoryType { get; set; }
        public User User { get; set; }
        public PostsPaginationViewModel()
        {
            Page = 1;
            Limit = 24;
            Total = 0;
            SortOrder = "Created-desc";
            PageType = "category";
            

        }
        
    }


}
