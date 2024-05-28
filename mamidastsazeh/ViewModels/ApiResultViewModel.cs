using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.ViewModels
{
    public class ApiResult
    {
        public string TokenKey { get;set;}
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        
    }
}
