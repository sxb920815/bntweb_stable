jQuery(function ($) {
    $('#ChannelsTable').dataTable({
        "processing": true,
        "serverSide": true,
        "sorting": [[0, "desc"]],
        "ajax": url_loadPage,
        "aoColumns":
		[
			{
			    "mData": "Name",
			    'sClass': 'left',
			    "mRender": function (data, type, full) {
			        return "<a href=\"#\">" + data + "</a>";
			    }
			},
			{
			    "mData": "UsersCount", 'sClass': 'left',
			    "sWidth": "180px",
			    "mRender": function (data, type, full) {
			        return data;
			    }
			},
			{
			    "mData": "CreateTime", 'sClass': 'left',
			    "sWidth": "180px",
			    "mRender": function (data, type, full) {
			        if (data != null && data.length > 0) {
			            return eval('new ' + data.replace(/\//g, '')).Format("yyyy-MM-dd hh:mm:ss");
			        }
			        return "";
			    }
			},
			{
			    "mData": "Id",
			    'sClass': 'center',
			    "sWidth": "150px",
			    "orderable": false,
			    "mRender": function (data, type, full) {
			        var render = '<div class="visible-md visible-lg hidden-sm hidden-xs action-buttons">';

			        render += '<a class="blue" href="' + url_edit + '?id=' + full.Id + '" title="编辑"><i class="icon-pencil bigger-130"></i></a>';

			        render += '<a class="red delete" data-id="' + full.Id + '" href="#" title="删除"><i class="icon-trash bigger-130"></i></a>';
			        render += '</div>';
			        return render;
			    }
			}
		]
    });

    $('#ChannelsTable').on("click", ".delete", function (e) {
        var id = $(this).data("id");

        bntToolkit.confirm("删除后不可恢复，确定还要删除该会员吗？", function () {
            bntToolkit.post(url_delete, { id: id }, function (result) {
                if (result.Success) {
                    $("#ChannelsTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });
});