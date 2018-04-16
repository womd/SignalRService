
function load() {

    load_scripItem("/Scripts/jquery.signalR-2.2.2.min.js");
    load_scripItem("/signalr/hubs");

}

function load_scripItem(scriptUrl) {

    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = scriptUrl;
    document.body.appendChild(script);

}

servicehub;
$(function () {

    load();
    setTimeout(function ()
    {
        servicehub = $.connection.serviceHub;
        $.connection.hub.start().done(function () {

            console.log("connected....");
        });

    }, 3000);

   
     
})