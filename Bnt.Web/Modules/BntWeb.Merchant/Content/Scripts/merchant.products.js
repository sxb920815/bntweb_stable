jQuery(function ($) {
    var productName = $("#ProductName").val();
    var merchantId = $("#MerchantId").val();


    var loadTable = $('#MerchantProductInfoTable').dataTable({
        "processing": true,
        "serverSide": true,
        "sorting": [[5, "desc"]],
        "ajax": {
            "url": url_loadPage,
            "data": function (d) {
                //添加额外的参数传给服务器
                d.extra_search = { "ProductName": productName, "MerchantId": merchantId };
            }
        },
        "aoColumns":
        [
            { "mData": "ProductName", 'sClass': 'left' },
            {
                "mData": "Merchant", 'sClass': 'left',
                "mRender": function (data, type, full) {
                    if (data != null) {
                        return data.MerchantName;
                    }
                    return "";
                }
            },
            {
                "mData": "IsRecommend", 'sClass': 'left',
                "mRender": function (data, type, full) {
                    if (data) {
                        return '<span class="label label-sm label-warning">是</span>';
                    }
                    else {
                        return '<span class="label label-sm label-default">否</span>';
                    }
                }
            },
            {
                "mData": "IsShowInFront", 'sClass': 'left',
                "mRender": function (data, type, full) {
                    if (data) {
                        return '<span class="label label-sm label-warning">是</span>';
                    }
                    else {
                        return '<span class="label label-sm label-default">否</span>';
                    }
                }
            },
            {
                "mData": "CreateTime", 'sClass': 'left',
                "mRender": function (data, type, full) {
                    if (data != null && data.length > 0) {
                        return eval('new ' + data.replace(/\//g, '')).Format("yyyy-MM-dd hh:mm:ss");
                    }
                    return "";
                }
            },
            {
                "mData": "Status",
                'sClass': 'left',
                "orderable": false,
                "mRender": function (data, type, full) {
                    if (data == -1) {
                        return '<span class="label label-sm label-default">已删除</span>';
                    }
                    else if (data == 0) {
                        return '<span class="label label-sm label-danger">已禁用</span>';
                    }
                    else if (data == 1) {
                        return '<span class="label label-sm label-success">正常</span>';
                    } else {
                        return "";
                    }
                }
            },
            {
                "mData": "Id",
                'sClass': 'center',
                "sWidth": "150px",
                "orderable": false,
                "mRender": function (data, type, full) {
                    var render = '<div class="visible-md visible-lg hidden-sm hidden-xs action-buttons">';
                    if (canViewMerchant)
                        render += '<a class="green view" data-id="' + full.Id + '" href="' + url_viewMerchant + '&id=' + full.Id + '" title="查看"><i class="icon-eye-open bigger-130"></i></a>';

                    if (canEditMerchant)
                        render += '<a class="blue edit" data-id="' + full.Id + '" href="' + url_editMerchant + '&id=' + full.Id + '" title="编辑"><i class="icon-pencil bigger-130"></i></a>';
                    if (canDeleteMerchant)
                        render += '<a class="red delete" data-id="' + full.Id + '" href="#" title="删除"><i class="icon-trash bigger-130"></i></a>';
                    if (canEditAdvert) {
                        var url = url_sendAdvert.replace('%5BsourceId%5D', full.Id).replace('%5BsourceTitle%5D', full.ProductName).replace('%5BviewUrl%5D', "");
                        render += '<a class="blue" data-id="' + full.Id + '" href="' + url + '" title="设为广告"><i class="icon-barcode bigger-130"></i></a>';
                    }
                    render += '</div>';
                    return render;
                }
            }
        ]
    });

    //查询
    $('#QueryButton').on("click", function () {
        productName = $("#ProductName").val();
        merchantId = $("#MerchantId").val();
        loadTable.api().ajax.reload();
    });
    
    $('#MerchantProductInfoTable').on("click", ".delete", function (e) {
        var id = $(this).data("id");

        bntToolkit.confirm("删除后不可恢复，确定还要删除吗？", function () {
            bntToolkit.post(url_deleteMerchant, { id: id }, function (result) {
                if (result.Success) {
                    $("#MerchantProductInfoTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });
});