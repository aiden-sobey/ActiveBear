"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Runtime variables
var currentUser = document.cookie.split(" User=")[1].split(" ")[0];
var currentChannel = window.location.href.split("/Channel/Engage/")[1].substring(0, 36);

//Disable send button until connection is established
var sendButton = document.getElementById("sendButton");
sendButton.disabled = true;

/*		Messages		*/

// Send
document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    var messagePacket = GenerateMessagePacket(message);
    connection.invoke("SendMessage", messagePacket).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

// Receive
connection.on("ReceiveMessage", function (message) {
	// Deserialize
	if (message == null)
		return;
    var messageBubble = document.createElement("div");
	messageBubble.setAttribute('class', 'message_contents');
	//TODO: decrypt
    messageBubble.textContent = message;

    document.getElementById("container").appendChild(messageBubble);
});

// Recieve All
connection.on("ReceiveAllMessages", function (message) {
    var messageBubble = document.createElement("div");
	messageBubble.setAttribute('class', 'message_contents');
    messageBubble.textContent = "JavaScript not implemented yet.";
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

function GenerateMessagePacket(message) {
	var packet = {
		UserCookie: currentUser,
		Channel: currentChannel,
		Message: message
    }
	return JSON.stringify(packet);
}

function GenerateChannelPacket() {
	var packet = {
		UserCookie: currentUser,
		Channel: currentChannel
	}

	return JSON.stringify(packet);
}
