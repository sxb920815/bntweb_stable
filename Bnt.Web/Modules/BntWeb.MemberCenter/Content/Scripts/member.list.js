jQuery(function ($) {
    var options = $.datepicker.regional["zh-CN"];
    options["dateFormat"] = "yy-mm-dd";
    $("#CreateTimeBegin").datepicker(options);
    $("#CreateTimeEnd").datepicker(options);

    var userName = $("#UserName").val();
    var nickName = $("#NickName").val();
    var sex = $("#Sex").val();
    var createTimeBegin = $("#CreateTimeBegin").val();
    var createTimeEnd = $("#CreateTimeEnd").val();

    var goodsTable = $('#MembersTable').dataTable({
        "processing": true,
        "serverSide": true,
        "sorting": [[6, "desc"]],
        "ajax": {
            "url": url_loadPage,
            "data": function (d) {
                //添加额外的参数传给服务器
                d.extra_search = { "UserName": userName, "NickName": nickName, "Sex": sex, "CreateTimeBegin": createTimeBegin, "CreateTimeEnd": createTimeEnd };
            }
        },
        "aoColumns":
		[
			{
			    "mData": "UserName",
			    'sClass': 'left',
			    "mRender": function (data, type, full) {
			        return "<a href=\"#\">" + data + "</a>";
			    }
			},
			{
			    "mData": "NickName", 'sClass': 'left'
			},
			{
			    "mData": "Sex", 'sClass': 'left',
			    "mRender": function (data, type, full) {
			        if (data == 1) return "男";
			        if (data == 2) return "女";
			        return "未知";
			    }
			},
			{
			    "mData": "Birthday", 'sClass': 'left',
			    "mRender": function (data, type, full) {
			        if (data != null && data.length > 0) {
			            return eval('new ' + data.replace(/\//g, '')).Format("yyyy-MM-dd");
			        }
			        return "";
			    }
			},
			{ "mData": "Email", 'sClass': 'left' },
			{ "mData": "PhoneNumber", 'sClass': 'left' },
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
			    "mData": "LockoutEnabled",
			    'sClass': 'left',
			    "mRender": function (data, type, full) {
			        if (data) {
			            return '<span class="label label-sm label-danger">已禁用</span>';
			        }
			        return '<span class="label label-sm label-success">已启用</span>';
			    }
			},
			{
			    "mData": "LockoutEnabled",
			    'sClass': 'center',
			    "sWidth": "150px",
			    "orderable": false,
			    "mRender": function (data, type, full) {
			        var render = '<div class="visible-md visible-lg hidden-sm hidden-xs action-buttons">';

			        if (full.UserName !== "bocadmin" && canSwitchMember) {
			            if (data) {
			                render += '<a class="green switch" data-id="' + full.Id + '" data-value="on" href="#" title="启用"><i class="icon-circle-blank bigger-130"></i></a>';
			            } else {
			                render += '<a class="red switch" data-id="' + full.Id + '" data-value="off" href="#" title="禁用"><i class="icon-circle bigger-130"></i></a>';
			            }
			        }

			        if (canEditMember)
			            render += '<a class="blue" href="' + url_editMember + '?id=' + full.Id + '" title="查看"><i class="icon-eye-open bigger-130"></i></a>';

			        if (full.UserName !== "bocadmin" && canDeleteMember)
			            render += '<a class="red delete" data-id="' + full.Id + '" href="#" title="删除"><i class="icon-trash bigger-130"></i></a>';

			        var cashBill = url_cashBill.replace('%5BmemberId%5D', full.Id);
			        render += '<a class="blue" data-id="' + full.Id + '" href="' + cashBill + '" title="现金明细"><i class=" icon-money bigger-130"></i></a>';

			        var integralBill = url_integralBill.replace('%5BmemberId%5D', full.Id);
			        render += '<a class="blue" data-id="' + full.Id + '" href="' + integralBill + '" title="积分明细"><i class="icon-star bigger-130"></i></a>';

			        render += '</div>';
			        return render;
			    }
			}
		]
    });

    //查询
    $('#QueryButton').on("click", function () {
        userName = $("#UserName").val();
        nickName = $("#NickName").val();
        sex = $("#Sex").val();
        createTimeBegin = $("#CreateTimeBegin").val();
        createTimeEnd = $("#CreateTimeEnd").val();
        goodsTable.api().ajax.reload();
    });

    $('#MembersTable').on("click", ".switch", function (e) {
        var id = $(this).data("id");
        var val = $(this).data("value");
        bntToolkit.post(url_switchMember, { MemberId: id, Enabled: val == "off" }, function (result) {
            if (result.Success) {
                $("#MembersTable").dataTable().fnDraw();
            } else {
                bntToolkit.error(result.ErrorMessage);
            }
        });
    });

    $('#MembersTable').on("click", ".delete", function (e) {
        var id = $(this).data("id");

        bntToolkit.confirm("删除后不可恢复，确定还要删除该会员吗？", function () {
            bntToolkit.post(url_deleteMember, { memberId: id }, function (result) {
                if (result.Success) {
                    $("#MembersTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });
});