﻿"use strict";
$(window).on('load', function () {
    var navUserAgent = navigator.userAgent;
    var browserName = navigator.appName;
    var browserVersion = '' + parseFloat(navigator.appVersion);
    var majorVersion = parseInt(navigator.appVersion, 10);
    var tempNameOffset, tempVersionOffset, tempVersion;


    if ((tempVersionOffset = navUserAgent.indexOf("Opera")) != -1) {
        browserName = "Opera";
        browserVersion = navUserAgent.substring(tempVersionOffset + 6);
        if ((tempVersionOffset = navUserAgent.indexOf("Version")) != -1)
            browserVersion = navUserAgent.substring(tempVersionOffset + 8);
    } else if ((tempVersionOffset = navUserAgent.indexOf("MSIE")) != -1) {
        browserName = "Microsoft Internet Explorer";
        browserVersion = navUserAgent.substring(tempVersionOffset + 5);
    } else if ((tempVersionOffset = navUserAgent.indexOf("Chrome")) != -1) {
        browserName = "Chrome";
        browserVersion = navUserAgent.substring(tempVersionOffset + 7);
    } else if ((tempVersionOffset = navUserAgent.indexOf("Safari")) != -1) {
        browserName = "Safari";
        browserVersion = navUserAgent.substring(tempVersionOffset + 7);
        if ((tempVersionOffset = navUserAgent.indexOf("Version")) != -1)
            browserVersion = navUserAgent.substring(tempVersionOffset + 8);
    } else if ((tempVersionOffset = navUserAgent.indexOf("Firefox")) != -1) {
        browserName = "Firefox";
        browserVersion = navUserAgent.substring(tempVersionOffset + 8);
    } else if ((tempNameOffset = navUserAgent.lastIndexOf(' ') + 1) < (tempVersionOffset = navUserAgent.lastIndexOf('/'))) {
        browserName = navUserAgent.substring(tempNameOffset, tempVersionOffset);
        browserVersion = navUserAgent.substring(tempVersionOffset + 1);
        if (browserName.toLowerCase() == browserName.toUpperCase()) {
            browserName = navigator.appName;
        }
    }

    // trim version
    if ((tempVersion = browserVersion.indexOf(";")) != -1)
        browserVersion = browserVersion.substring(0, tempVersion);
    if ((tempVersion = browserVersion.indexOf(" ")) != -1)
        browserVersion = browserVersion.substring(0, tempVersion);
   
    if (browserName == "Safari") {

        $("#Password").attr("type", "text");
        $("#NewPassword").attr("type", "text");
        $("#OldPassword").attr("type", "text");
        $("#ConfirmPassword").attr("type", "text");
    }
});