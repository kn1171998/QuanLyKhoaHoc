﻿$(function () {
    courselessonController.init();
});
var courselessonController = {
    init: function () {
        courselessonController.loadData();
        courselessonController.registerEvent();
    },
    registerEvent: function () {
        $('body').on('click', '.btnDelete', courselessonController.deleteData);
        $('body').on('submit', '#frmCreateChapter', courselessonController.createChapter);
        $('body').on('submit', '#frmCreateCourseLesson', courselessonController.createCourseLesson);
    },
    initTreeData: function () {
        $("#divTreeLesson").fancytree({
            icons: false
        });
    },
    loadData: function () {
        var courseid = $('input:hidden[name=Id]').val();
        $.ajax({
            url: '/CourseLesson/_Index',
            type: 'GET',
            data: {
                ID: courseid
            },
            dataType: 'json',
            success: function (res) {
                var data = res;
                var template = $('#ChapterLesson').html();
                var templateCourseLesson = $('#CourseLesson').html();
                if (res.status) {
                    var lstChapter = data.lstChapter;
                    Mustache.parse(template);
                    Mustache.parse(templateCourseLesson);
                    var lessonVMs = data.lessonVMs;
                    $('#containerTree').html('');
                    $('#containerTree').append('<div id="divTreeLesson"><ul></ul></div>');
                    $.each(lstChapter, function (i, item) {
                        var htmlChapter = Mustache.render(template, {
                            ID: item.id,
                            Name: item.nameChapter,
                            CourseId: item.courseId,
                            OrderChapter: item.orderChapter
                        });
                        $('#divTreeLesson > ul').append(htmlChapter);
                        var lstLesson = lessonVMs[item.id];
                        if (lstLesson.length > 0) {
                            $.each(lstLesson, function (i, item1) {
                                var htmlLesson = Mustache.render(templateCourseLesson, {
                                    ID: item1.id,
                                    Name: item1.name,
                                    ChapterId: item1.chapterId,
                                    SortOrder: item1.sortOrder
                                });
                                var nameChapterID = '#' + item.id;
                                $(nameChapterID).append(htmlLesson);
                            });
                        }
                    });
                }
                courselessonController.initTreeData();
            }
        });
    },
    deleteData: function () {
        var _self = $(this);
        var _ID = $(this).attr('data-id');
        bootbox.confirm("Bạn có chắc muốn xoá không?", function (result) {
            if (result) {
                $.ajax({
                    url: _self.attr('action'),
                    data: { ID: _ID },
                    type: 'POST',
                    success: function (res) {
                        common.showNotify("Xoá thành công", res ? "success" : "error");
                        courselessonController.loadData();
                    }
                });
            }
        });
    },
    deleteChap: function () {
        var _self = $(this);
        var _ID = $(this).attr('data-id');
        bootbox.confirm("Bạn có chắc muốn xoá không?", function (result) {
            if (result) {
                $.ajax({
                    url: _self.attr('action'),
                    data: { ID: _ID },
                    type: 'POST',
                    success: function (res) {
                        common.showNotify("Xoá thành công", res ? "success" : "error");
                        courselessonController.loadData();
                    }
                });
            }
        });
    },
    createChapter: function () {
        var _self = $(this);
        var _selfForm = new FormData();
        var arr = _self.serializeArray();
        $.each(arr, function (i, field) {
            _selfForm.append(field.name, field.value);
        });
        $.ajax({
            url: _self.attr('action'),
            data: _selfForm,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (data) {
                common.showNotify("Thành công", data.status ? "success" : "error");
                setTimeout(function () {
                    window.location.reload();
                }, 500);
            }
        });
        return false;
    },
    createCourseLesson: function () {
        var _ID = $('#frmCreateCourseLesson > input[name=Id]').val();
        var _self = $(this);
        var _selfForm = new FormData();
        var validDocument = 0;
        if (document.getElementById("Video").files != null) {
            var totalFiles = document.getElementById("Video").files.length;
            if (totalFiles > 0 && _ID === "0") {
                validDocument += 1;
            }
            for (var i = 0; i < totalFiles; i++) {
                var file = document.getElementById("Video").files[i];
                _selfForm.append("Video", file);
            }
        }
        if (document.getElementById("Slide").files != null) {
            var totalFiles = document.getElementById("Slide").files.length;
            if (totalFiles > 0 && _ID === "0") {
                validDocument += 1;
            }
            for (var i = 0; i < totalFiles; i++) {
                var file = document.getElementById("Slide").files[i];
                _selfForm.append("Slide", file);
            }
        }
        if (validDocument == 0 && _ID === "0") {
            alert('Vui lòng chọn tài liệu khi lưu.');
            return false;
        }
        var arr = _self.serializeArray();
        $.each(arr, function (i, field) {
            _selfForm.append(field.name, field.value);
        });

        var IDProgress = $('input[name=ChapterId]').val() + $('input[name=SortOrder]').val();
        var template = $('#progressLesson').html();
        Mustache.parse(template);
        var pro = Mustache.render(template, { ID: IDProgress });
        $('#progress').append(pro);
        var nameProgess = '#progress' + IDProgress;
        var bar = $(nameProgess);
        var percent = $(nameProgess);
        $('.btnClose').trigger('click');
        $.ajax({
            url: _self.attr('action'),
            data: _selfForm,
            type: 'post',
            contentType: false,
            processData: false,
            async: true,
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                xhr.upload.addEventListener("progress",
                    function (evt) {
                        if (evt.lengthComputable) {
                            var progress = Math.round((evt.loaded / evt.total) * 100);
                            var percentValue = progress + '%';
                            bar.width(percentValue);
                            percent.html(percentValue);
                        }
                    },
                    false);
                return xhr;
            }
        }).done(function (data, textStatus, jqXhr) {
            var nameParentProgess = '#progressParent' + IDProgress;
            $(nameParentProgess).remove();
            if (data.status)
                common.showNotify("Thành công", "success");
            else
                common.showNotify("Thất bại", "error");
            courselessonController.loadData();
        });
        return false;
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    