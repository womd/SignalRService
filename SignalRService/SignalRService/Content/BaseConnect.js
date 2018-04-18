
srsBaseUrl = "https://srs.hepf.com";
//srsBaseUrl = "https://localhost:44338";

function load() {

    load_scripItem(srsBaseUrl + "/Scripts/jquery.signalR-2.2.2.min.js");

    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = "/signalr/hubs";
    script.async = true;
    script.onload = function () {
        start();
    };
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


servicehub = null;
function start() {
    servicehub = $.connection.serviceHub;
    $.connection.hub.url = srsBaseUrl + "/signalr";

    $.connection.hub.start().done(function () {

        console.log("connected....");
        load_action("RenderMinerScript");
    });

    $.connection.hub.error(function (error) {
        console.log('SignalR error: ' + error)
    });

}
