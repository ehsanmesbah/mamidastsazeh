using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public enum ReportState
    {
        UnSet=0,
        New=1,
        Rejected=2,
        Accepted=3

    }
    public class Report
    {
        public int Id { get; set; }

        public string Description { get; set; }
        public int? PostId { get; set; }
        public virtual  Post Post { get; set; }
        public long? PostCommentId { get; set; }
        public virtual PostComment PostComment { get; set; }
        public DateTime ReportTime { get; set; }
        public ReportState ReportState{ get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string Ip { get; set; }
    }
}
