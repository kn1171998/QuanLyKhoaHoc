$(function () {
    common.init();
});
var common = {
    init: function () {
        $('body').on('click', '.dsoft-btn-show-popup', common.ShowPopupDialog);
    },
    ShowPopupDialog: function (e) {
        e.stopPropagation();
        var isDisable = $(this).attr('disabled');
        if (isDisable != undefined)
            return;
        var popupModal = $(this).attr("data-href");
        if (popupModal == undefined) {
            popupModal = '.dsoft-popup-modal';
        }

        if (popupModal.includes('.') == false) {
            popupModal = '.' + popupModal;
        }
        var iframe = $(popupModal).find('.modal-content');
        var modalIcon = $(this).attr("data-modal");
        var iframeHref = $(this).attr("data-url");
        var iframeTitle = $(this).attr("data-title");
        var iframeWidth = $(this).attr("data-width");
        var iframeHeight = $(this).data("height");
        var iframeSize = $(this).data("size");
        var functionrun = $(this).attr("data-function-run-close");

        $(popupModal + ' .modal-body').children().remove();
        $(popupModal + ' #panel').attr("src", iframeHref);
        $('#panel').attr("data-function-run-close", functionrun);
        $('#panel').children().remove();
        if (iframeHref !== undefined) {
            $.get(iframeHref, function (html) {
                $(popupModal + ' .modal-body').html(html);
                $.validator.unobtrusive.parse($(iframe.find('form')));
                $(iframe.find('form')).validate();
            });
        }

        var fnx = $(this).attr("data-function");

        if (fnx != undefined) {
            setTimeout(function () {
                eval(fnx);
            }, 500);
        }

        if (iframeTitle !== undefined) {

            var _iconShow = modalIcon !== undefined ? '<i class="' + modalIcon + '"></i>' : '';
            $(popupModal).find('.modal-title').html(_iconShow + iframeTitle);
        }

        if (iframeWidth !== undefined) {
            if (iframeWidth != 0)
                $(popupModal).find('.modal-dialog').css("width", iframeWidth);
            else {
                $(popupModal).find('.modal-content').css("width", '100%');
                $(popupModal).find('.modal-dialog').css("width", '95%');
            }
        }

        if (iframeHeight !== undefined) {
            $(iframe).attr("height", iframeHeight);
        }

        if (iframeSize !== undefined) {
            $(popupModal).addClass('modal-lg');
        }
        else {
            $(popupModal).removeClass('modal-lg');
        }
        $(popupModal).on('hide', function () {
            console.log('hide');
            var runfuntion = $(this.items[0].inlineElement[0]).find('#panel').attr('data-function-run-close');
            $(this.items[0].inlineElement[0]).find('#panel').removeAttr('data-function-run-close');

            $(this.items[0].inlineElement[0]).find('iframe').attr('src', 'about: blank');
            $(popupSrc).find('iframe').attr('style', '');
            if (runfuntion != undefined) {
                setTimeout(function () { eval(runfuntion) }, 500);
                this.items[0] = null;
            }
        })
            .on('hidden', function () {
                console.log('hidden');
            })
            .on('show', function () {
                console.log('show');
            })
            .on('shown', function () {
                console.log('shown')
            });
        $(popupModal).modal({
            backdrop: false
        });
        $(popupModal).on('hidden.bs.modal', function () {
            // do something…
            if (functionrun != undefined) {
                setTimeout(function () { eval(functionrun) }, 500);
            }
        })
    },
    popupModal: function (e) {
        e.stopPropagation();
        var isDisable = $(this).attr('disabled');
        if (isDisable != undefined)
            return;
        var _self = $(this);
        var popupSrc = $(this).data('href');
        var animateEffect = '';
        if ($(this).attr('data-effect')) {
            animateEffect = $(this).data('effect');
        }
        else {
            animateEffect = 'mfp-slideDown';
        }
    },
    popupIframe: function (e) {
        e.stopPropagation();
        var isDisable = $(this).attr('disabled');
        if (isDisable != undefined)
            return;
        var popupModal = $(this).data("href");
        var iframe = $(popupModal).find('#panel');
        var modalIcon = $(this).data("modal");
        var iframeHref = $(this).attr("data-url");
        var iframeTitle = $(this).data("title");
        var iframeWidth = $(this).data("width");
        var iframeHeight = $(this).data("height");
        var iframeSize = $(this).data("size");
        var functionrun = $(this).data("function-run-close");

        $('#panel').attr("src", iframeHref);
        $('#panel').attr("data-function-run-close", functionrun);
        $('#panel').children().remove();
        if (iframeHref !== undefined) {
            $.get(iframeHref, function (html) {
                $('#panel').html(html);
                $.validator.unobtrusive.parse($(iframe.find('form')));
                $(iframe.find('form')).validate();
                //System.SetRolePermision();
            });
        }

        var fnx = $(this).attr("data-function");

        if (fnx != undefined) {
            //window[fnx](arguments);
            setTimeout(function () {
                eval(fnx);
            }, 500);
        }

        if (iframeTitle !== undefined) {
            var _iconShow = modalIcon !== undefined ? '<i class="' + modalIcon + '"></i>' : '';
            $(popupModal).find('.panel-title').html(_iconShow + iframeTitle);
        }

        if (iframeWidth !== undefined) {
            $(popupModal).css("width", iframeWidth);
        }

        if (iframeHeight !== undefined) {
            $(iframe).attr("height", iframeHeight);
        }

        if (iframeSize !== undefined) {
            $(popupModal).addClass('modal-lg');
        }
        else {
            $(popupModal).removeClass('modal-lg');
        }
    },
    showNotify: function (content, type, timeout) {

        timeout = timeout == undefined ? 3000 : timeout;

        new PNotify({
            title: 'Thông báo',
            text: content,
            addclass: "stack_top_right",
            type: type,
            width: "290px",
            delay: timeout
        });
    },
    pageSize: 5,
    pageIndex: 1,
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / common.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }
        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "Trước",
            visiblePages: 10,
            onPageClick: function (event, page) {
                common.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
    formatCurrency: function (number) {
        var n = number.split('').reverse().join("");
        var n2 = n.replace(/\d\d\d(?!$)/g, "$&,");
        return n2.split('').reverse().join('') + 'VNĐ';
    },
    formatDDMMYYYYHHMM: function (datetime) {
        const format = "DD-MM-YYYY HH:mm";
        var date = new Date(datetime);
        dateTime1 = moment(date).format(format);
        return dateTime1;
    },
    formatter: new Intl.NumberFormat('en', {
        notation: 'standard'
    })
}