$(document).ready(function (e) {
    DataTableInit();
});
function GetReferralInviteInfo() {
    var deviceId = $('#DeviceId').val() == '' ? 0 : $('#DeviceId').val();

    $.ajax({
        url: '/Admin/ReferralInviteInfo/PartialGetReferralInviteInfoLst',
        type: 'GET',
        async: false, data: { search: $('#txtSearch').val(), searchByDeviceId: deviceId },
        beforeSend: function () {
            $(".loadingBox").show();
        },
        success: function (data) {
            if (data != null) {
                $('#dvReferralInviteInfoLst').html(data.html);
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

function DataTableInit() {
    table = $("#dataTable").DataTable({
        iDisplayLength: 10,
        responsive: true,
        stateSave: false,
        columns: [
            {
                "bSearchable": false,
                "bSortable": true,
                'render': function (data, type, full, meta) {
                    //alert(full.fromDevice + "  " + full.fromDeviceId)
                    return full.fromDevice;
                }
            },
            {
                "bSearchable": false,
                "bSortable": true,
                'render': function (data, type, full, meta) {
                    console.log("ToDevice " + full.toDevice);
                    return (full.toDevice == "" || full.toDevice == null) ? "" : full.toDevice;
                }
            },
            {

                'render': function (data, type, full, meta) {
                    return full.freeToken;
                }
            },
            {
                "bSearchable": false,
                "bSortable": true,
                'render': function (data, type, full, meta) {
                    return full.status;
                }
            },
            {
                "bSearchable": false,
                "bSortable": true,
                'data': 'referDate',
                'render': function (data, type, full, meta) {
                   
                    return (data==null || data=="")?"":moment(data).format('DD-MMM-yyyy');
                }
            },
            {
                "bSearchable": false,
                "bSortable": true,
                'data': 'referAcceptedDate',
                'render': function (data, type, full, meta) {

                    console.log("referAcceptedDate " + data);
                    return (data == null || data == "") ? "" : moment(data).format('DD-MMM-yyyy');
                    //return moment(data).format('DD-MMM-yyyy');
                }
            },
           
        ],
        destroy: true,
        oLanguage: {
            sProcessing: "<img src='/Images/Loader.gif'  style='width:32px;height:32px;'> &nbsp;&nbsp; Processing......"
        },
        processing: true,
        bServerSide: true,
        sAjaxSource: '/Admin/ReferralInviteInfo/PartialGetReferralInviteInfoLst',
        sServerMethod: 'POST',
        searching: false,
        "fnDrawCallback": function (oSettings) {

        },
        "fnServerParams": function (aoData) {
           
           aoData.push({ "name": "sSearch", "value": $("#txtSearch").val() });

        }
        //"scrollY": "200px",
    });
}
