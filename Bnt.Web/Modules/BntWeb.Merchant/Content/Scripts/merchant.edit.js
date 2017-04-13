var setting = {
    check: {
        enable: true,
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
        beforeClick: beforeClick,
        onCheck: onCheck
    }
};

function beforeClick(treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj("typeTree");
    zTree.checkNode(treeNode, !treeNode.checked, null, true);
    return false;
}

function onCheck(e, treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj("typeTree"),
    nodes = zTree.getCheckedNodes(true),
    v = "",
    ids = "";
    for (var i = 0, l = nodes.length; i < l; i++) {
        v += nodes[i].name + ",";
        ids += nodes[i].id + ","
    }
    if (v.length > 0) v = v.substring(0, v.length - 1);
    if (ids.length > 0) ids = ids.substring(0, ids.length - 1);
    $("#TypeName").attr("value", v);
    $("#TypeIds").attr("value", ids);

}

function showMenu() {
    var inputTypeName = $("#TypeName");
    var cityOffset = $("#TypeName").position();
    $("#menuContent").css({ left: cityOffset.left + "px", top: cityOffset.top + inputTypeName.outerHeight() + "px", width: inputTypeName.width() }).slideDown("fast");

    $("body").bind("mousedown", onBodyDown);
}
function hideMenu() {
    $("#menuContent").fadeOut("fast");
    $("body").unbind("mousedown", onBodyDown);
}
function onBodyDown(event) {
    if (!(event.target.id == "menuBtn" || event.target.id == "TypeName" || event.target.id == "menuContent" || $(event.target).parents("#menuContent").length > 0)) {
        hideMenu();
    }
}

$(document).ready(function () {
    $.fn.zTree.init($("#typeTree"), setting, zNodes);

    var typeTree = $.fn.zTree.getZTreeObj("typeTree");
    typeTree.expandAll(true);//展开所有节点
});

jQuery(function ($) {
    bntToolkit.initForm($("#MerchantForm"), {
        MerchantName: {
            required: true,
            maxlength: 100
        },
        PhoneNumber: {
            required: true,
            maxlength: 50
        },
        Intro: {
            required: true,
            maxlength: 200
        },
        OpenTime: {
            maxlength: 50
        },
        TypeName: {
            required: true
        },
        Address: {
            required: true,
            maxlength: 200
        }
    }, null, success);


    $("#cboxIsHowInFront").click(function () {
        $("#IsHowInFront").val($("#cboxIsHowInFront").is(':checked'));
    });
    $("#cboxIsRecommend").click(function () {
        $("#IsRecommend").val($("#cboxIsRecommend").is(':checked'));
    });
});

// post-submit callback
function success(result, statusText, xhr, $form) {
    if (!result.Success) {
        bntToolkit.error(result.ErrorMessage);
    } else {
        location.href = url_loadMerchant;
    }
}

function deleteRelation(imageId, merchantId) {
    bntToolkit.post(url_deleteRelation, { imageId: imageId, merchantId: merchantId }, function (result) {
        if (result.Success) {
            $("#background_" + imageId).remove();
        } else {
            bntToolkit.error(result.ErrorMessage);
        }
    });
}