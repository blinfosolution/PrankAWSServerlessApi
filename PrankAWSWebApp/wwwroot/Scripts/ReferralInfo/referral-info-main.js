$(document).ready(function (e) {
    GetReferralInfo();
});
function GetReferralInfo() {
    $.ajax({
        url: '/Admin/ReferralInfo/PartialReferralInfoLst',
        type: 'GET',
        async: false,
        beforeSend: function () {
            $(".loadingBox").show();
        },
        success: function (data) {
            if (data != null) {
                $('#dvReferralInfoLst').html(data.html);
            }
        },
        error: function () {
            alert("Error");
            $(".loadingBox").hide();
        },
        complete: function (jqXHR, status) {
            $(".loadingBox").hide();
        }
    });
}

function AddEditReferralInfo(Id,mode) {
    $.ajax({
        url: '/Admin/ReferralInfo/PartialAddEditReferralInfo',
        type: 'GET',
        data: { referralId: Id },
        async: false,
        beforeSend: function () {
        },
        success: function (data) {
            if (data != null) {
                $('#dvAddEditReferralInfo').html('');
                $('#dvAddEditReferralInfo').html(data.html);
                $("#ReferralInfoAddEditModal").modal("show");
                $('#referralHeading').text(mode);
                $.validator.unobtrusive.parse("#frmReferralInfo");
            }
        },
        error: function () {
            alert("Error");

        },
        complete: function (jqXHR, status) {

        }
    });
}

$("#btnSaveReferralInfo").click(function (e) {
    e.preventDefault();
    var form = "frmReferralInfo";
    var token = $('input[name="__RequestVerificationToken"]').val();
    var headers = {};
    headers['__RequestVerificationToken'] = token;
    if ($("#" + form).valid()) {
        $(".loadingBox").show();
        $.ajax({
            url: $("#" + form).attr("action"),
            type: $("#" + form).attr("method"),
            data: $("#" + form).serialize(),
            dataType: "json",
            headers: headers,
            success: function (data) {
                if (data.objResponse.statusCode === 200) {
                    if (data.objResponse.html !== "") {
                        $('#dvReferralInfoLst').html(data.objResponse.html);
                        $("#ReferralInfoAddEditModal").modal("hide");

                    }
                    toastr.success(data.objResponse.message);
                }
                else {
                    toastr.error(data.objResponse.message);
                }

            },
            error: function (xhr, status, error) {
                //  toastr.error("Oops something went wrong");
                alert('err' + xhr.responseText);
            },
            beforeSend: function () {
                $(".loadingBox").show();
            },
            complete: function () {
                $(".loadingBox").hide();
            }
        });
    }
    else {
        toastr.error("Oops something went wrong");
    }
})
function DeleteReferralInfo(id) {

    var r = confirm("Are you sure you want to delete this record !");
    if (r == true) {

        $.ajax({
            url: '/Admin/ReferralInfo/DeleteReferralInfo',
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
                        GetReferralInfo();
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
