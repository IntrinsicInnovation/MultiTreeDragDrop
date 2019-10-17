$(document).ready(function () {

    initTHModal();
    $('#divHistory').corners();
});


function initTHModal() {
    $("#divHistory").dialog({
        autoOpen: false,
        modal: true,
        width: 950,
        position: "center",
        title: "History",
        zIndex: 20000    //Put dialog box on top of menu (menu zindex was 10000)
    });
}


function displayHistory(type, id, filterblank) {
    try {
        if (filterblank == undefined || filterblank == null)
            filterblank = true;
            showHistoryDialog();
            var oTable = $('#tblHistory').dataTable(
                {
                    "bDestroy": true,
                    "bJQueryUI": true,
                    "bSort": true,
                    "bAutoWidth": true,
                    "bProcessing": true,
                    "bServerSide": true,
                    "sScrollX": "1500px",
                    "sScrollY": "385px",
                    "sPaginationType": "full_numbers",
                    "iDisplayLength": 15,
                    "aLengthMenu": [[10, 15, 25, 50, 100], [10, 15, 25, 50, 100]],
                    "sAjaxSource": "/Services/HistoryManagement.asmx/ProcessHistory",
                    //Extra parameters
                    "fnServerParams": function (aoData) {
                        aoData.push({ "name": "type", "value": type },
                                    { "name": "rowId", "value": id },
                                    { "name": "filterBlank", "value": filterblank });
                    },

                    "aoColumnDefs": [
                        { "sTitle": "Table", "sWidth": "160px", "type": "text", "aTargets": [0] },
                        { "sTitle": "Column", "sWidth": "110px", "type": "text", "aTargets": [1],
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                //$(nTd).css('color', 'purple');
                                $(nTd).attr('title', oData[6]);
                            }
                        },
                        { "sTitle": "Old Value", "sWidth": "110px", "type": "text", "aTargets": [2],
                             "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).attr('title', oData[7]);
                            }
                        },
                        { "sTitle": "New Value", "sWidth": "110px", "type": "text", "aTargets": [3],
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).attr('title', oData[8]);
                            } 
                        },
                        { "sTitle": "Changed", "sWidth": "195px", "sType": "date", "aTargets": [4] },
                        { "sTitle": "Changed By", "sWidth": "140px", "type": "text", "aTargets": [5] }
                        ] 
                });
            }
            catch (exception) {
            }
        } //end displayHistory

function showHistoryDialog() {
    $("#tblHistory").show();
    $("#divHistory").dialog("open");
}

function closeHistoryDialog() {
    $("#divHistory").dialog("close");
}
