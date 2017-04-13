jQuery(function ($) {
    var options = $.datepicker.regional["zh-CN"];
    options["dateFormat"] = "yy-mm-dd";
    $("#CreateTimeBegin").datepicker(options);
    $("#CreateTimeEnd").datepicker(options);

    var typeId = $("#TypeId").val();
    var status = $("#Status").val();
    var title = $("#Title").val();
    var memberName = $("#MemberName").val();
    var createTimeBegin = $("#CreateTimeBegin").val();
    var createTimeEnd = $("#CreateTimeEnd").val();


    var loadTable = $('#ActivityInfoTable').dataTable({
        "processing": true,
        "serverSide": true,
        "sorting": [[5, "desc"]],
        "ajax": {
            "url": url_loadPage,
            "data": function (d) {
                //添加额外的参数传给服务器
                d.extra_search = { "TypeId": typeId, "Status": status, "Title": title, "MemberName": memberName, "CreateTimeBegin": createTimeBegin, "CreateTimeEnd": createTimeEnd };
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
                "mData": "ActivityType", 'sClass': 'left', "orderable": false,
                "mRender": function (data, type, full) {
                    if (data != null) {
                        return data.TypeName;
                    }
                    return "";
                }
            },
            {
                "mData": "StartTime", 'sClass': 'left',
                "sWidth": "250px",
                "mRender": function (data, type, full) {
                    var render = "";
                    if (full.StartTime != null && full.StartTime.length > 0) {
                        render += eval('new ' + full.StartTime.replace(/\//g, '')).Format("yyyy-MM-dd hh:mm");
                    }
                    render += " - ";
                    if (full.EndTime != null && full.EndTime.length > 0) {
                        render += eval('new ' + full.EndTime.replace(/\//g, '')).Format("yyyy-MM-dd hh:mm");
                    }
                    return render;
                }
            },
            { "mData": "Postion", 'sClass': 'left', "orderable": false },
            {
                "mData": "CreateTime", 'sClass': 'left',
                "sWidth": "150px",
                "mRender": function (data, type, full) {
                    if (data != null && data.length > 0) {
                        return eval('new ' + data.replace(/\//g, '')).Format("yyyy-MM-dd hh:mm:ss");
                    }
                    return "";
                }
            },
            { "mData": "ApplyNum", 'sClass': 'left' },
            { "mData": "MemberName", 'sClass': 'left', "orderable": false },
            {
                "mData": "IsShowInFront",
                'sClass': 'left',
                "orderable": false,
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
                "mData": "IsBest",
                'sClass': 'left',
                "orderable": false,
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
                "mData": "Status",
                'sClass': 'left',
                "orderable": false,
                "mRender": function (data, type, full) {
                    if (data == 1) {
                        return '<span class="label label-sm label-warning">待举行</span>';
                    }
                    else if (data == 2) {
                        return '<span class="label label-sm label-danger">进行中</span>';
                    }
                    else if (data == 3) {
                        return '<span class="label label-sm label-success">已结束</span>';
                    }
                    else {
                        return '<span class="label label-sm label-default">已删除</span>';
                    }
                }
            },
            {
                "mData": "Id",
                'sClass': 'center',
                "sWidth": "200px",
                "orderable": false,
                "mRender": function (data, type, full) {
                    var render = '<div class="visible-md visible-lg hidden-sm hidden-xs action-buttons">';
                    if (full.ActivityType.TypeName == "官方认证") {
                        render += '<a class="blue view" data-id="' + full.Id + '" href="' + url_editActivity + '?id=' + full.Id + '&isView=false" title="编辑"><i class="icon-pencil bigger-130"></i></a>';
                    }
                    render += '<a class="green view" data-id="' + full.Id + '" href="' + url_editActivity + '?id=' + full.Id + '&isView=true" title="查看"><i class="icon-eye-open bigger-130"></i></a>';
                    if (canManageComment)
                        render += '<a class="blue" data-id="' + full.Id + '" href="' + url_commentList.replace('[sourceId]', full.Id) + '" title="评论"><i class="icon-comments-alt bigger-130"></i></a>';
                    if (canViewApply)
                        render += '<a class="blue" data-id="' + full.Id + '" href="' + url_viewApply + '?activityId=' + full.Id + '" title="查看报名人员"><i class="icon-user bigger-130"></i></a>';
                    if (canDeleteActivity) {
                        render += '<a class="red delete" data-id="' + full.Id + '" href="#" title="删除"><i class="icon-trash bigger-130"></i></a>';
                        if (!full.IsShowInFront)
                            render += '<a class="red setHome" data-id="' + full.Id + '" href="#" title="推荐到首页"><i class="icon-fire bigger-130"></i></a>';
                        else {
                            render += '<a class="green setHome" data-id="' + full.Id + '" href="#" title="取消首页显示"><i class="icon-fire bigger-130"></i></a>';
                        }
                        if (!full.IsBest)
                            render += '<a class="red setBest" data-id="' + full.Id + '" href="#" title="加精"><i class="icon-thumbs-up bigger-130"></i></a>';
                        else {
                            render += '<a class="green setBest" data-id="' + full.Id + '" href="#" title="取消加精"><i class="icon-thumbs-up bigger-130"></i></a>';
                        }
                    }
                    if (canEditCarousel) {
                        var url = url_addCarousel.replace('%5BsourceId%5D', full.Id).replace('%5BsourceTitle%5D', full.Title.substring(0, 10)).replace('%5BviewUrl%5D', "");
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
        title = $("#Title").val();
        status = $("#Status").val();
        memberName = $("#MemberName").val();
        createTimeBegin = $("#CreateTimeBegin").val();
        createTimeEnd = $("#CreateTimeEnd").val();
        loadTable.api().ajax.reload();
    });

    $('#ActivityInfoTable').on("click", ".view", function (e) {
        var id = $(this).data("id");
    });

    $('#ActivityInfoTable').on("click", ".delete", function (e) {
        var id = $(this).data("id");

        bntToolkit.confirm("删除后不可恢复，确定还要删除吗？", function () {
            bntToolkit.post(url_deleteActivity, { activityId: id }, function (result) {
                if (result.Success) {
                    $("#ActivityInfoTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });

    //设为首页显示
    $('#ActivityInfoTable').on("click", ".setHome", function (e) {
        var id = $(this).data("id");

        bntToolkit.post(url_setHome, { activityId: id }, function (result) {
            if (result.Success) {
                $("#ActivityInfoTable").dataTable().fnDraw();
            } else {
                bntToolkit.error(result.ErrorMessage);
            }
        });
    });

    //设为首页显示
    $('#ActivityInfoTable').on("click", ".setBest", function (e) {
        var id = $(this).data("id");

        bntToolkit.post(url_setBest, { activityId: id }, function (result) {
            if (result.Success) {
                $("#ActivityInfoTable").dataTable().fnDraw();
            } else {
                bntToolkit.error(result.ErrorMessage);
            }
        });
    });
});