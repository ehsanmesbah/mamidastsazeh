"use strict";


function alertLogin(obj) {
    alert("برای لایک یا ارسال نظر باید به حساب کاربری خود وارد شوید");

}
function callLogin(returnUrl) {
    window.location.replace("https://mamidastsazeh.com/Membership/Account/Login?returnUrl=" + returnUrl);
}
function likePostFunction(obj) {
    if (obj.getAttribute("data-type") == "Post") {
        $.ajax({

            type: "POST",
            url: "/Post/LikePost",
            data: {
                "id": obj.getAttribute("data-id"),
                "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
            },

            success: function (response) {
                if ($(obj).hasClass("far")) {
                    $(obj).removeClass("far");
                    $(obj).addClass("fas");
                    $(".NumberOfPostLikes").text(response.numberOfLikes);
                }
                else {
                    $(obj).removeClass("fas");
                    $(obj).addClass("far");
                    $(".NumberOfPostLikes").text(response.numberOfLikes);
                }

            },
            failure: function (response) {

            },
            error: function (response) {

            }
        });
    }
}
function likeCommentFunction(obj) {


    $.ajax({

        type: "POST",
        url: "/Post/LikeComment",
        data: {
            "id": obj.getAttribute("data-id"),
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
        },

        success: function (response) {
            if ($(obj).hasClass("far")) {
                $(obj).removeClass("far");
                $(obj).addClass("fas");
                $(obj).next().text(parseInt($(obj).next().text()) + 1);

            }
            else {
                $(obj).removeClass("fas");
                $(obj).addClass("far");
                $(obj).next().text(parseInt($(obj).next().text()) - 1);

            }

        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });

}
function deleteComment(obj) {


    $.ajax({

        type: "POST",
        url: "/Post/DeleteComment",
        data: {
            "id": obj.getAttribute("data-id"),
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
        },

        success: function (response) {
            $(obj).parents(".comment").remove();

        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });

}
function askDelete(obj) {

    $(obj).next(".askForDelete").removeClass("d-none");
    $(obj).next(".askForDelete").addClass("d-flex");
}
function closeDeleteSpan(obj) {
    $(obj).parent(".askForDelete").addClass("d-none");
    $(obj).parent(".askForDelete").removeClass("d-flex");
}
function reportComment(obj) {
    $(obj).next(".ReportDiv").removeClass("d-none");

    $(obj).next(".ReportDiv").addClass("d-flex");
}
function closeReportComment(obj) {
    $(obj).parent(".ReportDiv").addClass("d-none");
    $(obj).parent(".ReportDiv").removeClass("d-flex");

}
function sendReportComment(obj) {
    $.ajax({

        type: "POST",
        url: "/Post/SendReportComment",
        data: {
            "id": obj.getAttribute("data-id"),
            "reportText": $(obj).prev(".reportText").val(),
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
        },

        success: function (response) {
            $(obj).parent(".ReportDiv").prev(".reportCommentBtn").text("گزارش شما دریافت شد");
            $(obj).parent(".ReportDiv").prev(".reportCommentBtn").prop('disabled', true);
            $(obj).parent(".ReportDiv").addClass("d-none");
            $(obj).parent(".ReportDiv").removeClass("d-flex");
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });
}
function moreComments(obj) {
    $.ajax({

        type: "POST",
        url: "/Post/MoreComments",
        data: {
            "postId": obj.getAttribute("data-postId"),
            "skip": obj.getAttribute("data-skip"),
            "limit": obj.getAttribute("data-limit"),
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
        },

        success: function (data) {
            $(obj).attr("data-skip", parseInt(obj.getAttribute("data-skip")) + 10);

            $(obj).parent().find(".CommentsHolder").append('<div id="CommentsHolder' + obj.getAttribute("data-skip") + '">' + data + '</div>');
            var scroll = "#CommentsHolder" + obj.getAttribute("data-skip");
            $('html, body').animate({
                scrollTop: $(scroll).offset().top
            }, 2000);

        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });

}



function toggleEdit() {

    $("#editDiv").toggle();

};
