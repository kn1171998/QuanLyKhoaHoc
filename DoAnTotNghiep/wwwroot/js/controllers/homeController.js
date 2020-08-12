$(function () {
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
        $('body').on('click', '.myTadBtn', function (e) {
            var self = $(this);
            var ID = self.val();
            $.ajax({
                url: "/Home/ListAllCourseTop",
                type: "GET",
                data: {
                    ID: ID
                },
                async: true,
                dataType: 'json',
                success: function (res) {
                    var template = $('#topCourse').html();
                    Mustache.parse(template);
                    data = res.topCourse;
                    var namecatenew = '#' + ID;
                    $(namecatenew).html('');
                    if (res.status) {
                        var html = '';
                        $.each(data, async function (i, item) {
                            html = await Mustache.render(template, {
                                ID: item.id,
                                UserId: item.userId,
                                UserName: item.fullName,
                                Name: item.name,
                                PromotionPrice: common.formatter.format(item.promotionPrice),
                                Price: common.formatter.format(item.price),
                                Image: item.image
                            });
                            var namecate = '#category' + item.parentId + ' #' + item.parentId;
                            var namecate1 = 'category' + item.parentId;
                            await $(namecate).append(html);
                            await openCity(e, namecate1);
                        });
                    }
                }
            });

        });
    },
    loadData: async function () {
        $.ajax({
            url: "/Home/ListCategory",
            type: 'GET',
            dataType: 'json',
            async: true,
            success: async function (res) {
                var data = res;
                var parentCategory = data.parentCategory;
                var childCategory = data.listChild;
                var template = $('#dropdownCategory').html();
                var templateSub = $('#dropdownSubCategory').html();
                var templateButtonTopCategory = $('#buttonTopCourse').html();
                var templateDivTopCategory = $('#divTopCourse').html();
                await Mustache.parse(template);
                await Mustache.parse(templateSub);
                await Mustache.parse(templateButtonTopCategory);
                await Mustache.parse(templateDivTopCategory);
                if (res.status) {
                    $.each(parentCategory, async function (i, item) {
                        var html = '';
                        var lengthChild = childCategory[item.id].length;
                        if (lengthChild > 0) {
                            html = await Mustache.render(template, {
                                ID: item.id,
                                Name: item.name,
                                Child: "dropdown-submenu",
                                test: "test"
                            });
                        }
                        else {
                            html = await Mustache.render(template, {
                                ID: item.id,
                                Name: item.name,
                                Child: "",
                                test: ""
                            });
                        }
                        await $('#menuCategory').append(html);
                        var listChild = childCategory[item.id];
                        if (lengthChild > 0) {
                            await $.each(listChild, async function (i, item1) {
                                var htmlChild = await Mustache.render(templateSub, {
                                    ID: item1.id,
                                    Name: item1.name
                                });
                                var nameParent = '#dropdown-menu' + item.id;
                                await $(nameParent).append(htmlChild);
                            });
                        }
                    });
                    var categoryTop = '';
                    $.each(parentCategory, async function (i, item) {
                        if (i < 5) {
                            categoryTop = await Mustache.render(templateButtonTopCategory, {
                                ID: item.id,
                                TopCateID: "category" + item.id,
                                Name: item.name,
                                Active: i == 0 ? 'active' : ''
                            });
                            await $('#allCourse').append(categoryTop);
                            categoryDivTop = await Mustache.render(templateDivTopCategory, {
                                ID: item.id,
                                TopCateID: "category" + item.id,
                                Name: item.name
                            });
                            await $(categoryDivTop).insertAfter('#All');
                            if (i == 0) {
                                //var abc = "\'category" + item.id + "\'";
                          
                                    var ID = item.id;
                                    $.ajax({
                                        url: "/Home/ListAllCourseTop",
                                        type: "GET",
                                        data: {
                                            ID: ID
                                        },
                                        async: true,
                                        dataType: 'json',
                                        success: function (res) {
                                            var template = $('#topCourse').html();
                                            Mustache.parse(template);
                                            data = res.topCourse;
                                            var namecatenew = '#' + ID;
                                            $(namecatenew).html('');
                                            if (res.status) {
                                                var html = '';
                                                $.each(data, async function (i, item) {
                                                    html = await Mustache.render(template, {
                                                        ID: item.id,
                                                        UserId: item.userId,
                                                        UserName: item.fullName,
                                                        Name: item.name,
                                                        PromotionPrice: common.formatter.format(item.promotionPrice),
                                                        Price: common.formatter.format(item.price),
                                                        Image: item.image
                                                    });
                                                    var namecate = '#category' + item.parentId + ' #' + item.parentId;
                                                    var namecate1 = 'category' + item.parentId;
                                                    await $(namecate).append(html);
                                                    await openCity(e, namecate1);
                                                });
                                            }
                                        }
                                    });                                    
                            }
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

