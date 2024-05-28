using mamidastsazeh.Areas.Membership.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Models;

namespace mamidastsazeh.ViewModels
{
    public class PostApproveViewModel
    {
       [Required]

       public string[] videoGuid { get; set; }

        public Post post { get; set; }

        [BindNever]
        public IEnumerable<Category> Categories { get; set; }

    }
}
