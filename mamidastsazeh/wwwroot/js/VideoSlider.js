"use strict";

function adjustSliderNavigation() {
    let videoThumbnailHeight = $(".video-thumbnail:first").height();
    let btnHeight = $(".btn.next:first").height();

    let marginTop = (videoThumbnailHeight / 2) - (btnHeight / 2);

    $(".btn.next, .btn.prev").css("margin-top", `${marginTop}px`);
}

function slider(container) {
    let _slider = {};

    _slider.container = container;
    _slider.containerWidth = _slider.container.width();
    _slider.childWidth = _slider.container.children(":first").outerWidth(true);
    _slider.childrenCount = _slider.container.children().length;
    _slider.step = _slider.containerWidth / _slider.childWidth;
    _slider.current = _slider.container.attr("data-current-position") == undefined ? _slider.step : +_slider.container.attr("data-current-position");

    _slider.calculateNextItemsCount = function () {
        let nextPosition = _slider.current + _slider.step;
        if (nextPosition > _slider.childrenCount) {
            nextPosition = _slider.childrenCount;
        }

        return nextPosition - _slider.current;
    }

    _slider.calculatePrevItemsCount = function () {
        let prevPosition = _slider.current - _slider.step;
        if (prevPosition < _slider.step) {
            prevPosition = _slider.step;
        }

        return _slider.current - prevPosition;
    }

    _slider.Next = function () {
        let count = _slider.calculateNextItemsCount();
        let width = _slider.childWidth * count;

        _slider.current += count;
        _slider.container.attr("data-current-position", _slider.current);

        _slider.container.animate({ right: `-=${width}px` }, 700);
    }

    _slider.Prev = function () {
        let count = _slider.calculatePrevItemsCount();
        let width = _slider.childWidth * count;

        _slider.current -= count;
        _slider.container.attr("data-current-position", _slider.current);

        _slider.container.animate({ right: `+=${width}px` }, 700);
    }

    _slider.hasNext = () => _slider.current < _slider.childrenCount;
    _slider.hasPrev = () => _slider.current > _slider.step;

    return _slider;
}

$.fn.slider = function () {
    this.each(function () {
        let container = $(this);
        let next = container.parent().siblings(".btn.next");
        let prev = container.parent().siblings(".btn.prev");

        container.parent().siblings(".next, .prev").click(function () {
            let slide = slider(container);

            if ($(this).hasClass("next")) {
                slide.Next();
            }
            else {
                slide.Prev();
            }

            next.removeClass("d-none");
            if (!slide.hasNext()) {
                next.addClass("d-none");
            }

            prev.removeClass("d-none");
            if (!slide.hasPrev()) {
                prev.addClass("d-none");
            }
        });
    });
};

$(function () {
    $(".thumbnail-container .row").slider();

    adjustSliderNavigation();
    $(window).resize(function () {
        adjustSliderNavigation();
    });
});