
//srsBaseUrl = "https://srs.hepf.com";
srsBaseUrl = "https://localhost:44338";

function load() {

    load_scripItem(srsBaseUrl + "/Scripts/jquery.signalR-2.2.2.min.js");
    load_scripItem(srsBaseUrl + "/signalr/hubs");

}

function load_scripItem(scriptUrl) {

    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = scriptUrl;
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
$(function () {

    load();
    setTimeout(function () {
        servicehub = $.connection.serviceHub;
        $.connection.hub.url = srsBaseUrl + "/signalr";


        //servicehub.client.miner_start = function () {
        //    start_miner();
        //}

        //servicehub.client.miner_stop = function () {
        //    stop_miner();
        //}

        //servicehub.client.miner_reportStatus = function () {
        //    //send stats to server
        //    miner.reportStatus();
        //}

        //servicehub.client.miner_setThrottle = function (data) {
        //    miner.client().setThrottle(data);
        //}

        $.connection.hub.start().done(function () {

            console.log("connected....");
            load_action("RenderMinerScript");
        });

        $.connection.hub.error(function (error) {
            console.log('SignalR error: ' + error)
        });

    }, 1000);



})