$(document).on("change", ".chkBoxEmployee", function (e) {
    var flag = false;
    $('#dataTablesEmp tr').each(function () {
        var chkBoxChecked = $(this).find(".chkBoxEmployee").prop("checked");
        if (chkBoxChecked === false) {
            $(".allChkBoxEmp").prop("checked", chkBoxChecked);
        }
        else {
            flag = true;
        }
    });
    if (flag === true) {
        $("#Deleteemployee").show();
    }
    else {
        $("#Deleteemployee").hide();
    }
    //if ($('[name="chkBoxEmployee[]"]:checked').length > 0)
    //    $("#Deleteemployee").show();
    //else
    //    $("#Deleteemployee").hide();

});

$(".allChkBoxEmp").change(function (e) {
    var flag = $(".allChkBoxEmp").prop("checked");
    $('#tbodyEmployee tr').each(function () {
        $(this).find(".chkBoxEmployee").prop("checked", flag);
    });
    //if (flag === true) {
    //    $("#Deleteemployee").show();
    //}
    //else {
    //    $("#Deleteemployee").hide();
    //}
});

$("#btnAddEmployee").click(function (e) {
    $.ajax({
        url: '/Admin/Employee/PartialCreate',
        type: 'GET',
        async: false,
        beforeSend: function () {
            $(".loadingBox").show();
        },
        success: function (data) {
            if (data != null) {
                $('#dvaddEmployee').html('');
                $('#dvaddEmployee').html(data.html);
                $("#myEmployeeModal").modal("show");
                $('#EmployeeHeading').text('Add Employee');
            }
        },
        error: function () {
            toastr.error("Oops something went wrong");
            //alert("Error");
            //$(".ajax-loading-block-window").hide();
            //$(".showalertMessage").html("Technical error occured, please contact to administrator.");
            //$('.divWarningMessageBox').show();
            setTimeout(function () {
                $(".loadingBox").hide();
            }, 3000);
        },
        complete: function (jqXHR, status) {
            setTimeout(function () {
                $(".loadingBox").hide();
            }, 500);
            //console.log("CreateQuotationDetail complete:-");
        }
    });
})

$("#btnSave").click(function (e) {
    e.preventDefault();
    $('#lblusererror').hide();
    var form = "frmemployee";
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
                        $('#tbodyEmployee').empty();
                        $('#tbodyEmployee').append(data.objResponse.html);
                        $("#myEmployeeModal").modal("hide");
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

$(document).on("click", ".btnEdit", function (e) {
    var tr = $(this).closest("tr");
    var id = tr.attr("data-id");
    $.ajax({
        url: '/Admin/Employee/PartialEdit',
        type: 'GET',
        async: false,
        data: { id: id },
        beforeSend: function () {

        },
        success: function (data) {
            if (data != null) {
                if (data.html == "") {
                    toastr.info("No employee found");
                }
                else {
                    $('#dvaddEmployee').html('');
                    $('#dvaddEmployee').html(data.html);
                    $("#myEmployeeModal").modal("show");
                    $('#EmployeeHeading').text('Edit Employee');
                }
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

$(document).on("click", ".btnDelete", function (e) {
    var tr = $(this).closest("tr");
    var id = tr.attr("data-id");
 
    var r = confirm("Do you want to delete !");
    if (r == true) {

        $.ajax({
            url: '/Admin/Employee/Delete',
            type: 'GET',
            async: false,
            data: { id: id },
            //headers: headers,
            beforeSend: function () {

            },
            success: function (data) {


                if (data != null) {

                    if (data.objResponse.statusCode === 200) {
                        if (data.objResponse.Html !== "") {
                            $('#tbodyEmployee').empty();
                            $('#tbodyEmployee').append(data.objResponse.html);
                        }
                        toastr.success(data.objResponse.message);
                    }
                    else {
                        toastr.error(data.objResponse.message);
                    }
                }
            },
            error: function (jqXHR, status) {
                toastr.error("Oops something went wrong");
                //alert("Error" + jqXHR.responseText);
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
    } else {

    }
});

$(document).on("click", ".btnResetPass", function (e) {
    var tr = $(this).closest("tr");
    var id = tr.attr("data-id");
 
        $.ajax({
            url: '/Admin/Employee/ResetPassword',
            type: 'GET',
            async: false,
            data: { id: id },
            beforeSend: function () {
                $(".loadingBox").show();
            },
            success: function (data) {
                if (data != null) {
                    if (data.objResponse.statusCode === 200) {
                        toastr.success(data.objResponse.message);
                    }
                    else {
                        toastr.error(data.objResponse.message);
                    }
                }
            },
            error: function (jqXHR, status) {
                toastr.error("Oops something went wrong" + jqXHR.responseText);
                //alert("Error" + jqXHR.responseText);
                //$(".ajax-loading-block-window").hide();
                //$(".showalertMessage").html("Technical error occured, please contact to administrator.");
                //$('.divWarningMessageBox').show();
                setTimeout(function () {
                    $(".loadingBox").hide();
                }, 3000);
            },
            complete: function (jqXHR, status) {
                setTimeout(function () {
                    $(".loadingBox").hide();
                }, 500);
                //console.log("CreateQuotationDetail complete:-");
            }
        });
  
});