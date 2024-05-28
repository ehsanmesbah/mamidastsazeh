"use strict";

$(function () {
    let isSearching = false;

    //Library URL: http://manos.malihu.gr/jquery-custom-content-scroller/#get-started-section
    // Theme URL: http://manos.malihu.gr/repository/custom-scrollbar/demo/examples/scrollbar_themes_demo.html
    function bindScrollbar() {
        $(".scrollable").mCustomScrollbar({
            theme: "dark-3"
        });
    }

    function openSuggestions() {
        let body = $("body");
        let container = $(".search-suggest");
        let overlay = $(".search-overlay");

        if (!body.hasClass("has-suggestions")) {
            body.addClass("has-suggestions ");
        }

        if (!overlay.hasClass("visible")) {
            overlay.addClass("visible");
        }

        if (!container.hasClass("visible")) {
            container.addClass("visible");
        }

        fillSuggestions("");
    }

    function closeSuggestions() {
        let body = $("body");
        let container = $(".search-suggest");
        let overlay = $(".search-overlay");

        container.removeClass("visible");
        overlay.removeClass("visible");
        body.removeClass("has-suggestions");
    }

    function fillSuggestions(content) {
        let container = $(".search-suggest");
        container.html(content);
        bindScrollbar();
    }

    function doSearch(term) {
        if (term.length < 3) {
            closeSuggestions();
            isSearching = false;
            return;
        }
        if (!isSearching) {
            isSearching = true;
            openSuggestions();

            let jqXhr = $.get("/Ajax/Search", { term: term });

            jqXhr.done(function (response) {
                fillSuggestions(response);
            })

            jqXhr.fail(function () {
            })

            jqXhr.always(function () {
                isSearching = false;
            })
        }
    }

    $(".search input").keyup(function () {
        let term = $(this).val();
        doSearch(term);
    })

    $(".search-overlay").click(function () {
        closeSuggestions();
    })
  
});

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}