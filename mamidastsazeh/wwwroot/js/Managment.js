"use strict";

function acceptCommentReport(obj) {
    
    $.ajax({

        type: "POST",
        url: "/membership/admin/AcceptCommentReport",
        data: {
            "id": obj.getAttribute("data-id"),
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
        },

        success: function (response) {
            if (response.success) {
                $(obj).parent().parent(".ReportRow").remove();
            }
            else alert("fail");
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });
        /*$.ajax({

            type: "POST",
            url: "/Membership/Admin/AcceptCommentReport",
            data: {
                "id": obj.getAttribute("data-id")
               
            },

            success: function (response) {
                if (response.success) {
                    $(obj).parent().parent(".ReportRow").remove();
                }
                else alert("fail");

            },
            failure: function (response) {
                alert(response.data);
            },
            error: function (response) {
                alert(response.data);
            }
        });
    */
}
function rejectCommentReport(obj) {

    $.ajax({

        type: "POST",
        url: "/Membership/Admin/RejectCommentReport",
        data: {
            "id": obj.getAttribute("data-id"),
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
        },

        success: function (response) {
            
            if (response.success) {
                $(obj).parent().parent(".ReportRow").remove();
                
            }
            else alert("fail");

        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });

}
function approveComment(obj) {

    $.ajax({

        type: "POST",
        url: "/membership/admin/ApproveComment",
        data: {
            "id": obj.getAttribute("data-id"),
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
        },

        success: function (response) {
            if (response.success) {
                $(obj).parent().parent(".CommentRow").remove();
            }
            else alert("fail");
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });
    /*$.ajax({

        type: "POST",
        url: "/Membership/Admin/AcceptCommentReport",
        data: {
            "id": obj.getAttribute("data-id")
           
        },

        success: function (response) {
            if (response.success) {
                $(obj).parent().parent(".ReportRow").remove();
            }
            else alert("fail");

        },
        failure: function (response) {
            alert(response.data);
        },
        error: function (response) {
            alert(response.data);
        }
    });
*/
}
function rejectComment(obj) {

    $.ajax({

        type: "POST",
        url: "/Membership/Admin/RejectComment",
        data: {
            "id": obj.getAttribute("data-id"),
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
        },

        success: function (response) {

            if (response.success) {
                $(obj).parent().parent(".CommentRow").remove();

            }
            else alert("fail");

        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });

}
