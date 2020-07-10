$(document).ready(function (e) {
    $("#btnlogin").click(function (e) {       
        e.preventDefault();
        $('#lblusererror').hide();
        var form = "frmlogin";
        var token = $('input[name="__RequestVerificationToken"]').val();
        var headers = {};
        headers['__RequestVerificationToken'] = token;
        if ($("#" + form).valid()) {
            $(".loadingBox").show();
            $.ajax({
                url: $("#" + form).attr("action"),
                type: $("#" + form).attr("method"),
                data: $("#" + form).serialize() + "&returnUrl=" + $("#" + form).attr("ReturnUrl"),
                dataType: "json",
                headers: headers,
                success: function (data) {
                    if (data.length > 0) {
                        window.location.href = data[0];
                    }
                    else {
                        $('#lblusererror').show();
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
    });
});