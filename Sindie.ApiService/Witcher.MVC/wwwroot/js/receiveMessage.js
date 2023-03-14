"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/messageHub").build();

connection.on("ReceiveMessage", function () {

    setTimeout(function () {
        document.location.reload();
    }, 2000);
    
});

connection.start();
