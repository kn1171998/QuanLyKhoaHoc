$(function () {
    CategoryToCourseController.init();
});
var CategoryToCourseController = {
    init: function () {
        CategoryToCourseController.loadData();
        CategoryToCourseController.registerEvent();
    },
    registerEvent: function () {
        $('body').on('click', '#idbtnSearch', function () {
            var radiofree = $('input[name=radiofree]:checked').val();
            var sortPrice = $('#sortPrice').val();
            CategoryToCourseController.loadData(radiofree, sortPrice);
        });
    },
    loadData: function (radiofree, sortPrice) {
  
        var idcategoryhidden = $('#idcategoryhidden').val();
        $.ajax({
            url: '/CourseCategory/Sort',
            type: 'GET',
            data: {

                id: idcategoryhidden,
                sortPrice: sortPrice,
                radiofree: radiofree
                //page: common.pageIndex,
                //pageSize: common.pageSize
            },
            dataType: 'json',
            success: function (res) {
                var data = res.data;
                var html = '';
                var template = $('#course-item').html();
                if (res.status) {
                    var data = res.data;
                    var formatter = new Intl.NumberFormat('en', {
                        notation: 'standard'
              
                    });
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.id,
                            nameCourse: item.name,
                            Description: item.description,
                            image: item.image,
                            nameAuthor: item.fullName,
                            PromotionPrice: item.promotionPrice,
                            Price: formatter.format(item.price)
                        });
                    });
                    $('#addcate').html(html);
                    //common.paging(res.total, function () {
                    //    CategoryToCourseController.loadData();
                    //}, changePageSize);
                }
            }
        });
    }
}