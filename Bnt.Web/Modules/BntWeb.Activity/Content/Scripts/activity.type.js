jQuery(function ($) {
    $('#ActivityTypeTable').dataTable({
        "processing": true,
        "serverSide": true,
        "sorting": false,
        "ajax": url_loadPage,
        "aoColumns":
        [
            { "mData": "Id", 'sClass': 'left', "orderable": false },
            { "mData": "TypeName", 'sClass': 'left', "orderable": false },
            { "mData": "Description", 'sClass': 'left', "orderable": false },
            {
                "mData": "",
                'sClass': 'center',
                "sWidth": "150px",
                "orderable": false,
                "mRender": function (data, type, full) {
                    var render = '<div class="visible-md visible-lg hidden-sm hidden-xs action-buttons">';
                    render += '<a class="blue view" data-id="' + full.Id + '" href="' + url_editType + '?id=' + full.Id + '" title="编辑"><i class="icon-pencil bigger-130"></i></a>';
                    render += '<a class="red delete" data-id="' + full.Id + '" href="#" title="删除"><i class="icon-trash bigger-130"></i></a>';
                    render += '</div>';
                    return render;
                }
            }
        ]
    });

    $('#ActivityTypeTable').on("click", ".delete", function (e) {
        var id = $(this).data("id");

        bntToolkit.confirm("删除后不可恢复，确定还要删除吗？", function () {
            bntToolkit.post(url_deleteType, { typeId: id }, function (result) {
                if (result.Success) {
                    $("#ActivityTypeTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });
});