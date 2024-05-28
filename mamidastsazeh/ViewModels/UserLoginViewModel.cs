using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.ViewModels
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "{0} نمی تواند خالی باشد.")]
        [Display(Name = "شماره موبایل", Prompt = "شماره موبایل")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} نمی تواند خالی باشد.")]
        [Display(Name = "رمز عبور", Prompt = "رمز عبور")]
        [UIHint("password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
  
    }
}
