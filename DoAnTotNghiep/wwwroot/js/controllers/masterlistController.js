var masterlistController = {
    init: function () {
        masterlistController.registerEvent();
    },
    registerEvent: function () {
        masterlistController.loadData();
    },
    loadData: function () {
        var _self = $(this);        
        $.ajax({
            url: _self.attr('action'),
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                var data = res.data;
                var html = '';
                var template = $('#data-template').html();
                if (res.status) {
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.MasterListCode,
                            Name
                        })
                    });
                }
            }
        });
    }
}
masterlistController.init();