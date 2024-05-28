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
    public class UserUpdateViewModel
    {
       
        [Display(Name ="نام کاربری",Prompt ="نام کاریری")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "لطفا نام مستعار را وارد کنید")]
        [Display(Name = "نام مستعار(سایر کاربران شما را با این نام میشناسند)",Prompt = "نام مستعار(سایر کاربران شما را با این نام میشناسند)")]
        public string NickName { get; set; }

       
        [Display(Name = "نام (سایر کاربران این نام را نمی بینند)",Prompt = "نام (سایر کاربران این نام را نمی بینند)")]
        public string FullName { get; set; }


        [EmailAddress(ErrorMessage = "{0} وارد شده صحیح نیست")]
        [Display(Name = "آدرس ایمیل(اختیاری-ایمیل ندارید خالی بماند)",Prompt = "آدرس ایمیل(اختیاری-ایمیل ندارید خالی بماند)")]
        public string Email { get; set; }

        [Display(Name = "صفحه اینستاگرام (بدون آدرس سایت مثلا @mamidastsazeh)", Prompt = "صفحه اینستاگرام (مثلا @mamidastsazeh)")]
        public string InstaAddress { get; set; }

        [DataType(DataType.Upload)]
        [MaxFileSize(10 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".gif", ".jpeg" })]
        [Display(Name = "تصویر پروفایل", Prompt = "تصویر پروفایل")]
        public IFormFile ProfileImage { get; set; }

      
        
        [Display(Name = "بیو", Prompt = "توضیحات")]
        public string Description { get; set; }
       [Display(Name = "آدرس اینترنتی در سایت مامی", Prompt = " لطفا انگلیسی وارد شود. این آدرس اختصاصی صفحه شما در سایت است")]
        public string BusinessName { get; set; }
        
        [Display(Name = "فروشگاه آنلاین دارم")]
        public bool IsBusiness { get; set; }
        [Display(Name = "استان", Prompt = "استان")]
        public int? Province { get; set; }
        [Display(Name = "شهر", Prompt = "شهر")]
        public string City { get; set; }
        [Display(Name = "عنوان فروشگاه", Prompt = "نام فارسی فروشگاه")]
        public string BusinessPersianName { get; set; }

        [BindNever]
        public IEnumerable<Province> Provinces { get; set; }


    }
}
