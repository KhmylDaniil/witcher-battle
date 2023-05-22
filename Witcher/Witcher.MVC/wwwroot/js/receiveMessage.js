"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/messageHub").build();

connection.on("ReceiveMessage", function () {

    setTimeout(function () {
        document.location.reload();
    }, 1000);
    
});

connection.start();
