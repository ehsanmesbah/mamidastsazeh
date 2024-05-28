"use strict";

$(function () {
    var mySwiper = new Swiper('.swiper-container', {
        slidesPerView: 2,
        spaceBetween: 10,

        pagination: {
            el: '.swiper-pagination',
            clickable: true,
        },
        breakpoints: {
            500: {
                slidesPerView: 3,
                spaceBetween: 40,
            },
            800: {
                slidesPerView: 5,
                spaceBetween: 50,
            }
        },
        loop: false,
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        }
    });
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var paneTarget = $(e.target).attr('href');
        var $thePane = $('.tab-pane' + paneTarget);
        var paneIndex = $thePane.index();
        if ($thePane.find('.swiper-container').length > 0 && 0 === $thePane.find('.swiper-slide-active').length) {
            mySwiper[paneIndex].update();
        }
    });

});