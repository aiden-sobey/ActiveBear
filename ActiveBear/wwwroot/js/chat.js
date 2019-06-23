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
var channelKey = "";
var messageInput = document.getElementById("messageInput");
var passwordInput = document.getElementById("passwordInput");
var messageContainer = document.getElementById("container");


/*		Messages		*/


// Send
messageInput.addEventListener("keydown", function(event) {
	if (event.key === "Enter")
		PostMessage();
});
passwordInput.addEventListener("keydown", function(event) {
	if (event.key === "Enter") {
		RemoveAllMessages();
		RequestAllMessages();
	}
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
	AuthenticateChannel();
	//RequestAllMessages();

}).catch(function (err) {
	return console.error(err.toString());
});

function AuthenticateChannel() {
	channelKey = prompt("Please enter a channel key:", "password123");
	passwordInput.value = channelKey; // TODO: store this somewhere more secure than HTML DOM
	messageInput.hidden = false;
	RequestAllMessages();
}


/*		Helper methods		*/


function RequestAllMessages() {
	connection.invoke(GetChannelMessages, GenerateChannelPacket()).catch(function (err) {
		return console.error(err.toString());
	});
}

function updateScroll() {
	messageContainer.scrollTop = messageContainer.scrollHeight;
}

function PostMessage() {
	if (message === null || message === "") return;
	if (passwordInput.value === "" || passwordInput.value === null)
		return PasswordEmptyError();

	var message = AesEncrypt(messageInput.value);
	var messagePacket = GenerateMessagePacket(message);
	connection.invoke(SendMessage, messagePacket).catch(function (err) {
		return console.error(err.toString());
	});
	messageInput.value = "";
}

function PasswordEmptyError() {
	alert("Key can't be empty! Otherwise your message won't be encrypted.");
	messageInput.value = "";
}

function CreateMessageBubble(message) {
	if (message === null || message === "") return;
	var decryptedText = AesDecrypt(message);
	if (decryptedText === null) return;

	var messageBubble = document.createElement("div");
	messageBubble.setAttribute('class', 'message_contents');
	messageBubble.textContent = decryptedText;
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
	var key = HashedChannelPassword();
	var iv = [ 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 ];

	// Convert text to bytes
	var textBytes = aesjs.utils.utf8.toBytes(text);

	var aesOfb = new aesjs.ModeOfOperation.ofb(key, iv);
	var encryptedBytes = aesOfb.encrypt(textBytes);

	return aesjs.utils.hex.fromBytes(encryptedBytes);
}

function AesDecrypt(encryptedHex) {
	var key = HashedChannelPassword();
	var iv = [ 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 ];
		
	var encryptedBytes = aesjs.utils.hex.toBytes(encryptedHex);
	var aesOfb = new aesjs.ModeOfOperation.ofb(key, iv);

	try {
		var decryptedBytes = aesOfb.decrypt(encryptedBytes);
		return aesjs.utils.utf8.fromBytes(decryptedBytes);
	}
	catch (err) {
		//return encryptedHex;
		return null;
	}
}

function HashedChannelPassword() {
	// Get the password entered by the user and hash it to 256 bits
	var hashedPassword = sha256(passwordInput.value);
	return aesjs.utils.hex.toBytes(hashedPassword);
}

function RemoveAllMessages() {
	while (messageContainer.firstChild) {
		messageContainer.removeChild(messageContainer.firstChild);
	}
}
