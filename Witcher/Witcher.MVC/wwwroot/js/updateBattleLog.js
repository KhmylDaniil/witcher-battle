"use strict";

let connection = new signalR.HubConnectionBuilder().withUrl("/messageHub").build();

connection.on("UpdateBattleLog", function () {

    document.location.reload();
});

connection.start();
