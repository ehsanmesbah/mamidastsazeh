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
    public class User : IdentityUser {


        [Display(Name = "نام")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "مقدار  {0} الزامی می باشد")]
        [Display(Name = "اسم مستعار")]
        public string NickName { get; set; }



        [Required(ErrorMessage = "مقدار  {0} الزامی می باشد")]
        [Display(Name = "شماره موبایل")]

        public string Mobile { get; set; }
        public string Description { get; set; }
        public double Score { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastLogin { get; set; }

        public string InstaAddress { get; set; }

        public string ProfileImage { get; set; }

        public string WorkPhone { get; set; }
        public bool IsBusinessAccount { get; set; }
        public string BusinessName {get;set;}
        public string Website { get; set; }
        public string BusinessAddress { get; set; }
        public string CityName { get; set; }

        public string BusinessPersianName { get; set; }
        /*
        [Compare(nameof(AlwaysTrue), ErrorMessage = "شما باید با قوانین سایت موافقت نمایید")]
        
        public bool? TermsAccepted { get; set; }

        [BindNever]
        public bool? AlwaysTrue { get; set; } = true;*/
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<PostComment> Comments { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public virtual ICollection<PostView> PostView { get; set; }
        public string UniquePrefix { get; set; }
        public virtual ICollection<Sms> Smss { get; set; }
        public virtual ICollection<MamiEvent> MamiEvents { get; set; }
        public virtual ICollection<CommentLike> CommentLikes { get; set; }

        public int? ProvinceId { get; set; }
        public virtual Province Province { get; set; }

        public bool FalseReporting { get; set; }
      

    }
}
