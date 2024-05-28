using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.ViewModels
{
    public class UpdateUserPasswordViewModel
    {
       
        [Display(Name = "رمز عبور فعلی", Prompt = "رمز عبور فعلی")]
        [UIHint("password")]
        public string OldPassword { get; set; }

      
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "رمز عبور جدید",Prompt ="رمز عبور(حداقل طول 6- ترکیب حرف و عدد)")]
        [UIHint("password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Compare(nameof(NewPassword), ErrorMessage = "{0} و {1} با هم برابر نیستند")]
        [Display(Name = "تایید رمز عبور",Prompt = "تایید رمز عبور")]
        [UIHint("password")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
        
        


    }
}
