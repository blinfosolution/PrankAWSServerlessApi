$(document).ready(function (e) {
    DataTableInit();
});

function AddEditPrankinfo(prankId, mode) {
    $.ajax({
        url: '/Admin/Prank/PartialAddEditPrankInfo',
        type: 'GET',
        data: { PrankId: prankId },
        async: false,
        beforeSend: function () {

        },
        success: function (data) {
            if (data != null) {
                $('#dvAddEditPrankInfo').html('');
                $('#dvAddEditPrankInfo').html(data.html);
                $("#PrankInfoAddEditModal").modal("show");
                $('#prankHeading').text(mode);
                $.validator.unobtrusive.parse("#frmPrankInfo");
            }
        },
        error: function () {
            alert("Error");

        },
        complete: function (jqXHR, status) {

        }
    });
}

$("#btnSavePrankInfo").click(function (e) {
    e.preventDefault();
    $('#lblusererror').hide();
    var form = "frmPrankInfo";
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
                    $("#PrankInfoAddEditModal").modal("hide");
                    DataTableInit();
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
function DeletePrankInfo(id) {

    var r = confirm("Are you sure you want to delete this record !");
    if (r == true) {

        $.ajax({
            url: '/Admin/Prank/DeletePrankInfo',
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

    var tablePrank = $("#dataTablePrankInfo").DataTable({
        iDisplayLength: 10,
        responsive: true,
        //stateSave: true,
        columns: [          
            {
                "bSearchable": false,
                "bSortable": true,
                'data': 'prankImage',
                'render': function (data, type, full, meta) {
                    var PrankName = "<strong>" + full.prankName + "</strong>"
                    var image = "<img src=" + full.prankImage + " alt='prank image' class='img-fluid'/>";
                    var startDiv = '<div class="buttonMeida">'
                    
                    var Play = "<button title='Play' class='btn btn-play' id='playaudio_" + full.prankId + "' onclick='javascript:playAudio(" + full.prankId +",\"" + full.previewAudioFile+ "\");'> <i class= 'fa fa-play' ></i></button> ";
                    var Pause = '<button title="Pause" class="btn btn-Pause" id="pauseaudio_' + full.prankId + '" onclick="javascript: pauseAudio(' + full.prankId + ');" style="display:none;"> <i class="fa fa-pause"></i></button> &nbsp;';

                    var MainPlay = '<button title="Play" class="btn btn-main" id="playaudio_' + full.prankId + full.prankId + '" onclick="javascript: playAudio(' + full.prankId + full.prankId + ',\'' + full.mainAudioFile + '\'); " ><i class="fa fa-play"></i></button>';
                    var MAinPause = '<button title="Pause" class="btn btn-Pause" id="pauseaudio_' + full.prankId + full.prankId + '" onclick="javascript: pauseAudio(' + full.prankId + full.prankId + ');" style="display:none;"> <i class="fa fa-pause"></i></button>';
                    var endDiv = '</div>'
                    return PrankName + image + startDiv + Play + Pause + MainPlay + MAinPause + endDiv;
                }
            },
            {
                'render': function (data, type, full, meta) {
                                   return full.prankDesc;
                }
            },
            {
                'render': function (data, type, full, meta) {
                    return full.isActive;
                }
            },
            {
                'data': 'addedDate',
                'render': function (data, type, full, meta) {
                    return moment(data).format('DD-MMM-yyyy');
                }
            },
            {
                'data': 'modifiedDate',
                'render': function (data, type, full, meta) {
                    if (data != null) {
                        return moment(data).format('DD-MMM-yyyy');
                    } else
                        return '-';
                }
            },
            {
                "bSearchable": false,
                "bSortable": false,
                'render': function (data, type, full, meta) {
                    var edit = '<button class="btn btn-secondary" onclick="javascript: AddEditPrankinfo(' + full.prankId + ');">Edit</button>&nbsp;';
                    var PrankDelete = '<button class="btn btn-danger" onclick="javascript: DeletePrankInfo(' + full.prankId + ')">Delete</button>';
                    return edit + PrankDelete;
                }
            }

        ],
        destroy: true,
        oLanguage: {
            sProcessing: "<img src='/Images/Loader.gif'  style='width:32px;height:32px;'> &nbsp;&nbsp; Processing......"
        },
        processing: true,
        bServerSide: true,
        sAjaxSource: '/Admin/Prank/PartialPrankInfoLst',
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