using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.ViewModels
{
    public class SendSmsApiResult
    {
        public double VerificationCodeId { get;set;}
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        
    }
}
