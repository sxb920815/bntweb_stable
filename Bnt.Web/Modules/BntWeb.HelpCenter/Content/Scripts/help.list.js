var setting = {
    check: {
        enable: false,
        chkboxType: { "Y": "", "N": "" }
    },
    view: {
        dblClickExpand: false
    },
    data: {
        simpleData: {
            enable: true
        }
    },
    callback: {
        onClick: zTreeOnClick
    }
};

function zTreeOnClick(e, treeId, treeNode) {
    $("#CategoryName").attr("value", treeNode.name);
    $("#CategoryId").attr("value", treeNode.id);
    hideMenu();
};

function showMenu() {
    var inputCategoryNames = $("#CategoryName");
    var cityOffset = $("#CategoryName").offset();
    $("#menuContent").css({ left: cityOffset.left + "px", top: cityOffset.top + inputCategoryNames.outerHeight() + "px", width: inputCategoryNames.width() }).slideDown("fast");

    $("body").bind("mousedown", onBodyDown);
}

function hideMenu() {
    $("#menuContent").fadeOut("fast");
    $("body").unbind("mousedown", onBodyDown);
}
function onBodyDown(event) {
    if (!(event.target.id == "menuBtn" || event.target.id == "CategoryName" || event.target.id == "menuContent" || $(event.target).parents("#menuContent").length > 0)) {
        hideMenu();
    }
}

$(document).ready(function () {
    $.fn.zTree.init($("#categoryTree"), setting, zNodes);
    var typeTree = $.fn.zTree.getZTreeObj("categoryTree");
    typeTree.expandAll(true); //展开所有节点
});


jQuery(function ($) {

    var categoryId = $("#CategoryId").val();
    var title = $("#Title").val();
    var createName = $("#CreateName").val();

    var loadTable = $('#HelpInfoTable').dataTable({
        "processing": true,
        "serverSide": true,
        "sorting": [[3, "desc"]],
        "ajax": {
            "url": url_loadPage,
            "data": function (d) {
                //添加额外的参数传给服务器
                d.extra_search = { "CategoryId": categoryId, "Title": title, "CreateName": createName };
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
                "mData": "HelpCategory", 'sClass': 'left', "orderable": false,
                "mRender": function (data, type, full) {
                    if (data != null) {
                        return data.Name;
                    }
                    return "";
                }
            },
            {
                "mData": "CreateTime", 'sClass': 'left',
                "mRender": function (data, type, full) {
                    if (data != null && data.length > 0) {
                        return eval('new ' + data.replace(/\//g, '')).Format("yyyy-MM-dd hh:mm");
                    }
                    return "";
                }
            },
            { "mData": "CreateName", 'sClass': 'left', "orderable": false },
            {
                "mData": "Id",
                'sClass': 'center',
                "sWidth": "150px",
                "orderable": false,
                "mRender": function (data, type, full) {
                    var render = '<div class="visible-md visible-lg hidden-sm hidden-xs action-buttons">';
                    render += '<a class="green view" data-id="' + full.Id + '" href="' + url_editHelp + '?id=' + full.Id + '&isView=true" title="查看"><i class="icon-eye-open bigger-130"></i></a>';
                    render += '<a class="blue view" data-id="' + full.Id + '" href="' + url_editHelp + '?id=' + full.Id + '&isView=false" title="编辑"><i class="icon-pencil bigger-130"></i></a>';
                    if (canDeleteHelp)
                        render += '<a class="red delete" data-id="' + full.Id + '" href="#" title="删除"><i class="icon-trash bigger-130"></i></a>';
                    render += '</div>';
                    return render;
                }
            }
        ]
    });

    //查询
    $('#QueryButton').on("click", function () {
        categoryId = $("#CategoryId").val();
        title = $("#Title").val();
        createName = $("#CreateName").val();
        loadTable.api().ajax.reload();
    });

    $('#HelpInfoTable').on("click", ".view", function (e) {
        var id = $(this).data("id");
    });

    $('#HelpInfoTable').on("click", ".delete", function (e) {
        var id = $(this).data("id");

        bntToolkit.confirm("删除后不可恢复，确定还要删除吗？", function () {
            bntToolkit.post(url_deleteHelp, { helpId: id }, function (result) {
                if (result.Success) {
                    $("#HelpInfoTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });
});