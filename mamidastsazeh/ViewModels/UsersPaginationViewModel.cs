using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.ViewModels
{
    public class UsersPaginationViewModel
    {
        public IEnumerable<User> Users { get; set; }

        public IEnumerable<PaginationButton> Buttons { get; set; }

        public int Page { get; set; }
        public int Limit { get; set; }

    }

    public class PaginationButton
    {
        public string Label { get; set; }
        public bool IsActive { get; set; }
        public bool Disabled { get; set; }
        public int Page { get; set; }
       

    }
}
