﻿$(function () {
    homeController.init();
});

var homeController = {
    init: function () {
        homeController.loadData();
        homeController.registerEvent();
    },
    registerEvent: function () {
        $('body').on('click', '#btn-search', function () {
            var search = $('#search').val();
            homeController.Search(search);
        });
    },
    loadData: function () {
        var listCate = [];
        $.ajax({
            url: "/Home/ListCategory",
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                var data = res;
                var parentCategory = data.parentCategory;
                var childCategory = data.listChild;
                var template = $('#dropdownCategory').html();
                var templateSub = $('#dropdownSubCategory').html();
                var templateButtonTopCategory = $('#buttonTopCourse').html();
                var templateDivTopCategory = $('#divTopCourse').html();
                Mustache.parse(template);
                Mustache.parse(templateSub);
                Mustache.parse(templateButtonTopCategory);
                Mustache.parse(templateDivTopCategory);
                if (res.status) {
                    $.each(parentCategory, function (i, item) {
                        var html = '';
                        var lengthChild = childCategory[item.id].length;
                        if (lengthChild > 0) {
                            html = Mustache.render(template, {
                                ID: item.id,
                                Name: item.name,
                                Child: "dropdown-submenu",
                                test: "test"
                            });
                        }
                        else {
                            html = Mustache.render(template, {
                                ID: item.id,
                                Name: item.name,
                                Child: "",
                                test: ""
                            });
                        }
                        $('#menuCategory').append(html);
                        var listChild = childCategory[item.id];
                        if (lengthChild > 0) {
                            $.each(listChild, function (i, item1) {
                                var htmlChild = Mustache.render(templateSub, {
                                    ID: item1.id,
                                    Name: item1.name
                                });
                                var nameParent = '#dropdown-menu' + item.id;
                                $(nameParent).append(htmlChild);
                            });
                        }
                    });
                    var categoryTop = '<button class="tablinks myTadBtn active" onclick="openCity(event, \'All\')">All</button>';
                    $('#allCourse').append(categoryTop);
                    $.each(parentCategory, function (i, item) {
                        if (i < 5) {
                            categoryTop = Mustache.render(templateButtonTopCategory, {
                                ID: item.id,
                                TopCateID: "category" + item.id,
                                Name: item.name
                            });
                            $('#allCourse').append(categoryTop);
                            categoryDivTop = Mustache.render(templateDivTopCategory, {
                                ID: item.id,
                                TopCateID: "category" + item.id,
                                Name: item.name
                            });
                            $(categoryDivTop).insertAfter('#All');
                        }
                    });

                }
            }
        });
    },
    Search: function (search) {
        window.location.href = "/CourseCategory/search_home?search=" + search;
    }
}

