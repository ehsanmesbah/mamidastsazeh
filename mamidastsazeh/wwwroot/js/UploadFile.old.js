'use strict';

$(function () {
    var $form = $('.box'),
        $input = $('.box__file'),
        $label = $input.next('label'),
        $bar = $('.box__bar'),
        $status = $('.box__status').find("span"),
        $errorMsg = $('.box__error').find("span"),
        $boxRestart = $('.box__restart'),
        $videoGuid = $("#videoGuid"),
        labelVal = $label.html(),
        multiple = $input.attr("multiple") != undefined,
        humanReadabeSize = function (bytes, si) {
            var thresh = si ? 1000 : 1024;
            if (Math.abs(bytes) < thresh) {
                return bytes + ' B';
            }
            var units = si
                ? ['kB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB']
                : ['KiB', 'MiB', 'GiB', 'TiB', 'PiB', 'EiB', 'ZiB', 'YiB'];
            var u = -1;
            do {
                bytes /= thresh;
                ++u;
            } while (Math.abs(bytes) >= thresh && u < units.length - 1);
            return bytes.toFixed(1) + ' ' + units[u];
        },
        boxRestart = function () {
            $form.removeClass("is-error")
                .removeClass("is-uploading")
                .removeClass("is-success");

            $input.click();
        };

    $input.each(function () {
        $input.on('change', function (e) {
            var fileName = '';

            if (this.files && this.files.length > 1 && multiple) {
                fileName = (this.getAttribute('data-multiple-caption') || '')
                    .replace('{count}', this.files.length);
            }
            else if (e.target.value) {
                fileName = e.target.value.split('\\').pop();
            }

            if (fileName) {
                $label.find('span').html(fileName);
            }
            else {
                $label.html(labelVal);
            }

            if ($input.prop("files").length > 0) {
                $form.submit();
            }
        });

        // Firefox bug fix
        $input
            .on('focus', function () { $input.addClass('has-focus'); })
            .on('blur', function () { $input.removeClass('has-focus'); });
    });

    var isAdvancedUpload = function () {
        var div = document.createElement('div');
        return (('draggable' in div) || ('ondragstart' in div && 'ondrop' in div)) &&
            'FormData' in window && 'FileReader' in window;
    }();

    if (isAdvancedUpload) {
        $form.addClass('has-advanced-upload');

        $form
            .on('drag dragstart dragend dragover dragenter dragleave drop', function (e) {
                e.preventDefault();
                e.stopPropagation();
            })
            .on('dragover dragenter', function () {
                $form.addClass('is-dragover');
            })
            .on('dragleave dragend drop', function () {
                $form.removeClass('is-dragover');
            })
            .on('drop', function (e) {
                $input
                    .prop("files", e.originalEvent.dataTransfer.files)
                    .trigger("change");
            });
    }

    $boxRestart.on("click", function (e) {
        e.preventDefault();
        boxRestart();
    });

    $form.ajaxForm({
        dataType: "json",
        beforeSend: function () {
            $status.empty();
            $bar.attr("aria-valuenow", "0")
                .width("0%")
                .html("0%");
            $form.addClass("is-uploading");
        },
        uploadProgress: function (event, position, total, percentComplete) {
            let totalMb = humanReadabeSize(total, false);
            let positionMb = humanReadabeSize(position, false);

            $status.html(`${positionMb} of ${totalMb}`);
            $bar.attr("aria-valuenow", percentComplete)
                .width(`${percentComplete}%`)
                .html(`${percentComplete}%`);
        },
        complete: function (data) {
            $form.removeClass("is-uploading");
            var response = data.responseJSON;
            //console.log(response.Result);
            if (response.success) {
                $form.addClass("is-success");
                if (response.FileName != null && response.FileName != '') {
                    $videoGuid.val(response.FileName);
                }
                if (response.ThumbnailName != null && response.ThumbnailName != '') {
                    $("#thumbnailGuid").val(response.ThumbnailName);
                }
            }
            else {
                $form.addClass("is-error");
                $errorMsg.text(response.error);
            }
        },
        error: function () {
            $form.removeClass("is-uploading");
        }
    });
});