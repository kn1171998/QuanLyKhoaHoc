$(function () {
    DiscountController.init();
});
var DiscountController = {
    init: function () {
        DiscountController.loadData();
        DiscountController.registerEvent();
    },
    registerEvent: function () {
        $('body').on('change', 'input[name=CheckTypeDiscount]', DiscountController.inputRadioDiscount);
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
        Mustache.parse(template);
        var valueRadio = $('input[name=CheckTypeDiscount]:checked').val();
        var idInput = {
            ID: "DiscountPercent",
            Min: "1000",
            Max: ""
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
            idInput.Min = "1000";
            idInput.Max = "";
        }
        var html = Mustache.render(template, idInput);
        $('#DivInputDiscount').html(html);
    },
    formatPrice: function () {
        var valueFormat = common.formatCurrency(this.value.replace(/[,VNĐ]/g, ''));
        $('#priceFormat').text(valueFormat);
    },
    formatPromotionPrice: function () {
        var valueFormat = common.formatCurrency(this.value.replace(/[,VNĐ]/g, ''));
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
                    Mustache.parse(template);
                    var data = res.data;
                    var formatter = new Intl.NumberFormat('en', {
                        notation: 'standard'             
                    });
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.id,
                            CodeDiscount: item.codeDiscount,
                            FromDate: common.formatDDMMYYYYHHMM(item.fromDate),
                            ToDate: common.formatDDMMYYYYHHMM(item.toDate),
                            DiscountPercent: formatter.format(item.discountPercent),
                            DiscountAmount: formatter.format(item.discountAmount)
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
    }  
}