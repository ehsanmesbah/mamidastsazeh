"use strict";
$(function () {
    $(window).on("load resize", function () {
        if (this.matchMedia("(min-width: 768px)").matches) {
            $(".dropdown").hover(

                function () {
                    const $this = $(this);
                    $this.addClass("show");
                    $this.find($(".dropdown-toggle")).attr("aria-expanded", "true");
                    $this.find($(".dropdown-menu")).addClass("show");
                },
                function () {
                    const $this = $(this);
                    $this.removeClass("show");
                    $this.find($(".dropdown-toggle")).attr("aria-expanded", "false");
                    $this.find($(".dropdown-menu")).removeClass("show");
                }
            );
        } else {
            $(".dropdown").off("mouseenter mouseleave");
        }
    });
});
