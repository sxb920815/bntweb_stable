jQuery(function ($) {
    var productTable = $('#ProductTable').dataTable({
        "processing": true,
        "serverSide": true,
        "ajax": url_loadPage,
        "sorting": [[0, "desc"]],
        "aoColumns":
            [
                { "mData": "SingleGoodsName", 'sClass': 'left' },
                { "mData": "Specification", 'sClass': 'left' },
                {
                    "mData": "OriginalPrice",
                    "sWidth": "150px", 'sClass': 'left'
                },
                {
                    "mData": "LimitPrice",
                    "sWidth": "150px", 'sClass': 'left'
                },
                {
                    "mData": "Quantity",
                    "sWidth": "150px", 'sClass': 'left'
                },
                {
                    "mData": "Stock",
                    "sWidth": "150px", 'sClass': 'left'
                },
                {
                    "mData": "BeginTime", 'sClass': 'left',
                    "sWidth": "150px",
                    "mRender": function (data, type, full) {
                        if (data != null && data.length > 0) {
                            return eval('new ' + data.replace(/\//g, '')).Format("yyyy-MM-dd hh:mm:ss");
                        }
                        return "";
                    }
                },
                {
                    "mData": "EndTime", 'sClass': 'left',
                    "sWidth": "150px",
                    "mRender": function (data, type, full) {
                        if (data != null && data.length > 0) {
                            return eval('new ' + data.replace(/\//g, '')).Format("yyyy-MM-dd hh:mm:ss");
                        }
                        return "";
                    }
                },
                {
                    "mData": "Enabled", 'sClass': 'center', "orderable": false,
                    "sWidth": "150px",
                    "mRender": function (data, type, full) {
                        var render = '<div class="visible-md visible-lg hidden-sm hidden-xs action-buttons">';
                        render += '<a class="blue" href="' + url_editGoods + '?id=' + full.Id + '" title="编辑"><i class="icon-pencil bigger-130"></i></a>';
                        render += '<a class="red delete" data-id="' + full.Id + '" href="#" title="删除"><i class="icon-trash bigger-130"></i></a>';
                        render += '</div>';
                        return render;
                    }
                }
            ]
    });


    $('#ProductTable').on("click", ".delete", function (e) {
        var id = $(this).data("id");
        bntToolkit.confirm("确定删除该产品吗？", function () {
            bntToolkit.post(url_deleteGoods, { id: id }, function (result) {
                if (result.Success) {
                    $("#ProductTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });
});

//function formatDate(now) {
//    var year = now.getYear();
//    var month = now.getMonth() + 1;
//    var date = now.getDate();
//    var hour = now.getHours();
//    var minute = now.getMinutes();
//    var second = now.getSeconds();
//    return year + "-" + month + "-" + date + "   " + hour + ":" + minute + ":" + second;
//}
