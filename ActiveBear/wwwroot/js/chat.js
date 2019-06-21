"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
var sendButton = document.getElementById("sendButton");
sendButton.disabled = true;

/*		Messages		*/

// Send
document.getElementById("sendButton").addEventListener("click", function (event) {
	// TODO: check if empty
    var message = document.getElementById("messageInput").value;
    var messagePacket = GenerateMessagePacket("Aiden", "TCIP", message);
    connection.invoke("SendMessage", messagePacket).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

// Receive
connection.on("ReceiveMessage", function (message) {
    //var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var messageBubble = document.createElement("div");
	messageBubble.setAttribute('class', 'message_contents');
    messageBubble.textContent = message;
    document.getElementById("container").appendChild(messageBubble);
});

// Recieve All
connection.on("ReceiveAllMessages", function (message) {
    //var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var messageBubble = document.createElement("div");
	messageBubble.setAttribute('class', 'message_contents');
    messageBubble.textContent = message;
    document.getElementById("container").appendChild(messageBubble);
});

/*		Connection		*/

connection.start().then(function(){
    document.getElementById("sendButton").disabled = false;

	// Get all existing messages
    connection.invoke("GetChannelMessages", GenerateChannelPacket()).catch(function (err) {
        return console.error(err.toString());
    });

}).catch(function (err) {
    return console.error(err.toString());
});



/*		Helper methods		*/

function GenerateMessagePacket(userCookie, channel, message) {
	var packet = {
		"UserCookie": userCookie,
		"Channel": channel,
		"Message": message
    }
	return JSON.stringify({ MessagePacket: packet  });
}

function GenerateChannelPacket() {
	var packet = {
		"UserCookie": "Aiden",
		"Channel": "34c34235-84a1-40fa-840d-19f6ede9a8cc"
	}

	return JSON.stringify({ ChannelPacket: packet });
}
