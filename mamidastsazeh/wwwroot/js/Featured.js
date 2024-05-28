"use strict";

$(function () {
    $(".btn-more-videos").click(function () {
        let parent = $(this).parents(".row:first");

        parent.addClass("d-none");
        parent.siblings(".more-videos:first").removeClass("d-none");
        parent.siblings(".less-videos-trigger:first").removeClass("d-none");
    });

    $(".btn-less-videos").click(function () {
        let parent = $(this).parents(".row:first");

        parent.addClass("d-none");
        parent.siblings(".more-videos:first").addClass("d-none");
        parent.siblings(".more-videos-trigger").removeClass("d-none");
    });
});