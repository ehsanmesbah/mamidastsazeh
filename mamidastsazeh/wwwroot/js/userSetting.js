"use strict";
$(document).ready(function () {
    if ($("#IsBusiness").is(":checked") ) {

        $("#isBusinessDiv").show();
    }
    else {

        $("#isBusinessDiv").hide();
    }
});
function togglebunisess() {

    $("#isBusinessDiv").toggle();

};
