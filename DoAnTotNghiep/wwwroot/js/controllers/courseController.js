$(function () {
    courseController.init();
});
var courseController = {
    init: function () {
        courseController.loadData();
        courseController.registerEvent();
    },
    registerEvent: function () {
        $('body').on('change', '#CategoryParent', courseController.loadCategoryChild);
        $('body').on('input', '#Price', courseController.formatPrice)
            .on('keypress', '#Price', function (e) {
                if (!$.isNumeric(String.fromCharCode(e.which))) e.preventDefault();
            })
            .on('paste', '#Price', function (e) {
                var cb = e.originalEvent.clipboardData || window.clipboardData;
                if (!$.isNumeric(cb.getData('text'))) e.preventDefault();
            });
        $('body').on('input', '#PromotionPrice', courseController.formatPromotionPrice)
            .on('keypress', '#PromotionPrice', function (e) {
                if (!$.isNumeric(String.fromCharCode(e.which))) e.preventDefault();
            })
            .on('paste', '#PromotionPrice', function (e) {
                var cb = e.originalEvent.clipboardData || window.clipboardData;
                if (!$.isNumeric(cb.getData('text'))) e.preventDefault();
            });
        $('body').on('click', '#btnSearch', function () {
            courseController.loadData(true);
        });
        $('body').on('keypress', '#SearchCourse', function (e) {
            if (e.which == 13) {
                courseController.loadData(true);
            }
        });
        $('body').on('click', '.btnDelete', courseController.deleteData);
        $('body').on('click', '#btnExport', courseController.exportCourse);
    },
    formatPrice: function () {
        var valueFormat = common.formatCurrency(this.value.replace(/[,VNĐ]/g, ''));
        console.log(valueFormat);
        $('#priceFormat').text(valueFormat);
    },
    formatPromotionPrice: function () {
        var valueFormat = common.formatCurrency(this.value.replace(/[,VNĐ]/g, ''));
        console.log(valueFormat);
        $('#promotionPriceFormat').text(valueFormat);
    },
    loadData: function (changePageSize) {
        var searchCourse = $('#SearchCourse').val();
        $.ajax({
            url: '/Course/_Index',
            type: 'GET',
            data: {
                searchCourse: searchCourse,
                page: common.pageIndex,
                pageSize: common.pageSize
            },
            dataType: 'json',
            success: function (res) {
                var data = res.data;
                var html = '';
                var template = $('#data-template').html();
                var hasExported = $('#hasExported').html();
                Mustache.parse(template);
                if (res.status) {
                    var data = res.data;
                    var formatter = new Intl.NumberFormat('en', {
                        notation: 'standard',
                        //style: 'standard',
                        //currency: 'VND',                        
                        //maximumFractionDigits:0
                    });
                    $.each(data, function (i, item) {
                        if (item.hasOrder) {
                            html += Mustache.render(hasExported, {
                                ID: item.id,
                                Name: item.name,
                                Status: item.status ? "Đã xuất bản" : "Chưa xuất bản",
                                Image: item.image,
                                Price: formatter.format(item.price)
                            });
                        }
                        else {
                            html += Mustache.render(template, {
                                ID: item.id,
                                Name: item.name,
                                Status: item.status ? "Đã xuất bản" : "Chưa xuất bản",
                                Image: item.image,
                                Price: formatter.format(item.price)
                            });
                        }
                    });
                    $('#tblCourse').html(html);
                    common.paging(res.total, function () {
                        courseController.loadData();
                    }, changePageSize);
                }
            }
        });
    },
    loadCategoryChild: function () {
        var idParent = $('#CategoryParent').val();
        $.ajax({
            url: '/Course/GetChildCategories',
            type: 'GET',
            data: {
                parentId: idParent
            },
            dataType: 'json',
            success: function (res) {
                var data = res.child;
                var html = '';
                var template = $('#combobox').html();
                if (res.status) {
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.id,
                            Name: item.name
                        });
                    });
                    $('#CategoryId').html(html);
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
                        if (res)
                            common.showNotify("Xoá thành công", "success");
                        else
                            common.showNotify("Xoá thất bại", "error");
                        courseController.loadData();
                    }
                });
            }
        });
    },
    exportCourse: function () {
        var _self = $(this);
        var _ID = $(this).attr('data-id');
        bootbox.confirm("Bạn có muốn thay đổi trạng thái của khoá học không?", function (result) {
            if (result) {
                $.ajax({
                    url: _self.attr('action'),
                    data: { ID: _ID },
                    type: 'POST',
                    success: function (res) {
                        if (res.result == 1) {
                            common.showNotify("Thay đổi thành công", "success");
                        }
                        else if (res.result == 0) {
                            bootbox.alert(res.message)
                        }
                        else {
                            common.showNotify(res.message, "error");
                        }
                        courseController.loadData();
                    }
                });
            }
        });
    }
}