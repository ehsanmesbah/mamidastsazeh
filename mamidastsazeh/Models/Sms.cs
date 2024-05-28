using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public class Sms
    {
        [Key]
        public int Id { get; set; }
        public string MobileNumber { get; set; }
        public string Code { get; set; }
        public DateTime SendTime { get; set; }
        public bool Validate { get; set; }
        public DateTime ValidateTime { get; set; }
        public string Ip { get; set; }
        public string Type { get; set; }
        public virtual User User { get; set; }
        public double? VerificationCodeId { get; set; }
        public bool? IsSuccessful { get; set; }
        public string ServerMessage { get; set; }
    }
}
