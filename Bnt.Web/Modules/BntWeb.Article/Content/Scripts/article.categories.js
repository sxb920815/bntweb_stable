
var setting = {
    async: {
        enable: true,
        url: url_load,
        autoParam: ["Id=pid"],
        dataFilter: filter
    },
    view: {
        expandSpeed: "",
        addHoverDom: addHoverDom,
        removeHoverDom: removeHoverDom,
        selectedMulti: false
    },
    callback: {
        onAsyncSuccess: onAsyncSuccess,
        beforeClick: beforeClick
       // onClick: onClick
    }
};

var curParentNode;
var curTreeNode;
var newCount = 1;
function addHoverDom(treeId, treeNode) {
    //if (treeNode.Level >= 4 || !canEditCategories) return;

    var sObj = $("#" + treeNode.tId + "_span");
    if (treeNode.editNameFlag || $("#AddBtn_" + treeNode.tId).length > 0) return;
    var addStr;
   
    addStr = "<span class='button add' style='margin-left:10px;' id='AddBtn_" + treeNode.tId
         + "' title='添加子分类' onfocus='this.blur();'></span>";
    sObj.after(addStr);
    var btn = $("#AddBtn_" + treeNode.tId);
    if (btn) btn.bind("click", function () {
        zTree.selectNode(treeNode);

        var parentNode = treeNode.getParentNode();

        curTreeNode = treeNode;
        curParentNode = parentNode;

        //新建节点
        $("#ParentId").val(treeNode.Id);
        var parent = treeNode.Name;
        if (parent === "") parent = "顶级";
        $("#Parent").val(parent);

        $("#Id").removeAttr("readonly");
        $("#Id").val("");
        $("#Name").val("");
        $("#Sort").val("");

        $("#EditMode").val(2);

        //$("#DeleteButton").hide();

        return false;
    });

    if (treeNode.getParentNode() == null) {
        return;
    }
    addStr = "<span class='button edit' style='margin-left:10px;' id='EditBtn_" + treeNode.tId
           + "' title='编辑' onfocus='this.blur();'></span>";
    sObj.append(addStr);

    btn = $("#EditBtn_" + treeNode.tId);
    if (btn) btn.bind("click", function () {
        zTree.selectNode(treeNode);

        $("#ParentId").val(treeNode.ParentId);
        var parentNode = treeNode.getParentNode();
        var parent = "";
        if (parentNode != null) {
            parent = parentNode.Name;
        }
        if (parent === "") parent = "顶级";
        $("#Parent").val(parent);

        $("#Id").attr("readonly", "readonly");
        $("#Id").val(treeNode.Id);

        $("#Name").val(treeNode.Name);
        $("#Sort").val(treeNode.Sort);

        $("#EditMode").val(1);

        $("#SaveButton").show();

        curTreeNode = treeNode;
        curParentNode = parentNode;

        //取联合数据
        //setMergerData(curParentNode);
        return false;
    });

    if (treeNode.getParentNode() == null) {
        return;
    }
    addStr = "<span class='button remove' style='margin-left:10px;' id='DeleteBtn_" + treeNode.tId
        + "' title='删除分类' onfocus='this.blur();'></span>";
    sObj.append(addStr);

    btn = $("#DeleteBtn_" + treeNode.tId);
    if (btn) btn.bind("click", function () {
        bntToolkit.confirm("删除后不可恢复，确定还要删除吗？", function () {
            bntToolkit.post(url_deleteCategories, { Id: treeNode.Id }, function (result) {
                if (result.Success) {
                    //删除节点
                    var nodes = zTree.getSelectedNodes();
                    if (nodes.length > 0) {
                        zTree.removeNode(nodes[0]);
                        //清空数据
                        reset();
                        return false;
                    }
                } else {
                    bntToolkit.error(result.ErrorMessage);
                    return false;
                }
            });
        });
    });
};
function removeHoverDom(treeId, treeNode) {
   
    $("#AddBtn_" + treeNode.tId).unbind().remove();
    $("#EditBtn_" + treeNode.tId).unbind().remove();
    $("#DeleteBtn_" + treeNode.tId).unbind().remove();
};

function onAsyncSuccess(event, treeId, msg) {
    var nodes = zTree.getNodes();
    zTree.expandNode(nodes[0], true);
}

function beforeClick(treeId, treeNode) {
   
    if (treeNode.Id === "0") return false;
    return true;
}

function onClick(event, treeId, treeNode, clickFlag) {
    
    //if (treeNode.Id === "0") return;
    //if (!canEditCategories) {
    //    $("#CategoriesForm input").attr("readonly", "readonly");
    //}


    //$("#ParentId").val(treeNode.ParentId);
    //var parentNode = treeNode.getParentNode();
    //var parent = "";
    //if (parentNode != null) {
    //    parent = parentNode.Name;
    //}
    //if (parent === "") parent = "顶级";

    //$("#Parent").val(parent);

    //$("#Id").attr("readonly", "readonly");
    //$("#Id").val(treeNode.Id);
    //$("#Name").val(treeNode.Name);
   
    //$("#Sort").val(treeNode.Sort);

    //$("#EditMode").val(1);

    //$("#DeleteButton").show();

    ////$("#NewThisButton").show();
    ////$("#NewChildButton").show();

    //$("#DistrictForm").valid();

  

    //curTreeNode = treeNode;
    //curParentNode = parentNode;
   

}

function filter(treeId, parentNode, childNodes) {
    if (!childNodes) return null;
    for (var i = 0, l = childNodes.length; i < l; i++) {

        childNodes[i].pId = childNodes[i].ParentId;
        childNodes[i].name = childNodes[i].Name;
        childNodes[i].isParent = true;
        //childNodes[i].icon = iconOpen;
        //childNodes[i].iconOpen = iconOpen;
        //childNodes[i].iconClose = iconClose;
    }
    return childNodes;
}

var zTree;
jQuery(function ($) {
    zTree = $.fn.zTree.init($("#tree"), setting);
  
    bntToolkit.initForm($("#CategoriesForm"), {
        Name: {
            required: true
        },
        Sort: {
            required: true,
            digits: true
        }
    }, null, success);

    if (!canEditCategories) {
        $("#CategoriesForm input").attr("readonly", "readonly");
    }
 

    //$("#DeleteButton").click(function () {
    //    bntToolkit.confirm("删除后不可恢复，确定还要删除该分类吗？", function () {
    //        bntToolkit.post(url_deleteCategories, { Id: $("#Id").val() }, function (result) {
    //            if (result.Success) {
    //                //删除节点
    //                var nodes = zTree.getSelectedNodes();
    //                if (nodes.length > 0)
    //                    zTree.removeNode(nodes[0]);
    //            } else {
    //                bntToolkit.error(result.ErrorMessage);
    //            }
    //        });
    //    });
    //});
});

function reset() {
    $("#ParentId").val("");
    $("#Parent").val("");
    $("#Id").val("");
    $("#Name").val("");
    $("#Sort").val("");

    $("#EditMode").val(0);
    $("#DeleteButton").hide();
    $("#SaveButton").hide();
    $("#NewThisButton").hide();
    $("#NewChildButton").hide();
}

// post-submit callback
function success(result, statusText, xhr, $form) {
    if (!result.Success) {
        bntToolkit.error(result.ErrorMessage);
    } else {
        //刷新节点
        var nodes = zTree.getSelectedNodes();
        if (nodes.length > 0) {
            var currentNode = nodes[0];
            if ($("#EditMode").val() === "1") {
                //更新本地字段

                currentNode.Name = $("#Name").val();
            
                currentNode.Sort = $("#Sort").val();

                $("#Id").val(result.Data);
                zTree.updateNode(currentNode);
                //bntToolkit.alert("保存成功",2);
            }
            else if ($("#EditMode").val() === "0") {
                zTree.reAsyncChildNodes(curParentNode, "refresh");
                //刷新兄弟
            }
            else if ($("#EditMode").val() === "2") {
                //刷新孩子
                zTree.reAsyncChildNodes(currentNode, "refresh");
            }

            reset();
        }
    }
}

$(window).resize(function () {
    $("#treeParent").attr("style", "max-height:" + ($(window).height() - 200) + "px;overflow-y:auto;");
});
$(window).trigger("resize");