$(function () {
    usecategoryController.init();
});

var usecategoryController = {
    init: function () {
        usecategoryController.loadData();
        usecategoryController.registerEvent();
    },
    registerEvent: function () {
        $('body').on('click', '.btnDelete', usecategoryController.deleteData);
        $('body').on('click', '#btnSearch', function () {
            usecategoryController.loadData(true);
        });
        $('body').on('keypress', '#SearchName', function (e) {
            if (e.which == 13) {
                usecategoryController.loadData(true);
            }
        });
    },
    loadData: function (changePageSize) {
        var searchName = $('#SearchName').val();
        $.ajax({
            url: "/UserCategory/_Index",
            type: 'GET',
            data: {
                searchName: "",
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
                            Birthday: convertDate(item.birthday),
                            Sex: item.sex ? "Nam" : "Nữ",
                            Introduction: item.introduction,
                            Status: item.status == true ? "Đang mở" : "Đã khoá"
                        });
                    });
                    $('#tblCategories').html(html);
                    common.paging(res.total, function () {
                        usecategoryController.loadData();
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
        bootbox.confirm("Bạn có chắc muốn xoá không", function (result) {
            if (result) {
                $.ajax({
                    url: _self.attr('action'),
                    data: { ID: _ID },
                    type: 'POST',
                    success: function (res) {
                        common.showNotify("Xoá thành công", res ? "success" : "error");
                        usecategoryController.loadData();
                    }
                });
            }
        });
    }
}
