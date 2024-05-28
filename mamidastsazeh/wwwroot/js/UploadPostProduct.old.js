"use strict";

$(function () {
    var $form = $('.box'),
        $input = $('.box__file'),
        $label = $input.next('label'),
        $bar = $('.box__bar'),
        $status = $('.box__status').find("span"),
        $errorMsg = $('.box__error').find("span"),
        $boxRestart = $('.box__restart'),
        $videoGuid = $("#videoGuid"),
        $image1 = $("#image1"),
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
            alert("test");
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
    
    $("#removephotos").on("click", function (e) {

        var videoGuids = $('input[name="videoGuid"]').map(function () {
            return this.value;
        }).get();
    
       
        $.ajax({

            type: "POST",
            url: "/Membership/Panel/RemovePhotosAjax",
            data: { videoGuids: videoGuids },
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            success: function (response) {
               
            },
            failure: function (response) {
               
            },
            error: function (response) {
                
            }
        });
        $(".ul-uploadimages").empty();
        
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
            if (response.success) {
                //$form.addClass("is-success");
                
                //$image1.attr("src", "/photos/tempphotos/" + response.guid[0]);
                $(".sortmessage").remove();
                $(".UploadedPhotos").prepend('<p class="sortmessage"> آخرین تصویر کاور آموزش می باشد،با دکمه های جلوی تصاویر ترتیب آنها را مشخص کنید،</p>');

                $.each(response.guid, function (index, value) {
                //    $(".upload-images").prepend('<div id="imgdiv' + index + '" name="imgdiv' + index + '"></div>');
                //$(("#imgdiv").concat(index)).prepend('<img class="uploaded-img  pl-1 pb-2" id="theImg' + index + '" src="/photos/tempphotos/' + value + '" />');
                    //  $(("#imgdiv").concat(index)).prepend('<input type="hidden"  name="videoGuid[]"  class="videoGuid" id="img@value" value="' + value + '" />');
                   
                   $(".ul-uploadimages").prepend(
                       ' <li id="li_11" class="li_question"><input type="hidden"  name="imageGuids"  class="videoGuid" value="' + value + '" /><img class="uploaded-img  pl-1 pb-2" id="theImg' + index + '" src="/photos/tempphotos/' + value + '" /> <a onclick="up(this)" style="cursor:pointer;"><i class="fas fa-angle-double-up pr-3"  style="color:green;"></i></a><a onclick="down(this)" style="cursor:pointer;"><i class="pr-2 fas fa-angle-double-down " style="color:red;"></i></a></li>'
                    );


                   
                });
                
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