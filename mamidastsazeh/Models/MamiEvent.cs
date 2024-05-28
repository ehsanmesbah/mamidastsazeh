using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public enum MamiEventType
    {
        Login=0,
        Loutout=1,
        Register=2,
        SendSms=3,
        NewPost=4,
        ApprovePostByAdmin=5,
        RejectPostByAdmin=6,
        SetForEditByAdmin=7,
        DisableUserByAdmin=8,
        ResetPassword=9,
        ReserPasswordByAdmin=10,
        UpdateUser=11,
        UpdateUserByAdmin=12,
        DeletePost=13,
        DeletePostByAdmin=14,
        DisableUserRequest=15,
        ReportPost=16,
        ReportComment=17,
        SendComment=18,
        DeleteComment=19,
        DeleteCommentByAdmin=20,
        ApproveCommentByAdmin=21,
        uploadfile=22,
        LoginBySms=23,
        ErrorSendPost=24,
        ErrorUploadFile=25,
        SuspendUser=26,
        ErrorMoveFile=27

    }
    public class MamiEvent
    {
        
        [Key]
        public long Id { get; set; }
        public MamiEventType EventType { get; set; }  

        public string Ip { get; set; }
        public string Details { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int? PostID { get; set; }
        public virtual Post Post { get; set; }
        public string  AffectedUserId { get; set; }
        public DateTime EventTime { get; set; }

    }
}
