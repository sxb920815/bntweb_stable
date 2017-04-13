jQuery(function ($) {
    var options = $.datepicker.regional["zh-CN"];
    options["dateFormat"] = "yy-mm-dd";
    $("#CreateTimeBegin").datepicker(options);
    $("#CreateTimeEnd").datepicker(options);
   

    var typeId = $("#TypeId").val();
    var title = $("#Title").val();
    var createName = $("#CreateName").val();
    var createTimeBegin = $("#CreateTimeBegin").val();
    var createTimeEnd = $("#CreateTimeEnd").val();
   

    var loadTable = $('#ArticleInfoTable').dataTable({
        "processing": true,
        "serverSide": true,
        "sorting": [[4, "desc"]],
        "ajax": {
            "url": url_loadPage,
            "data": function (d) {
                //添加额外的参数传给服务器
                d.extra_search = { "TypeId": typeId, "Title": title, "CreateName": createName, "CreateTimeBegin": createTimeBegin, "CreateTimeEnd": createTimeEnd };
            }
        },
        "aoColumns":
        [
            {
                "mData": "Title",
                'sClass': 'left',
                "mRender": function (data, type, full) {
                    return "<a href=\"#\">" + data + "</a>";
                }
            },
            {
                "mData": "ArticleCategories", 'sClass': 'left', "orderable": false,
                "mRender": function (data, type, full) {
                    if (data != null) {
                        return data.Name;
                    }
                    return "";
                }
            },
           { "mData": "CreateName", 'sClass': 'left', "orderable": false },
            {
                "mData": "CreateTime", 'sClass': 'left',
                "mRender": function (data, type, full) {
                    if (data != null && data.length > 0) {
                        return eval('new ' + data.replace(/\//g, '')).Format("yyyy-MM-dd hh:mm:ss");
                    }
                    return "";
                }
            },
            //{
            //    "mData": "Status",
            //    'sClass': 'left',
            //    "orderable": false,
            //    "mRender": function (data, type, full) {
            //        if (data == 1) {
            //            return '<span class="label label-sm label-warning">正常</span>';
            //        }
            //        else if (data == 0) {
            //            return '<span class="label label-sm label-danger">其他</span>';
            //        }
                    
            //    }
            //},
            {
                "mData": "Id",
                'sClass': 'center',
                "sWidth": "150px",
                "orderable": false,
                "mRender": function (data, type, full) {
                    var render = '<div class="visible-md visible-lg hidden-sm hidden-xs action-buttons">';

                    render += '<a class="green view" data-id="' + full.Id + '" href="' + url_editArticle + '?id=' + full.Id + '&isView=true" title="查看"><i class="icon-eye-open bigger-130"></i></a>';
                    render += '<a class="blue view" data-id="' + full.Id + '" href="' + url_editArticle + '?id=' + full.Id + '&isView=false" title="编辑"><i class="icon-pencil bigger-130"></i></a>';

                    if (canEditCarousel) {
                        var url = url_addCarousel.replace('%5BsourceId%5D', full.Id).replace('%5BsourceTitle%5D', full.Title.substring(0, 10)).replace('%5BviewUrl%5D', "");
                        render += '<a class="blue" data-id="' + full.Id + '" href="' + url + '" title="加入轮播"><i class="icon-magic bigger-130"></i></a>';
                    }
                    if (canDeleteArticle)
                        render += '<a class="red delete" data-id="' + full.Id + '" href="#" title="删除"><i class="icon-trash bigger-130"></i></a>';
                    render += '</div>';
                    return render;
                }
            }
        ]
    });

    //查询
    $('#QueryButton').on("click", function () {
        typeId = $("#TypeId").val();
        title = $("#Title").val();
        createName = $("#CreateName").val();
        createTimeBegin = $("#CreateTimeBegin").val();
        createTimeEnd = $("#CreateTimeEnd").val();
      
        loadTable.api().ajax.reload();
    });

    $('#ArticleInfoTable').on("click", ".view", function (e) {
        var id = $(this).data("id");
    });

    $('#ArticleInfoTable').on("click", ".delete", function (e) {
        var id = $(this).data("id");
        
        bntToolkit.confirm("删除后不可恢复，确定还要删除吗？", function () {
            bntToolkit.post(url_deleteArticle, { articleId: id }, function (result) {
                if (result.Success) {
                    $("#ArticleInfoTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });
});

function deleteRelation(imageId, merchantId) {
    bntToolkit.post(url_deleteRelation, { imageId: imageId, merchantId: merchantId }, function (result) {
        if (result.Success) {
            $("#activityImage_" + imageId).remove();
        } else {
            bntToolkit.error(result.ErrorMessage);
        }
    });
}