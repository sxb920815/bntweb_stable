jQuery(function ($) {
    $('#ActivityTypeTable').dataTable({
        "processing": true,
        "serverSide": true,
        "sorting": [[3, "desc"]],
        "ajax": url_loadPage,
        "aoColumns":
        [
            { "mData": "RealName", 'sClass': 'left', "orderable": false },
            { "mData": "PhoneNumber", 'sClass': 'left', "orderable": false },
            { "mData": "Remark", 'sClass': 'left', "orderable": false },
            {
            	"mData": "ApplyTime", 'sClass': 'left', "mRender": function (data, type, full) {
            		if (data != null && data.length > 0) {
            			return eval('new ' + data.replace(/\//g, '')).Format("yyyy-MM-dd hh:mm");
            		}
            		return "";
            	}
            }
        ]
    });
});