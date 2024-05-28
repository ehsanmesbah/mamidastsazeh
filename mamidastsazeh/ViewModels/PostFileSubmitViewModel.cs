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
    public class PostFileSubmitViewModel
    {
        [Required]
        public string videoGuid { get; set; }

        
        [DataType(DataType.Upload)]
        [MaxFileSize(15 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".gif", ".jpeg",".bmp",".jpeg",".gif", ".heif", ".heic" })]
        [Display(Name = "تصویر کاور(در صورت عدم ارسال از فیلم بصورت اتوماتیک انتخاب می شود)", Prompt = "تصویر کاور")]
        public IFormFile Thumbnail { get; set; }

        [Required(ErrorMessage = "لطفا عنوان را وارد کنید")]
        [Display(Name ="عنوان",Prompt ="عنوان")]
        public string Title { get; set; }

        [Required(ErrorMessage = "لطفا توضیحات را وارد کنید")]
        [Display(Name = "لطفا توضیحات را وارد کنید", Prompt = "لطفا توضیحات را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "تگ ها", Prompt = "لطفا تگ ها را وارد کنید")]
        public string RawTags { get; set; }

        [Required(ErrorMessage = "یک {0} انتخاب کنید")]
        [Display(Name = "مجموعه")]
        public int Category { get; set; }

        [BindNever]
        public IEnumerable<Category> Categories { get; set; }

    }
}
