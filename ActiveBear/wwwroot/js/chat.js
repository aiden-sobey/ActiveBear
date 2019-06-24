"use strict";

var ReceiveMessage = "ReceiveMessage";
var ReceiveAllMessages= "ReceiveAllMessages";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var messageInput = document.getElementById("messageInput");
var passwordInput = document.getElementById("passwordInput");
var messageContainer = document.getElementById("container");

// Send actions

messageInput.addEventListener("keydown", function(event) {
	if (event.key === "Enter")
		ChatHub.PostMessage();
});

// Receive actions

connection.on(ReceiveMessage, function (message) {
	Messager.CreateMessageBubble(message);
});

connection.on(ReceiveAllMessages, function (message) {
	Messager.CreateMessageBubble("JavaScript not implemented yet.");
});


/*		Connection		*/


connection.start().then(function(){
	AuthenticateChannel();

}).catch(function (err) {
	return console.error(err.toString());
});


/*		Helper methods		*/


function PasswordEmptyError() {
	alert("Key can't be empty! Otherwise your message won't be encrypted.");
	messageInput.value = "";
}
