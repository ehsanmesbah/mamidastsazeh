"use strict";

$(function () {var swiper = new Swiper(".mySwiper", {
  
    spaceBetween: 10,
    slidesPerView: 3,
    freeMode: true,
    
    watchSlidesProgress: true,
});
var swiper2 = new Swiper(".mySwiper2", {
    autoHeight: true, 
    
    spaceBetween: 10,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    thumbs: {
        swiper: swiper,
    },
});
});