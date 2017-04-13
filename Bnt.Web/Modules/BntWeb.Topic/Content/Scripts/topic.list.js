jQuery(function ($) {
    var options = $.datepicker.regional["zh-CN"];
    options["dateFormat"] = "yy-mm-dd";
    $("#CreateTimeBegin").datepicker(options);
    $("#CreateTimeEnd").datepicker(options);

    var typeId = $("#TypeId").val();
    var isHot = $("#IsHot").val();
    var topicContent = $("#TopicContent").val();
    var memberName = $("#MemberName").val();
    var createTimeBegin = $("#CreateTimeBegin").val();
    var createTimeEnd = $("#CreateTimeEnd").val();


    var loadTable = $('#TopicInfoTable').dataTable({
        "processing": true,
        "serverSide": true,
        "sorting": [[2, "desc"]],
        "ajax": {
            "url": url_loadPage,
            "data": function (d) {
                //添加额外的参数传给服务器
                d.extra_search = { "TypeId": typeId, "IsHot":isHot,"TopicContent": topicContent, "MemberName": memberName, "CreateTimeBegin": createTimeBegin, "CreateTimeEnd": createTimeEnd };
            }
        },
        "aoColumns":
        [
            {
                "mData": "TopicType", 'sClass': 'left', "orderable": false,
                "mRender": function (data, type, full) {
                    if (data != null) {
                        return data.TypeName;
                    }
                    return "";
                }
            },
            { "mData": "TopicContent", 'sClass': 'left', "orderable": false },
            {
                "mData": "CreateTime", 'sClass': 'left',
                "mRender": function (data, type, full) {
                    if (data != null && data.length > 0) {
                        return eval('new ' + data.replace(/\//g, '')).Format("yyyy-MM-dd hh:mm:ss");
                    }
                    return "";
                }
            },
            { "mData": "MemberName", 'sClass': 'left', "orderable": false },
            {
                "mData": "Status",
                'sClass': 'left',
                "orderable": false,
                "mRender": function (data, type, full) {
                    if (data == 1) {
                        return '<span class="label label-sm label-default">正常</span>';
                    }
                    else {
                        return '<span class="label label-sm label-warning">未审核</span>';
                    }
                }
            },
            //{
            //    "mData": "IsHot",
            //    'sClass': 'left',
            //    "orderable": false,
            //    "mRender": function (data, type, full) {
            //        if (data == 1) {
            //            return '<span class="label label-sm label-danger">是</span>';
            //        }
            //        else {
            //            return '<span class="label label-sm label-default">否</span>';
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
                    if (canSetHot) {
                        if (full.IsHot == 0)
                            render += '<a class="red setHot" data-id="' + full.Id + '" href="#" title="设为热门"><i class="icon-fire bigger-130"></i></a>';
                        else {
                            render += '<a class="green setHot" data-id="' + full.Id + '" href="#" title="取消热门"><i class="icon-fire bigger-130"></i></a>';
                        }
                    }
                    render += '<a class="green view" data-id="' + full.Id + '" href="' + url_viewTopic + '?topicId=' + full.Id + '" title="查看"><i class="icon-eye-open bigger-130"></i></a>';
                    if (canManageComment)
                        render += '<a class="blue" data-id="' + full.Id + '" href="' + url_commentList.replace('[sourceId]', full.Id) + '" title="评论"><i class="icon-comments-alt bigger-130"></i></a>';
                    if (canDeleteTopic)
                        render += '<a class="red delete" data-id="' + full.Id + '" href="#" title="删除"><i class="icon-trash bigger-130"></i></a>';
                    if (canEditCarousel) {
                        var url = url_addCarousel.replace('%5BsourceId%5D', full.Id).replace('%5BsourceTitle%5D', full.TopicContent.substring(0, 10)).replace('%5BviewUrl%5D', "");
                        render += '<a class="blue" data-id="' + full.Id + '" href="' + url + '" title="加入轮播"><i class="icon-magic bigger-130"></i></a>';
                    }
                    render += '</div>';
                    return render;
                }
            }
        ]
    });

    //查询
    $('#QueryButton').on("click", function () {
        typeId = $("#TypeId").val();
        isHot = $("#IsHot").val();
        topicContent = $("#TopicContent").val();
        memberName = $("#MemberName").val();
        createTimeBegin = $("#CreateTimeBegin").val();
        createTimeEnd = $("#CreateTimeEnd").val();
        loadTable.api().ajax.reload();
    });

    $('#TopicInfoTable').on("click", ".delete", function (e) {
        var id = $(this).data("id");

        bntToolkit.confirm("删除后不可恢复，确定还要删除吗？", function () {
            bntToolkit.post(url_deleteTopic, { topicId: id }, function (result) {
                if (result.Success) {
                    $("#TopicInfoTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });

    //设置为热门
    $('#TopicInfoTable').on("click", ".setHot", function (e) {
        var id = $(this).data("id");

        bntToolkit.confirm("确定要设置或取消热门？", function () {
            bntToolkit.post(url_setHot, { topicId: id }, function (result) {
                if (result.Success) {
                    $("#TopicInfoTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });
});