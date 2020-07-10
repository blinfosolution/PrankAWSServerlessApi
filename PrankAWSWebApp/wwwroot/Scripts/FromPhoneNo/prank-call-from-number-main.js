$(document).ready(function (e) {
    DataTableInit();
});

function AddEditPrankCallFromPhoneNumber(Id, mode) {
    $.ajax({
        url: '/Admin/PrankCallFromPhoneNumber/PartialAddEditCallFromNumber',
        type: 'GET',
        data: { fromId: Id },
        async: false,
        beforeSend: function () {
        },
        success: function (data) {
            if (data != null) {
                $('#dvAddEditPrankCallFromPhoneNumeber').html('');
                $('#dvAddEditPrankCallFromPhoneNumeber').html(data.html);
                $("#PrankCallFromPhoneNumeberAddEditModal").modal("show");
                $('#CallFromNumberHeading').text(mode);
                if (Id > 0) {
                    $('#Country').val($('#hideCountry').val());
                } else {
                    $('#CountryCode').val($('option:selected', $('#Country')).attr('data-country'));
                }
            }
        },
        error: function () {
            alert("Error");

        },
        complete: function (jqXHR, status) {

        }
    });
}

$("#btnSavePrankCallFromPhoneNumeber").click(function (e) {
    e.preventDefault();
    var form = "frmPrankCallFromPhoneNumeber";
    var token = $('input[name="__RequestVerificationToken"]').val();
    var headers = {};
    headers['__RequestVerificationToken'] = token;
    if ($("#" + form).valid()) {
        $(".loadingBox").show();
        $.ajax({
            url: $("#" + form).attr("action"),
            type: $("#" + form).attr("method"),
            data: $("#" + form).serialize() + "&Country=" + $('#Country').val(),
            dataType: "json",
            headers: headers,
            success: function (data) {
                if (data.objResponse.statusCode === 200) {
                    DataTableInit();
                    toastr.success(data.objResponse.message);
                    $("#PrankCallFromPhoneNumeberAddEditModal").modal("hide");
                }
                else {
                    toastr.error(data.objResponse.message);
                }
            },
            error: function (xhr, status, error) {
               
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
function DeleteFromPhoneNumber(id) {

    var r = confirm("Are you sure you want to delete this record !");
    if (r == true) {

        $.ajax({
            url: '/Admin/PrankCallFromPhoneNumber/DeletePrankCallFromPhoneNumber',
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
                        DataTableInit();
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

function DataTableInit() {

    var tablePrank = $("#dataTable").DataTable({
        iDisplayLength: 10,
        responsive: true,
        "order": [[4, "desc"]],
        //stateSave: true,
        columns: [
            {
                "bSearchable": false,
                "bSortable": true,
                'render': function (data, type, full, meta) {
                    return full.country;
                }
            },
            {
                "bSearchable": false,
                "bSortable": true,
                'render': function (data, type, full, meta) {
                    return full.countryCode;
                }
            },
            {
                "bSearchable": false,
                "bSortable": true,
                'render': function (data, type, full, meta) {
                    return full.phoneNumber;
                }
            },

            {
                'render': function (data, type, full, meta) {
                    return full.isActive;
                }
            },
            {
                "bSearchable": false,
                "bSortable": true,
                'data': 'addedDate',
                'render': function (data, type, full, meta) {
                    return moment(data).format('DD-MMM-yyyy');
                }
            },          
            {
                "bSearchable": false,
                "bSortable": false,
                'render': function (data, type, full, meta) {

                    var btn = full.isDefault ? " btn-success" : " btn-warning";
                    var txt = full.isDefault ? "Default" : "Not Default";
                    var edit = '<button class="btn btn-light border " onclick="javascript: AddEditPrankCallFromPhoneNumber(' + full.prankCallFromId + ',' + "'Edit Prank Call From Phone Number'" + ');">Edit</button>';
                    var PrankDelete = '&nbsp;<button class="btn btn-danger" onclick="javascript: DeleteFromPhoneNumber(' + full.prankCallFromId + ');">Delete</button>';
                    var isDefault = '&nbsp; <button class="btn' + btn +'"  onclick="javascript: IsDefaultPrankCallFromPhoneNumber(' + full.prankCallFromId + ',' + full.isDefault + ');">'+txt+'</button>';
                    return edit + PrankDelete + isDefault;
                }
            }

        ],
        destroy: true,
        oLanguage: {
            sProcessing: "<img src='/Images/Loader.gif'  style='width:32px;height:32px;'> &nbsp;&nbsp; Processing......"
        },
        processing: true,
        bServerSide: true,
        sAjaxSource: '/Admin/PrankCallFromPhoneNumber/PartialCallFromPhoneNoLst',
        sServerMethod: 'post',
        searching: false,
        "fnDrawCallback": function (oSettings) {

            //alert(oSettings.json + "    " + JSON.stringify(oSettings));
        },
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "sSearch", "value": $("#txtSearch").val() });
        }
        //"scrollY": "200px",
    });
}

function IsDefaultPrankCallFromPhoneNumber(id, isDefault) {

    $.ajax({
        url: '/Admin/PrankCallFromPhoneNumber/ChangeStatusFromPhoneNumber',
        type: 'GET',
        async: false,
        data: { PrankCallFromId: id, isDefault: isDefault },
        //headers: headers,
        beforeSend: function () {
            $(".loadingBox").show();
        },
        success: function (data) {
            if (data != null) {
                if (data.objResponse.statusCode === 200) {
                    toastr.success(data.objResponse.message);
                    DataTableInit();
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

}