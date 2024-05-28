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
    public class PostSubmitViewModel
    {
       [Required]
    //    public string videoGuid { get; set; }
        public string[] imageGuids { get; set; }
        [Required]
        public int PostType { get; set; }

        [Required(ErrorMessage = "لطفا عنوان  را وارد کنید")]
        [Display(Name ="عنوان",Prompt ="لطفا از شکلک استفاده نکنید")]
        public string Title { get; set; }

        [Required(ErrorMessage = "لطفا توضیحات را وارد کنید")]
        [Display(Name = "توضیحات کامل را وارد کنید", Prompt = "توضیحات باید جامع باشد")]
        public string Description { get; set; }

        [Display(Name = "هشتگ ها", Prompt = ". لطفا حداکثر 5 هشتک مرتبط وارد کنید")]
        public string RawTags { get; set; }

        [Required(ErrorMessage = "یک {0} انتخاب کنید")]
        [Display(Name = "دسته بندی")]
        public int Category { get; set; }
        [Required(ErrorMessage = "یک {0} انتخاب کنید")]
        [Display(Name = "دسته بندی اصلی")]
        public int? MainCategory { get; set; }

        [Required(ErrorMessage = "قیمت را وارد کنید")]
        [Display(Name = "قیمت( تومان)",Prompt ="عدد انگلیسی وارد نمایید")]
        public int? Price { get; set; }
        [Required(ErrorMessage = "قیمت تخفیفی را وارد کنید")]
        [Display(Name = "قیمت با تخفیف (تومان)", Prompt = "عددانگلیسی وارد نمایید")]
        public int? DiscountPrice { get; set; }
        [Display(Name = "نحوه ارسال",Prompt ="رایگان، پیک،پست . . . ")]
        public string SendType { get; set; }
        [BindNever]
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<MainCategory> MainCategories { get; set; }
        public IEnumerable<PostType> PostTypes { get; set; }
    }
}
