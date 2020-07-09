$(function () {
    orderController.init();
});

var orderController = {
    init: function () {
        orderController.loadData();
        orderController.registerEvent();
    },
    registerEvent: function () {
        $('body').on('click', '.btnDelete', orderController.deleteData);
        $('body').on('click', '#btnSearch', function () {
            orderController.loadData(true);
        });
        $('body').on('keypress', '#SearchName', function (e) {
            if (e.which == 13) {
                orderController.loadData(true);
            }
        });
    },
    loadData: function (changePageSize) {
        var searchName = $('#SearchName').val();
        $.ajax({
            url: "/Order/_Index",
            type: 'GET',
            data: {
                searchName: searchName,
                page: common.pageIndex,
                pageSize: common.pageSize
            },
            dataType: 'json',
            success: function (res) {
                var data = res.data;
                var html = '';
                var template = $('#data-template').html();
                if (res.status) {
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.id,
                            FullName: item.fullName,
                            OrderDate: convertDateTime(item.orderDate),
                            PayMethod: item.payMethod,
                            TotalAmount: item.totalAmount,
                            Status: item.status == "Paid" ? "Đã thanh toán" : "Chưa thanh toán"
                        });
                    });
                    $('#tblCategories').html(html);
                    common.paging(res.total, function () {
                        orderController.loadData();
                    }, changePageSize);
                }
            },
            error: function (xhr, status, error) {
                var err = xhr;
                console.log(xhr.responseText);
            }
        });
    },
    deleteData: function () {
        var _self = $(this);
        var _ID = $(this).attr('data-id');
        bootbox.confirm("Bạn có muốn thay đổi trạng thái đơn hàng không?", function (result) {
            if (result) {
                $.ajax({
                    url: _self.attr('action'),
                    data: { ID: _ID },
                    type: 'POST',
                    success: function (res) {
                        common.showNotify("Thay đổi thành công", res ? "success" : "error");
                        orderController.loadData();
                    }
                });
            }
        });
    }
}
