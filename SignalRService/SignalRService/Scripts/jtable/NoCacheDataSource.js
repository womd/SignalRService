 jtable_datasource = {
    //add nocache token to urls to prevent ngnix caching
    list: function (postData, jtParams, controller, action) {
        return $.Deferred(function ($dfd) {
            $.ajax({
                url: '/' + controller + '/' + action + '?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting + '&nocachetok=' + jtable_datasource.makeid(),
                type: 'POST',
                dataType: 'json',
                data: postData,
                success: function (data) {
                    $dfd.resolve(data);
                },
                error: function () {
                    $dfd.reject();
                }
            });
        });
    },
    create: function (postData, controller, action) {
        return $.Deferred(function ($dfd) {
            $.ajax({
                url: '/' + controller + '/' + action + '?nocachetok=' + jtable_datasource.makeid(),
                type: 'POST',
                dataType: 'json',
                data: postData,
                success: function (data) {
                    $dfd.resolve(data);
                },
                error: function () {
                    $dfd.reject();
                }
            });
        });
    },
    update: function (postData, controller, action) {
        return $.Deferred(function ($dfd) {
            $.ajax({
                url: '/' + controller + '/' + action + '?nocachetok=' + jtable_datasource.makeid(),
                type: 'POST',
                dataType: 'json',
                data: postData,
                success: function (data) {
                    $dfd.resolve(data);
                },
                error: function () {
                    $dfd.reject();
                }
            });
        });
    },
    delete: function (postData, controller, action) {
        return $.Deferred(function ($dfd) {
            $.ajax({
                url: '/' + controller + '/' + action + '?nocachetok=' + jtable_datasource.makeid(),
                type: 'POST',
                dataType: 'json',
                data: postData,
                success: function (data) {
                    $dfd.resolve(data);
                },
                error: function () {
                    $dfd.reject();
                }
            });
        });
    },
    makeid: function () {
        var text = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (var i = 0; i < 5; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));

        return text;
    }

}