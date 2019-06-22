"use strict";


/*		Constants		*/


// JS-Accessible functions
var ReceiveMessage = "ReceiveMessage";
var ReceiveAllMessages= "ReceiveAllMessages";

// Server-side functions
var SendMessage = "SendMessage";
var GetChannelMessages = "GetChannelMessages";

// Runtime variables
var currentUser = document.cookie.split("User=")[1].split("; ")[0];
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
messageInput.addEventListener("keydown", function(event) {
	if (event.key === "Enter")
		PostMessage();
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
	var message = AesEncrypt(messageInput.value);
	if (message === null || message === "") return;

	var messagePacket = GenerateMessagePacket(message);
	connection.invoke(SendMessage, messagePacket).catch(function (err) {
		return console.error(err.toString());
	});
	messageInput.value = "";
}

function CreateMessageBubble(message) {
	if (message === null || message === "") return;

	var messageBubble = document.createElement("div");
	messageBubble.setAttribute('class', 'message_contents');
	messageBubble.textContent = AesDecrypt(message);
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

function AesEncrypt(text) {
	// 128-bit key
	var key = [ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 ];

	// The initialization vector (must be 16 bytes)
	var iv = [ 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 ];

	// Convert text to bytes
	var textBytes = aesjs.utils.utf8.toBytes(text);

	var aesOfb = new aesjs.ModeOfOperation.ofb(key, iv);
	var encryptedBytes = aesOfb.encrypt(textBytes);

	return aesjs.utils.hex.fromBytes(encryptedBytes);
}

function AesDecrypt(encryptedHex) {
		// 128-bit key
		var key = [ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 ];

		// The initialization vector (must be 16 bytes)
		var iv = [ 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 ];
		
		var encryptedBytes = aesjs.utils.hex.toBytes(encryptedHex);

		// The output feedback mode of operation maintains internal state,
		// so to decrypt a new instance must be instantiated.
		var aesOfb = new aesjs.ModeOfOperation.ofb(key, iv);

	try {
		var decryptedBytes = aesOfb.decrypt(encryptedBytes);
		return aesjs.utils.utf8.fromBytes(decryptedBytes);
	}
	catch (err) {
		return encryptedHex;
	}
}
