using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public class MobileViewModel
    {
        ///<summary>
        /// Gets or sets Name.
        ///</summary>
        ///
        [Required(ErrorMessage = "لطفا  موبایل را وارد کنید")]
        [Display(Name = "موبایل-حتما اعداد انگلیسی", Prompt = "موبایل(جهت ارسال کد اعتبار سنجی)")]
        
        [RegularExpression("^09[0-9]{9}$", ErrorMessage ="شماره موبایل وارد شده صحیح نمی باشد یا کیبرد شما فارسی است")]
        public string MobileNumber { get; set; }

        [Display(Name = " کد دریافتی از پیامک را وارد نمایید", Prompt = "")]
        public string Code { get; set; }
        public string Validate { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string ReturnUrl { get; set; }
        ///<summary>
        /// Gets or sets DateTime.
        ///</summary>
    }
}
