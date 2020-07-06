$(function () {
    DiscountController.init();
});
var DiscountController = {
    init: function () {
        DiscountController.loadData();
        DiscountController.registerEvent();
    },
    registerEvent: function () {
        //    $('body').on('change', '#CategoryParent', DiscountController.loadCategoryChild);
        $('body').on('change', 'input[name=CheckTypeDiscount]', DiscountController.inputRadioDiscount);
        //$('body').on('input', '#Price', DiscountController.formatPrice)
        //    .on('keypress', '#Price', function (e) {
        //        if (!$.isNumeric(String.fromCharCode(e.which))) e.preventDefault();
        //    })
        //    .on('paste', '#Price', function (e) {
        //        var cb = e.originalEvent.clipboardData || window.clipboardData;
        //        if (!$.isNumeric(cb.getData('text'))) e.preventDefault();
        //    });
        $('body').on('input', '#DiscountAmount', DiscountController.formatPrice)
            .on('keypress', '#DiscountAmount', function (e) {
                if (!$.isNumeric(String.fromCharCode(e.which))) e.preventDefault();
            })
            .on('paste', '#DiscountAmount', function (e) {
                var cb = e.originalEvent.clipboardData || window.clipboardData;
                if (!$.isNumeric(cb.getData('text'))) e.preventDefault();
            });
        $('body').on('click', '#btnSearch', function () {
            DiscountController.loadData(true);
        });
        $('body').on('keypress', '#SearchName', function (e) {
            if (e.which == 13) {
                DiscountController.loadData(true);
            }
        });
        $('body').on('click', '.btnDelete', DiscountController.deleteData);
    },
    inputRadioDiscount: function () {
        var template = $('#inputDiscount').html();
        var valueRadio = $('input[name=CheckTypeDiscount]:checked').val();
        var idInput = {
            ID: "DiscountPercent",
            Min: "0",
            Max: "100"
        }
        if (valueRadio == 0)//phan tram chiet khau
        {
            idInput.ID = "DiscountPercent";
            idInput.Min = "0";
            idInput.Max = "100";
        }
        else if (valueRadio == 1)//so tien chiet khau
        {
            idInput.ID = "DiscountAmount";
            idInput.Min = "0";
            idInput.Max = "";
        }
        var html = Mustache.render(template, idInput);
        $('#DivInputDiscount').html(html);
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
        var searchDiscount = $('#SearchName').val();
        $.ajax({
            url: '/Discount/_Index',
            type: 'GET',
            data: {
                searchDiscount: searchDiscount,
                page: common.pageIndex,
                pageSize: common.pageSize
            },
            dataType: 'json',
            success: function (res) {
                var data = res.data;
                var html = '';
                var template = $('#discount-template').html();
                if (res.status) {
                    var data = res.data;
                    console.log(data);
                    var formatter = new Intl.NumberFormat('en', {
                        notation: 'standard',
                        //style: 'standard',
                        //currency: 'VND',                        
                        //maximumFractionDigits:0
                    });

                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.id,
                            CodeDiscount: item.codeDiscount,
                            FromDate: item.fromDate,
                            ToDate: item.toDate,
                            DiscountPercent: formatter.format(item.discountPercent),
                            DiscountAmount: formatter.format(item.discountAmount),
                            CourseName: "test"
                        });
                    });
                    $('#tblDiscount').html(html);
                    common.paging(res.total, function () {
                        DiscountController.loadData();
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
                        DiscountController.loadData();
                    }
                });
            }
        });
    },
    loadCourse: function () {
        $.ajax({
            url: '/Discount/ListCourse',
            type: 'GET',
            data: {      
                
            },
            dataType: 'json',
            success: function (res) {
            }
        });
    }
}