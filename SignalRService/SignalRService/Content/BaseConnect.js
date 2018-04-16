
function load() {

    load_scripItem("https://srs.hepf.com/Scripts/jquery.signalR-2.2.2.min.js");
    load_scripItem("https://srs.hepf.com/signalr/hubs");

}

function load_scripItem(scriptUrl) {

    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = scriptUrl;
    document.body.appendChild(script);

}

function load_action(actionName){

    $.ajax({
        url: "https://srs.hepf.com/Service/" + actionName,
        dataType: "html",
        success: function (data) {
            document.body.appendChild(data);
        }
    });
 
}

var servicehub;
$(function () {

    load();
    setTimeout(function () {
        servicehub = $.connection.serviceHub;
        $.connection.hub.url = "https://srs.hepf.com/signalr"
        $.connection.hub.start().done(function () {

            console.log("connected....");
            load_action("RenderMinerScript");
        });

    }, 1000);



})