"use strict";

$(function () {
   
    $("#btnGet").click(function () {
             $("#btnGet").prop("disabled", true);
            $.ajax({

                type: "POST",
                url: "/Membership/Account/SendSmsAjaxMethod",
                data: {
                    "MobileNumber": $("#txtMobileNumber").val(),
                    "Type": $("#txtType").val(),
                    "DNTCaptchaText": $("#DNTCaptchaText").val(),
                    
                    "DNTCaptchaToken": $("#DNTCaptchaToken").val(),

                    "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
                    "DNTCaptchaInputText": $("#DNTCaptchaInputText").val()

                },

                success: function (response) {
                    if (response.MobileNumber != "0") {
                        $(".tab2").click();
                        $("#btnGet").prop("disabled", false);
                     }
                    else {
                        $("#errorLable").empty();
                        $("#errorLable").append(response.Message);
                        $("#dntCaptchaRefreshButton").click();
                        $("#btnGet").prop("disabled", false);

                    }
                },
                failure: function (response) {
                    $("#dntCaptchaRefreshButton").click();
                    $("#errorLable").empty();
                    $("#errorLable").append(response.responseText);
                    $("#btnGet").prop("disabled", false);
                },
                error: function (response) {
                    $("#dntCaptchaRefreshButton").click();
                    $("#errorLable").empty();
                    $("#errorLable").append(response.responseText);
                    $("#btnGet").prop("disabled", false);

                }
            });
        });
         $("#btnSendCode").click(function () {

        $.ajax({

            type: "POST",
            url: "/Membership/Account/VerifyCodeAjax",
            data: {
                "MobileNumber": $("#txtMobileNumber").val(),
                "Code": $("#txtCode").val(),
                "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
            },

            success: function (response) {


            },
            failure: function (response) {

            },
            error: function (response) {

            }
        });
                    });
                    $("#btnSendAgain").click(function () {
                         $("#errorLable").empty();
                        $("#txtCode").empty();
                        $(".tab1").click();
                        $("#dntCaptchaRefreshButton").click();
                    });

                });
