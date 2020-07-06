$(function () {
    courseController.init();
});
var courseController = {
    init: function () {
        courseController.loadData();
        courseController.registerEvent();
    },
    registerEvent: function () {       
        $('body').on('submit', '#frmUploadVideo', courseController.uploadWareHouse);
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
        $('body').on('keypress','#SearchCourse', function (e) {
            if (e.which == 13) {                
                courseController.loadData(true);
            }
        });
        $('body').on('click', '.btnDelete', courseController.deleteData);
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
                if (res.status) {
                    var data = res.data;
                    var formatter = new Intl.NumberFormat('en', {
                        notation: 'standard',
                        //style: 'standard',
                        //currency: 'VND',                        
                        //maximumFractionDigits:0
                    });
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.id,
                            Name: item.name,
                            Description: item.description,
                            Image: item.image,
                            Price: formatter.format(item.price)
                        });
                    });
                    $('#tblCourse').html(html);                   
                    common.paging(res.total, function () {
                        courseController.loadData();
                    }, changePageSize);
                }
            }
        });
    },
    uploadWareHouse: function () {
        var _self = $(this);
        var _selfForm = new FormData();
        if (document.getElementById("VideoPath").files != null) {
            var totalFiles = document.getElementById("VideoPath").files.length;
            if (totalFiles <= 0) {
                alert('Ảnh chưa được chọn. Vui lòng chọn ảnh trước khi lưu.');
                return false;
            }
        }
        _selfForm.append("VideoPath", $('#VideoPath')[0].files[0]);
        $.ajax({
            url: "/Course/UploadVideo",
            type: 'POST',
            data: _selfForm,
            contentType: false,
            processData: false,
            success: function (res) {
                if (res) {
                    bootbox.alert("Thành công");
                } else {
                    bootbox.alert("Thất bại");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                // When AJAX call has failed
                console.log('AJAX call failed.');
                console.log(textStatus + ': ' + errorThrown);
            }
        });
        return false;
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
                        common.showNotify("Xoá thành công", res ? "success" : "error");
                        courseController.loadData();
                    }
                });
            }
        });
    }
}