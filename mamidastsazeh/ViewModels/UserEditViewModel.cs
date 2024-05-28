using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Display(Name = "نام")]
        public string FullName { get; set; }

        
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Display(Name = "موبایل")]
        public string Mobile { get; set; }

        [Display(Name = "آدرس ایمیل")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل معتبر نیست")]
        public string Email { get; set; }
        
   
        [Display(Name = "نام مستعار")]
        public string NickName { get; set; }

        [Display(Name = "اینستاگرام")]
        public string InstaAddress { get; set; }

        [Display(Name = "توضیحات صفحه")]
        public string Description { get; set; }

        [Display(Name = "امتیاز")]
        public double Score { get; set; }

        [Display(Name = "تاریخ ساخت")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "آخرین لاگین")]
        public DateTime? LastLogin { get; set; }

        [Display(Name = "فعال")]
        public bool? IsActive { get; set; }


        [Display(Name = "رمز عبور")]
        [UIHint("password")]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Compare(nameof(Password), ErrorMessage = "{0} و {1} با هم برابر نیستند")]
        [UIHint("password")]
        public string ConfirmPassword { get; set; }

        public string ProfileImage { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public User User => new User
        {
            Id = Id,
            FullName = FullName,
            UserName = UserName,
            Mobile=Mobile,
            Email = Email,
            CreatedDate=CreatedDate,
            Description=Description,
            InstaAddress=InstaAddress,
            Score=Score,
            NickName=NickName,
            ProfileImage=ProfileImage
        };

    }
}
