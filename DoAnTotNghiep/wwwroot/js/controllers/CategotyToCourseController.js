$(function () {
    CategoryToCourseController.init();
});
var CategoryToCourseController = {
    init: function () {
        //CategoryToCourseController.loadData();
        CategoryToCourseController.loadDataC();
        CategoryToCourseController.registerEvent();
    },
    registerEvent: function () {
        var idcategoryhidden = $('#idcategoryhidden').val() != 'undefined' ? $('#idcategoryhidden').val() : 3;
        $('body').on('click', '#idbtnSearch', function () {

            var search = $('#search').val() != undefined ? $('#search').val() : "";
            var radiofree = $('input[id=radiofree]:checked').val() != undefined ? $('input[id=radiofree]:checked').val() : 2;
            var sortPrice = $('#sortPrice').val() != undefined ? $('#sortPrice').val() : 2;
            CategoryToCourseController.loadData(idcategoryhidden, radiofree, sortPrice, search);
        });
        $('body').on('click', '#btn-search', function () {
            var search = $('#search').val();
            var radiofree = 2;
            var sortPrice = 2;
            var idcategoryhidden = 0;
            CategoryToCourseController.loadData(idcategoryhidden, radiofree, sortPrice, search);
        });
    },
    loadData: function (idcategoryhidden, radiofree, sortPrice, search) {
           //load component card category 
        $.ajax({
            url: '/CourseCategory/Sort',
            type: 'GET',
            data: {
                idcategoryhidden: idcategoryhidden,
                id: idcategoryhidden,
                sortPrice: sortPrice,
                radiofree: radiofree,
                search: search
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

           //load component category left
        $.ajax({
            url: '/CourseCategory/GetChildCategories',
            type: 'GET',
            data: {
                id: idcategoryhidden
            },
            dataType: 'json',
            success: function (res) {
                var data = res.child;
                var html = '';
                var template = $('#dropdownSubCategory').html();
                if (res.status) {
                    var data = res.child;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.id,
                            Name: item.name
                        });
                    });
                    $('#idnamecourse').html(html);
                    //common.paging(res.total, function () {
                    //    CategoryToCourseController.loadData();
                    //}, changePageSize);
                }

            }
        });
    },

    loadDataC: function () {
        var idcategoryhidden = $('#idcategoryhidden').val() != 'undefined' ? $('#idcategoryhidden').val() : 3;
        //load component category left
        $.ajax({
            url: '/CourseCategory/GetChildCategories',
            type: 'GET',
            data: {
                id: idcategoryhidden
            },
            dataType: 'json',
            success: function (res) {
                var data = res.child;
                var html = '';
                var template = $('#dropdownSubCategory').html();
                if (res.status) {
                    var data = res.child;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.id,
                            Name: item.name
                        });
                    });
                    $('#idnamecourse').html(html);
                    //common.paging(res.total, function () {
                    //    CategoryToCourseController.loadData();
                    //}, changePageSize);
                }

            }
        });
    }
}


