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
	    beforeCheck: zTreeBeforeCheck,
		beforeClick: beforeClick,
		onCheck: onCheck
	}
};

function beforeClick(treeId, treeNode) {
	var zTree = $.fn.zTree.getZTreeObj("categoryTree");
	zTree.checkNode(treeNode, !treeNode.checked, null, true);
	return false;
}

function zTreeBeforeCheck(treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj("categoryTree");
    var nodes = zTree.getCheckedNodes(true);
    for (var i = 0, l = nodes.length; i < l; i++) {
        zTree.checkNode(nodes[i], false, true);
    }
};


function onCheck(e, treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj("categoryTree"),
    nodes = zTree.getCheckedNodes(true),
    v = "",
    ids = "";
	for (var i = 0, l = nodes.length; i < l; i++) {
		v += nodes[i].name + ",";
	    ids += nodes[i].id + ",";
	}
	if (v.length > 0) v = v.substring(0, v.length - 1);
	if (ids.length > 0) ids = ids.substring(0, ids.length - 1);
	$("#CategoryNames").attr("value", v);
	$("#CategoryIds").attr("value", ids);
	hideMenu();
}

function showMenu() {
	var inputCategoryNames = $("#CategoryNames");
	var cityOffset = $("#CategoryNames").offset();
	$("#menuContent").css({ left: cityOffset.left + "px", top: cityOffset.top + inputCategoryNames.outerHeight() + "px", width: inputCategoryNames.width() }).slideDown("fast");

	$("body").bind("mousedown", onBodyDown);
}

function hideMenu() {
	$("#menuContent").fadeOut("fast");
	$("body").unbind("mousedown", onBodyDown);
}
function onBodyDown(event) {
	if (!(event.target.id == "menuBtn" || event.target.id == "CategoryNames" || event.target.id == "menuContent" || $(event.target).parents("#menuContent").length > 0)) {
		hideMenu();
	}
}

$(document).ready(function() {
    $.fn.zTree.init($("#categoryTree"), setting, zNodes);
    var typeTree = $.fn.zTree.getZTreeObj("categoryTree");
    typeTree.expandAll(true); //展开所有节点
    var nodes = typeTree.getNodes();
    for (var i = 0; i < nodes.length; i++) {
        console.log(nodes[i].children != undefined);
        if (nodes[i].children != undefined) {
            typeTree.setChkDisabled(nodes[i], true);
            checkChildren(nodes[i].children, typeTree);
        }
    }
});

function checkChildren(childNodes, typeTree) {
    for (var k = 0; k < childNodes.length; k++) {
        if (childNodes[k].children != undefined) {
            typeTree.setChkDisabled(childNodes[k], true);
            checkChildren(childNodes[k]);
        }
    }
}

jQuery(function ($) {
	bntToolkit.initForm($("#HelpForm"), {
		Title: {
		    required: true,
		    maxlength: 50
		},
		CategoryNames: {
			required: true
		}
	}, null, success);
});

// post-submit callback
function success(result, statusText, xhr, $form) {
	if (!result.Success) {
		bntToolkit.error(result.ErrorMessage);
	} else {
		location.href = url_loadHelp;
	}
}