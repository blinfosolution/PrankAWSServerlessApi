$(document).ready(function (e) {
    DataTableInit();
});

function DataTableInit() {
    var tablecontack = $("#dataTableContact").DataTable({
        iDisplayLength: 10,
        responsive: true,
        columns: [
            {
                "bSearchable": false,
                "bSortable": true,
                'render': function (data, type, full, meta) {
                    return full.emailTo;
                }
            },
            {
                'render': function (data, type, full, meta) {
                    return full.messages;
                }
            },
            {
                "bSearchable": false,
                "bSortable": true,
                'data': 'sendDate',
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
        sAjaxSource: '/Admin/ContactUs/PartialContactUsLst',
        sServerMethod: 'post',
        searching: false,
        "fnDrawCallback": function (oSettings) {
        },
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "sSearch", "value": $("#txtSearch").val() });
        }

    });
}

