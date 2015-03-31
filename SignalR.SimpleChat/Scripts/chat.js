/// <reference path="jquery-1.7.1-vsdoc.js" />
$(function () {
    var chatHubClient = $.connection.chatHub;

    // Send a new message to the server
    $("#SendMessage").click(function () {
        chatHubClient.sendMessage(RoomId, $('#textMessage').val());
        $('#textMessage').val("");
    });

    // Start the connection
    $.connection.hub.start(function () {
        chatHubClient.joinedEvent(RoomId);
    });

    // Receive a new message from the server
    chatHubClient.addMessage = function (msg) {
        //message = $("#chatWindow").html() + "<br />" + message;
        $("#chatWindow").append('<div class="alert alert-success" style="margin-bottom: 1px;"> [' + msg.Time + '] <b>' + msg.User + '</b> - ' + msg.Text + '</div>');
    };


    chatHubClient.userList = function (message) {
        //message = JSON.parse(message);
        //var options;
        $("#users").empty();
        $("#users").append('<ul class="nav nav-list" id="users"></ul>');

        $.each(message, function (index) {
            $("#users").append('<li><a href="#">' + message[index] + '</a></li>');
        });
        //$("#users").append(options);
    }

    chatHubClient.kickOut = function () {
        alert("you are kicked out!");
    }
});