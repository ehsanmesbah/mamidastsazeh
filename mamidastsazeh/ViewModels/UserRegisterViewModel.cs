using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "لطفا  موبایل را وارد کنید")]
        [Display(Name = "موبایل(جهت ارسال کد اعتبار سنجی)",Prompt = "موبایل(جهت ارسال کد اعتبار سنجی)")]
        
        public string Mobile { get; set; }

        [Required(ErrorMessage = "لطفا نام مستعار را وارد کنید")]
        [Display(Name = "نام مستعار(سایر کاربران شما را با این نام میشناسند)",Prompt = "نام مستعار(سایر کاربران شما را با این نام میشناسند)")]
        public string NickName { get; set; }


        
      

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "رمز عبور",Prompt ="رمز عبور(حداقل طول 6- ترکیب حرف و عدد)")]
        [UIHint("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Compare(nameof(Password), ErrorMessage = "{0} و {1} با هم برابر نیستند")]
        [Display(Name = "تایید رمز عبور",Prompt = "تایید رمز عبور")]
        [UIHint("password")]
        public string ConfirmPassword { get; set; }
        
       
        [Required]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }
    }
}
