jQuery(function ($) {
    var options = $.datepicker.regional["zh-CN"];
    options["dateFormat"] = "yy/mm/dd";
    $("#BeginTime").datetimepicker(options);
    $("#EndTime").datetimepicker(options);

    var productTable = $('#singleGoodsTable').dataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": url_goodsList,
            "data": function (d) {
                d.extra_search = { "Name": name };
            }
        },
        "sorting": [[0, "desc"]],
        "aoColumns":
        [
            { "mData": "Name", 'sClass': 'left' },
           { "mData": "Specification", 'sClass': 'left' },
            {
                "mData": "Price", 'sClass': 'left'
            },
            { "mData": "Stock", 'sClass': 'left' },
            {
                "mData": "Enabled", 'sClass': 'center', "orderable": false,
                "mRender": function (data, type, full) {
                    var render = '<div class="visible-md visible-lg hidden-sm hidden-xs action-buttons">';
                    render += '<a class="blue" href="#" data-specification="' + full.Specification + '" data-price="' + full.Price + '" data-goodsid="' + full.GoodsId + '" data-id="' + full.Id + '" data-name="' + full.Name + '" title="选择" data-dismiss="modal"><i class="icon-ok bigger-130" title="选择"></i></a>';
                    render += '</div>';
                    return render;
                }
            }
        ]
    });

    //查询
    $('#QueryButton').on("click", function () {
        name = $("#Name").val();
        productTable.api().ajax.reload();
    });

    $('#singleGoodsTable').on("click", ".blue", function (e) {
        $("#SingleGoodsName").attr("value", $(this).data("name"));
        $("#SingleGoodId").attr("value", $(this).data("id"));
        $("#GoodsId").attr("value", $(this).data("goodsid"));
        $("#Price").attr("value", $(this).data("price"));
        $("#Specification").attr("value", $(this).data("specification"));

    });
});