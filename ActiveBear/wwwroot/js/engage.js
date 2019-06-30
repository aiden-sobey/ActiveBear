﻿"use strict";

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

connection.on(ReceiveAllMessages, function (messageList) {
	var messages = JSON.parse(messageList);
	messages.Result.forEach(function(message) {
		Messager.CreateMessageBubble(message.EncryptedContents);
	});
});

// Connection

connection.start().then(function(){
	Messager.AuthenticateChannel();
	ChatHub.RequestAllMessages();

}).catch(function (err) {
	return console.error(err.toString());
});
