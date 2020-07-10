$(document).ready(function (e) {
    DataTableInit();
});
function DataTableInit() {
    table = $("#dataTable").DataTable({
        iDisplayLength: 10,
        responsive: true,
        stateSave: true,
        columns: [
            {
                "bSearchable": false,
                "bSortable": true,
                'render': function (data, type, full, meta) {
                    return full.firstName + " " + full.lastName;
                }
            },
            {
                "bSearchable": false,
                "bSortable": true,
                'render': function (data, type, full, meta) {
                    return full.userName;
                }
            },
            {

                'render': function (data, type, full, meta) {
                    return full.moduleName;
                }
            },
            {
                "bSearchable": false,
                "bSortable": true,
                'render': function (data, type, full, meta) {
                    return full.workDescription;
                }
            },
            {
                "bSearchable": false,
                "bSortable": true,
                'data': 'trackDate',
                'render': function (data, type, full, meta) {
                    return moment(data).format('DD-MMM-yyyy');
                }
            }
           
        ],
        destroy: true,
        oLanguage: {
            sProcessing: "<img src='/Images/Loader.gif'  style='width:32px;height:32px;'> &nbsp;&nbsp; Processing......"
        },
        processing: true,
        bServerSide: true,
        sAjaxSource: '/Admin/TrackingInfo/PartialTrackingInfoLst',
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
