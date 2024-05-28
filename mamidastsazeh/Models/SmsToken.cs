using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public class SmsToken
    {
        [Key]
        public int Id { get; set; }
        public string TokenKey { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public DateTime GetDate { get; set; }
    }
}
