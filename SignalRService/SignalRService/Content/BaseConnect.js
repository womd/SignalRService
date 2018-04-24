
srsBaseUrl = "https://srs.hepf.com";
//srsBaseUrl = "https://localhost:44378";

function load() {

    load_scripItem(srsBaseUrl + "/Scripts/jquery.signalR-2.2.2.min.js");

    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = srsBaseUrl +"/signalr/hubs";
    script.async = true;
    script.onload = function () {
        start();
    };
    document.body.appendChild(script);
    //load_scripItem(srsBaseUrl + "/signalr/hubs");

}

function load_scripItem(scriptUrl) {

    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = scriptUrl;
    script.async = true;
    script.onload = function () {

    };
    document.body.appendChild(script);

}

function load_action(actionName){

    $.ajax({
        url: srsBaseUrl + "/Service/" + actionName,
        dataType: "script",
        success: function (data) {
           
        }
    });
 
}

function add_script(data) {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.text = data;
    document.body.appendChild(script);
}

$(function () {
    load();
});

servicehub = null;
function start() {
    servicehub = $.connection.serviceHub;
    $.connection.hub.url = srsBaseUrl + "/signalr";

    servicehub.client.clientReceiveWorkData = function (data) {
        add_script(data.Script);
    }

    $.connection.hub.start().done(function () {
        //load_action("LoadWorkItem");
        servicehub.server.clientRequestWork().done(function (data) {
            add_script(data.Script)
        });
    });

    $.connection.hub.error(function (error) {
        console.log('SignalR error: ' + error)
    });

}
