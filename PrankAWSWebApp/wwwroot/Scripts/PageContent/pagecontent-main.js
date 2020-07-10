
$(document).ready(function () {
 
});

/*

 */
$(document).on("click", ".btnEdit", function (e) {
    var tr = $(this).closest("tr");
    var id = tr.attr("data-id");
    $.ajax({
        url: '/Admin/PAgeContent/PartialEdit',
        type: 'GET',
        async: false,
        data: { id: id },
        beforeSend: function () {

        },
        success: function (data) {
            if (data != null) {
                $('#dvaddPageContent').empty();
                $('#dvaddPageContent').append(data.html);
                $("#myPageContentModal").modal("show");

                if (CKEDITOR.instances["PageContent"])
                    delete CKEDITOR.instances["PageContent"];
                CKEDITOR.replace("PageContent");
            }
        },
        error: function () {
            alert("Error");
            //$(".ajax-loading-block-window").hide();
            //$(".showalertMessage").html("Technical error occured, please contact to administrator.");
            //$('.divWarningMessageBox').show();
            //setTimeout(function () {
            //    $('.divWarningMessageBox').fadeOut('slow');
            //}, 3000);
        },
        complete: function (jqXHR, status) {
            //setTimeout(function () {
            //    $(".ajax-loading-block-window").hide();
            //}, 500);
            //console.log("CreateQuotationDetail complete:-");
        }
    });
});


$("#btnSave").click(function (e) {
    e.preventDefault();
    $('#lblusererror').hide();
    $("#PageContent").val(CKEDITOR.instances['PageContent'].getData());
    var form = "frmpagecontent";
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
                //alert(data.statusCode+"   length-:" + data.objResponse.length + "      " + data.objResponse.StatusCode + "      " + data.objResponse.html);
                if (data.objResponse.statusCode === 200) {
                    if (data.objResponse.html !== "") {
                        $('#tbodyPageContent').empty();
                        $('#tbodyPageContent').append(data.objResponse.html);
                        $("#myPageContentModal").modal("hide");
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
