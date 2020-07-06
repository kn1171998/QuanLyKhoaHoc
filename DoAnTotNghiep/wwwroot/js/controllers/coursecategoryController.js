$(function () {
    coursecategoryController.init();
});

var coursecategoryController = {
    init: function () {
        coursecategoryController.loadData();
        coursecategoryController.registerEvent();
    },
    registerEvent: function () {       
        $('body').on('click', '.btnDelete', coursecategoryController.deleteData);
        $('body').on('click', '#btnSearch', function () {
            coursecategoryController.loadData(true);
        });
        $('body').on('keypress', '#SearchName', function (e) {
            if (e.which == 13) {
                coursecategoryController.loadData(true);
            }
        });
    },
    loadData: function (changePageSize) {
        var searchName = $('#SearchName').val();
        $.ajax({
            url: "/CourseCategory/_Index",
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
                            Name: item.name,
                            SortOrder: item.sortOrder,
                            Status: item.status == true ? "Hiển thị" : "Ẩn"
                        });
                    });
                    $('#tblCategories').html(html);
                    common.paging(res.total, function () {
                        coursecategoryController.loadData();
                    }, changePageSize);
                }
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
                        coursecategoryController.loadData();
                    }
                });
            }
        });
    }
}
