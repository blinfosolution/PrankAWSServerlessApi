$(document).ready(function (e) {
    DataTableInit();
});
function DataTableInit() {
    table = $("#dataTableDeviceInfoLst").DataTable({
        iDisplayLength: 10,
        responsive: true,
        stateSave: true,
        columns: [
            {
                "bSearchable": false,
                "bSortable": true,
                'render': function (data, type, full, meta) {
                    return full.deviceKey;
                }
            },
            {
                "bSearchable": false,
                "bSortable": true,
                'render': function (data, type, full, meta) {
                    return full.deviceInfomation;
                }
            },
            {
               
                'render': function (data, type, full, meta) {
                    return full.creditBalance;
                }
            },
            {
                "bSearchable": false,
                "bSortable": true,
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
                    var edit = '<button class="btn btn-light border" onclick="javascript:ViewCallDetail('+ full.deviceId +');" title="View Call History"> Calls</button>';
                    var Delete = '&nbsp;<button class="btn btn-danger" onclick="javascript:ViewCallPackageOrderDetail('+ full.deviceId +');" title="View Package">Orders</button>';
                    return edit + Delete;
                }
            }
        ],
        destroy: true,
        oLanguage: {
            sProcessing: "<img src='/Images/Loader.gif'  style='width:32px;height:32px;'> &nbsp;&nbsp; Processing......"
        },
        processing: true,
        bServerSide: true,
        sAjaxSource: '/Admin/DeviceInfo/PartialGetDeviceinfoLst',
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

//-------------------------------
//=================================================
function ActiveDeviceInfo(deviceId, isActive) {
    $.ajax({
        url: '/Admin/DeviceInfo/ChangeStatusDeviceInfo',
        type: 'GET',
        data: { deviceId: deviceId, isActive: isActive },
        async: false,
        beforeSend: function () {
            $(".loadingBox").show();
        },
        success: function (data) {
            if (data != null) {
                if (data.objResponse.statusCode === 200) {
                    toastr.success(data.objResponse.message);
                    $('#dvDeviceInfoLst').html(data.objResponse.html);
                } else {
                    toastr.error(data.objResponse.message);
                }
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

function ViewCallDetail(deviceId) {
    $.ajax({
        url: '/Admin/DeviceInfo/GetCallHistoryByDeviceId',
        type: 'GET',
        data: { deviceId: deviceId },
        async: false,
        beforeSend: function () {
            $(".loadingBox").show();
        },
        success: function (data) {
            if (data != null) {
                $('#dvDeviceCallHistoryLst').html(data.html);
                $('#DeviceCallHistoryModal').modal('show');
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

function ViewCallPackageOrderDetail(deviceId) {
    $.ajax({
        url: '/Admin/DeviceInfo/GetPackageOrderByDeviceId',
        type: 'GET',
        data: { deviceId: deviceId },
        async: false,
        beforeSend: function () {
            $(".loadingBox").show();
        },
        success: function (data) {
            if (data != null) {
                $('#dvDevicePackageOrderLst').html(data.html);
                $('#DevicePackageOrderModal').modal('show');
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