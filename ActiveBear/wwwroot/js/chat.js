"use strict";


/*		Constants		*/


// JS-Accessible functions
var ReceiveMessage = "ReceiveMessage";
var ReceiveAllMessages= "ReceiveAllMessages";

// Server-side functions
var SendMessage = "SendMessage";
var GetChannelMessages = "GetChannelMessages";

// Runtime variables
var currentUser = document.cookie.split(" User=")[1].split(" ")[0];
var currentChannel = window.location.href.split("/Channel/Engage/")[1].substring(0, 36);
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// File elements
var sendButton = document.getElementById("sendButton");
var messageInput = document.getElementById("messageInput");
var messageContainer = document.getElementById("container");


/*		Messages		*/


// Send
sendButton.addEventListener("click", function (event) {
	PostMessage();
    event.preventDefault();
});

// Receive
connection.on(ReceiveMessage, function (message) {
	CreateMessageBubble(message);
	updateScroll();
});

// Recieve All
connection.on(ReceiveAllMessages, function (message) {
	CreateMessageBubble("JavaScript not implemented yet.");
	updateScroll();
});


/*		Connection		*/


connection.start().then(function(){
	sendButton.disabled = false;
	sendButton.value = "Send";

	// Get all existing messages
    connection.invoke(GetChannelMessages, GenerateChannelPacket()).catch(function (err) {
        return console.error(err.toString());
    });

}).catch(function (err) {
    return console.error(err.toString());
});


/*		Helper methods		*/


function updateScroll(){
    messageContainer.scrollTop = messageContainer.scrollHeight;
}

function PostMessage() {
	var message = messageInput.value;
	if (message === null || message === "") return;

    var messagePacket = GenerateMessagePacket(message);
    connection.invoke(SendMessage, messagePacket).catch(function (err) {
        return console.error(err.toString());
    });
}

function CreateMessageBubble(message) {
	if (message === null || message === "") return;

    var messageBubble = document.createElement("div");
	messageBubble.setAttribute('class', 'message_contents');
    messageBubble.textContent = message;
    messageContainer.appendChild(messageBubble);
}

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
