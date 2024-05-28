"use strict";


Dropzone.options.uploadImages = {
        maxFiles: 8,
        maxFilesize: 250, // MB
    acceptedFiles: 'image/*,video/*,audio/*,.pdf',
    timeout: 1200000,
        dictDefaultMessage: "انتخاب تصاویر(حداکثر 7 عکس و  یک ویدیو 5 دقیقه ای 100 مگابایتی)",
    dictMaxFilesExceeded: "حداکثر 8 فایل مجاز است",
    
    dictFileTooBig: 'حداکثر سایز مجاز ویدیو 1 فایل 100 مگابایتی 5 دقیقه ای می باشد ',
    
    init: function () {
        this.on("addedfiles", function (files) {
            $(".sortmessage").remove();
            $("#submitVideoConfirm").prop("disabled", true);
            $("#submitVideoConfirm").html('منتظر اتمام آپلود بمانید');

            $(".UploadedPhotos").prepend('<p class="waitmessage" style="color:red">در حال ارسال فایل ها، لطفا منتظر بمانید</p>');
            //console.log(files.length + ' files added');
        });
        this.on("error", function (file, message) {
            
            $(".UploadedPhotos").prepend('<p class="errormessage" style="color:red">خطایی رخ داده: ' + message + '</p>');
        }); 
    },
    queuecomplete: function (file) {

        $("#submitVideoConfirm").prop("disabled", false);
        $("#submitVideoConfirm").html('ارسال');

        $(".waitmessage").remove();
        $(".sortmessage").remove();
        $(".UploadedPhotos").prepend('<p class="sortmessage" style="color:blue"> آخرین عکس کاور آموزش می باشد،با دکمه های جلوی تصاویر ترتیب آنها را مشخص کنید،</p>');
    },    
    success: function (file, response) {
        if (response.success) {
          $.each(response.guid, function (index, value) {
              if (value.indexOf(".jpg") > 0) {
                  $(".ul-uploadimages").prepend(
                      ' <li id="li_11" class="li_question"><input type="hidden"  name="imageGuids"  class="videoGuid" value="' + value + '" /><img class="uploaded-img  pl-1 pb-2" id="theImg' + index + '" src="/photos/tempphotos/' + value + '" /> <a onclick="up(this)" style="cursor:pointer;"><i class="fas fa-angle-double-up pr-3"  style="color:green;"></i></a><a onclick="down(this)" style="cursor:pointer;"><i class="pr-2 fas fa-angle-double-down " style="color:red;"></i></a></li>'
                  );
              }
              else if (value.indexOf(".pdf") > 0) {
                  $(".ul-uploadimages").prepend(
                      ' <li id="li_11" class="li_question"><input type="hidden"  name="imageGuids"  class="videoGuid" value="' + value + '" /><img class="uploaded-img  pl-1 pb-2" id="theImg' + index + '" src="/images/pdfthumbnail.jpg" /> <a onclick="up(this)" style="cursor:pointer;"><i class="fas fa-angle-double-up pr-3"  style="color:green;"></i></a><a onclick="down(this)" style="cursor:pointer;"><i class="pr-2 fas fa-angle-double-down " style="color:red;"></i></a></li>'
                  );
              }
              else {
                  $(".ul-uploadimages").prepend(
                      ' <li id="li_11" class="li_question"><input type="hidden"  name="imageGuids"  class="videoGuid" value="' + value + '" /><img class="uploaded-img  pl-1 pb-2" id="theImg' + index + '" src="/images/videothumbnail.jpg" /> <a onclick="up(this)" style="cursor:pointer;"><i class="fas fa-angle-double-up pr-3"  style="color:green;"></i></a><a onclick="down(this)" style="cursor:pointer;"><i class="pr-2 fas fa-angle-double-down " style="color:red;"></i></a></li>'
                  );
              }



            });
        }
        else {
            //alert(response.error);
        }
        return file.previewElement.classList.add("dz-success");
    },
    accept: function (file, done) {
    done();
    }
};

function up(obj) {

        $(obj).parent().insertBefore($(obj).parent().prev());

    }
    function down(obj) {
        $(obj).parent().insertAfter($(obj).parent().next());

}

$(document).ready(function () {
    //Dropdownlist Selectedchange event
    $(function () {
        $("#MainCategory").change(function () {
            var selectedItem = $(this).val();
            var ddCategory = $("#Category");
            var statesProgress = $("#states-loading-progress");
            statesProgress.show();
            $.ajax({
                cache: false,
                type: "GET",
                url: "/membership/panel/RefreshCategory",
                data: { "MainCategoryId": selectedItem },
                success: function (data) {
                    ddCategory.prop("disabled", false);
                    
                    ddCategory.html('');
                    ddCategory.append($('<option></option>').val("").html("یک گروه اصلی را انتخاب کنید"));
                    $.each(data, function (id, option) {
                        ddCategory.append($('<option></option>').val(option.Id).html(option.Name));
                    });
                    statesProgress.hide();
                    ddCategory.val('')
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('مشکل در برقر اری ارتباط با سرور');
                    statesProgress.hide();
                }
            });
        });
    });
})