
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
        beforeClick: beforeClick,
        onClick:onClick
    }
};

var curParentNode;
var curTreeNode;
var newCount = 1;

function addHoverDom(treeId, treeNode) {
    if (!canEditMerchantType) return;

    var sObj = $("#" + treeNode.tId + "_a");

    var addStr;
    var btn;
    if (treeNode.getParentNode() == null) {
        if (treeNode.editNameFlag || $("#AddBtn_" + treeNode.tId).length > 0) return;
        addStr = "<span class='button add' style='margin-left:10px;' id='AddBtn_" + treeNode.tId
         + "' title='添加子分类' onfocus='this.blur();'></span>";
        sObj.append(addStr);

        btn = $("#AddBtn_" + treeNode.tId);
        if (btn) btn.bind("click", function () {
            zTree.selectNode(treeNode);
            var parentNode = treeNode.getParentNode();

            curTreeNode = treeNode;
            curParentNode = parentNode;
            //新建节点
            $("#ParentId").val(curTreeNode.Id);
            $("#Parent").val(curTreeNode.TypeName);

            $("#Id").val("");
            $("#TypeName").val("");
            $("#Remark").val("");
            $("#cboxIsShowInNav").attr("checked", false);
            $("#IsShowInNav").val("false");
            $("#Sort").val("");

            $("#EditMode").val(2);
            $("#SaveButton").show();
            $("#SetAdvs").hide();
            $("#AddCarousel").hide();

            //取联合数据
            setMergerData(curParentNode);
            return false;
        });
    }

    if (treeNode.editNameFlag || $("#EditBtn_" + treeNode.tId).length > 0) return;
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
            parent = parentNode.TypeName;
        }
        if (parent === "") parent = "顶级";
        $("#Parent").val(parent);

        $("#Id").attr("readonly", "readonly");
        $("#Id").val(treeNode.Id);
        $("#TypeName").val(treeNode.TypeName);
        $("#Remark").val(treeNode.Remark);
        $("#cboxIsShowInNav").attr("checked", treeNode.IsShowInNav);
        $("#IsShowInNav").val(treeNode.IsShowInNav);
        $("#Sort").val(treeNode.Sort);

        $("#EditMode").val(1);

        $("#SaveButton").show();
        $("#SetAdvs").show();
        $("#AddCarousel").show();

        curTreeNode = treeNode;
        curParentNode = parentNode;

        //取联合数据
        setMergerData(curParentNode);
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
            bntToolkit.post(url_deleteMerchantType, { typeId: treeNode.Id }, function (result) {
                if (result.Success) {
                    //删除节点
                    var nodes = zTree.getSelectedNodes();
                    if (nodes.length > 0) {
                        zTree.removeNode(nodes[0]);
                        //清空数据
                        reset();
                        bntToolkit.success("删除成功");
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

//取联合数据
function setMergerData(parentNode) {
    var merName = "";
    var merId = "";
    while (parentNode != null && parentNode != undefined) {
        merName = parentNode.TypeName + "," + merName;
        merId = parentNode.Id + "," + merId;
        parentNode = parentNode.getParentNode();
    }
    merName = merName.substring(0, merName.length - 1);
    merId = merId.substring(0, merId.length - 1);
    $("#MergerId").val(merId);
    $("#MergerTypeName").val(merName);
}

function removeHoverDom(treeId, treeNode) {
    $("#AddBtn_" + treeNode.tId).unbind().remove();
    $("#EditBtn_" + treeNode.tId).unbind().remove();
    $("#DeleteBtn_" + treeNode.tId).unbind().remove();
};

function onAsyncSuccess(event, treeId, msg) {
    var nodes = zTree.getNodes();
    if (nodes[0])
        zTree.expandNode(nodes[0], true);
    if (nodes[1])
        zTree.expandNode(nodes[1], true);
}

function beforeClick(treeId, treeNode) {
    if (treeNode.Id === "0") return false;
    return true;
}

function onClick(treeId, treeNode) {
    reset();
}

function filter(treeId, parentNode, childNodes) {
    if (!childNodes) return null;
    for (var i = 0, l = childNodes.length; i < l; i++) {

        childNodes[i].pId = childNodes[i].ParentId;
        childNodes[i].name = childNodes[i].TypeName;
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

    bntToolkit.initForm($("#DistrictForm"), {
        TypeName: {
            required: true
        },
        Sort: {
            required: true,
            digits: true
        }
    }, null, success);

    if (!canEditMerchantType) {
        $("#DistrictForm input").attr("readonly", "readonly");
    }

    $("#cboxIsShowInNav").click(function () {
        $("#IsShowInNav").val($("#cboxIsShowInNav").is(':checked'));
    });

    $("#SetAdvs").click(function () {
        var typeId = $("#Id").val();
        var typeName = $("#TypeName").val();
        var url = url_sendAdvert.replace('%5BsourceId%5D', typeId).replace('%5BsourceTitle%5D', typeName).replace('%5BviewUrl%5D', "").replace(/amp;/g,"");
        location.href = url;
    });

    $("#AddCarousel").click(function () {
        var typeId = $("#Id").val();
        var typeName = $("#TypeName").val();
        var url = url_addCarousel.replace('%5BsourceId%5D', typeId).replace('%5BsourceTitle%5D', typeName).replace('%5BviewUrl%5D', "").replace(/amp;/g, "");
        location.href = url;
    });

});

function reset() {
    $("#ParentId").val("");
    $("#Parent").val("");
    $("#Id").val("");
    $("#TypeName").val("");
    $("#Remark").val("");
    $("#cboxIsShowInNav").attr("checked", false);
    $("#IsShowInNav").val("false");
    $("#Sort").val("");

    $("#EditMode").val(0);
    $("#SaveButton").hide();
    $("#SetAdvs").hide();
    $("#AddCarousel").hide();
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
                //currentNode.Id = result.Data;
                currentNode.TypeName = $("#TypeName").val();
                currentNode.Remark = $("#Remark").val();
                currentNode.IsShowInNav = $("#cboxIsShowInNav").is(':checked');
                currentNode.Sort = $("#Sort").val();
                currentNode.name = $("#TypeName").val();

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

            bntToolkit.success("保存成功");
        }
    }
}

$(window).resize(function () {
    $("#treeParent").attr("style", "max-height:" + ($(window).height() - 200) + "px;overflow-y:auto;");
});
$(window).trigger("resize");