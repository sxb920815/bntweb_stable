
var level = 0;
var renderItem = function (category, render) {
    level++;
    if (category.Childs != null && category.Childs.length > 0) {
        render += '<li class="dd-item dd2-item"><div class="dd-handle dd2-handle"><img src="/' + category.HelpCategoryLogo + '" width="36" height="38" /></div>';
        render += '<div class="dd2-content">' + category.Name + '<div class="pull-right action-buttons">';
        if (canEditHelpCategory) {
            render += '<a class="blue" href="' + url_editCategory + '?id=' + category.Id + '&parentId=00000000-0000-0000-0000-000000000000" title="编辑"><i class="icon-pencil bigger-130"></i></a>';

            if (level <= 0)
                render += '<a class="blue" href="' + url_editCategory + '?id=00000000-0000-0000-0000-000000000000&parentId=' + category.Id + '" title="添加子分类"><i class="icon-plus bigger-130"></i></a>';
        }
        if (canDeleteHelpCategory) {
            render += '<a class="red delete" href="javascript:;" data-id="' + category.Id + '" ><i class="icon-trash bigger-130"></i></a>';
        }
        render += '</div></div><ol class="dd-list">';
        for (var i = 0; i < category.Childs.length; i++) {
            var categoryInner = category.Childs[i];
            render = renderItem(categoryInner, render);
        }
        render += '</ol></li>';
    } else {
        render += '<li class="dd-item dd2-item"><div class="dd-handle dd2-handle"><img src="/' + category.HelpCategoryLogo + '" width="36" height="38" /></div>';
        render += '<div class="dd2-content">' + category.Name + '<div class="pull-right action-buttons">';
        if (canEditHelpCategory) {
            render += '<a class="blue" href="' + url_editCategory + '?id=' + category.Id + '&parentId=00000000-0000-0000-0000-000000000000" title="编辑"><i class="icon-pencil bigger-130"></i></a>';

            if (level <= 0)
                render += '<a class="blue" href="' + url_editCategory + '?id=00000000-0000-0000-0000-000000000000&parentId=' + category.Id + '" title="添加子分类"><i class="icon-plus bigger-130"></i></a>';
        }
        render += '<a class="green" href="' + url_viewHelp + '?categoryId=' + category.Id + '" title="查看帮助列表"><i class="icon-eye-open bigger-130"></i></a>';
        if (canDeleteHelpCategory) {
            render += '<a class="red delete" href="javascript:;" data-id="' + category.Id + '" ><i class="icon-trash bigger-130"></i></a>';
        }
        render += '</div></div></li>';
    }
    return render;
}

var load = function (index) {
    var pageSize = 10;
    bntToolkit.post(url_load, { pageIndex: index, pageSize: pageSize }, function (result) {
        console.log(result);
        if (result.length > 0) {
            var render = '<div class="dd dd-draghandle" id="list_' + index + '" style="max-width: 100%;"><ol class="dd-list">';
            for (var i = 0; i < result.length; i++) {
                var category = result[i];
                level = 0;
                render = renderItem(category, render);
            }
            render += '</ol"></div>';

            $("#HelpCategoryList").html(render);
            //$("#pageDiv").html(bntToolkit.buildPagination(index, pageSize, result.Data.TotalCount));

            $('#list_' + index).nestable({ handleClass: "empty", collapseAll: true });

            $('.dd-handle a').on('mousedown', function (e) {
                e.stopPropagation();
            });

            $('[data-rel="tooltip"]').tooltip();
            $('#list_' + index).nestable("collapseAll");

        } else {
            var render = "没有数据...";
            $("#HelpCategoryList").html(render);
        }
    });
}

jQuery(function ($) {

    load(1);

    $("#HelpCategoryList").on("click", ".delete", function () {
        var id = $(this).data("id");

        bntToolkit.confirm("删除后不可恢复，确定还要删除该分类吗？", function () {
            bntToolkit.post(url_deleteCategory, { categoryId: id }, function (result) {
                if (result.Success) {

                    load(1);
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });

});