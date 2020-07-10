$(document).ready(function (e) {
    GetCallTrackingLst();
});
function GetCallTrackingLst() {

    $.ajax({
        url: '/Admin/CallTracking/PartialCallTrackingLst',
        type: 'GET',
        async: false,
        beforeSend: function () {
            $(".loadingBox").show();
        },
        success: function (data) {
            if (data != null) {
                $('#dvCallTravkingLst').html(data.html);
            }
        },
        error: function () {
            alert("Error");
            $(".loadingBox").hide();;

        },
        complete: function (jqXHR, status) {
            $(".loadingBox").hide();
        }
    });
}

function DeleteCallHistory(id) {

    var r = confirm("Are you sure you want to delete this record !");
    if (r == true) {

        $.ajax({
            url: '/Admin/CallTracking/DeleteCallHistory',
            type: 'GET',
            async: false,
            data: { id: id },
            //headers: headers,
            beforeSend: function () {
                $(".loadingBox").show();
            },
            success: function (data) {
                if (data != null) {
                    if (data.objResponse.statusCode === 200) {
                        toastr.success(data.objResponse.message);
                        GetCallTrackingLst();
                    }
                    else {
                        toastr.error(data.objResponse.message);
                    }

                }
            },
            error: function (jqXHR, status) {
                alert("Error" + jqXHR.responseText);
                $(".loadingBox").hide();

            },
            complete: function (jqXHR, status) {
                $(".loadingBox").hide();
            }
        });
    } else {

    }
}
